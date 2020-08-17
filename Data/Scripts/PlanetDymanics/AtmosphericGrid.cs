using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRageMath;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.ObjectBuilders;

using Digi.Utils;

namespace Dondelium.PlanetDynamics{
  public class AtmosphericGrid{
    private IMyEntity Entity;
    private MyObjectBuilder_EntityBase objectBuilder;
    private MyCubeGrid grid;
    private IMyCubeGrid iGrid;
    private bool active = false;
    private int count = 0;

    private float atmosphere = 0;
    private Vector3D vecMod = Vector3D.Zero;
    private Vector3 faceMod = Vector3.Zero;
    private int blocks = 0;

    private List<IMySlimBlock> leftFace = new List<IMySlimBlock>();
    private List<IMySlimBlock> rightFace = new List<IMySlimBlock>();
    private List<IMySlimBlock> topFace = new List<IMySlimBlock>();
    private List<IMySlimBlock> bottomFace = new List<IMySlimBlock>();
    private List<IMySlimBlock> frontFace = new List<IMySlimBlock>();
    private List<IMySlimBlock> backFace = new List<IMySlimBlock>();

    private Dictionary<Vector3I, HotBlock> hotBlocks = new Dictionary<Vector3I, HotBlock>();
    private List<Vector3I> hotBlockRemove = new List<Vector3I>();

    private int heavyCalcValue = 2; //Set like this for... reasons.
    
    MyParticleEffect heatEffect = null;
    
    public AtmosphericGrid(IMyEntity obj){
      Entity = obj;
      grid = Entity as MyCubeGrid;
      iGrid = Entity as IMyCubeGrid;
      
      if(grid == null)
        return;
      active = true;
      
      PlanetDynamics.UpdateHook += Update;
      iGrid.OnClosing += onClose;
    }

    internal void Close(){
      if(PlanetDynamics.UpdateHook != null)
        PlanetDynamics.UpdateHook -= Update;
    }
    
    public void Update(){
      if(grid == null || grid.Closed || grid.MarkedForClose || grid.Physics == null || grid.Physics.IsStatic)
        return;
      
      count++;
      if(count >= 10){
        count = 0;
        RunAtmosphericCalculation();
        HeavyCalcUpdate();
      }
      
      if(grid.BlocksCount < 2)
        return;
      
      float speedSqd = grid.Physics.LinearVelocity.LengthSquared();
      if(atmosphere >= PlanetDynamics.ATMOSPHERE_CALCTHRESHOLD && speedSqd > 9){
        float dragMod = speedSqd * 0.5f * atmosphere * PlanetDynamics.DRAGVALUE;
        Vector3D xVec = grid.WorldMatrix.Left * (-vecMod.X * faceMod.X);
        Vector3D yVec = grid.WorldMatrix.Up * (-vecMod.Y * faceMod.Y);
        Vector3D zVec = grid.WorldMatrix.Forward * (-vecMod.Z * faceMod.Z);
        Vector3D force = (xVec + yVec + zVec) * dragMod;
        iGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, force, iGrid.Physics.CenterOfMassWorld, null);
      }
    }
    
    public void HeavyCalcUpdate(){
      if(grid == null || grid.Closed || grid.MarkedForClose || grid.Physics == null)  //Removed, bad, trash, and static reduction
        return;
      
      //I built this section because I have 3 heavy calculation sections. All of which were on a timer of 10-20 frames. Now, I heave better control of which ones are done, and when they are done.
      if(heavyCalcValue >= 2) heavyCalcValue = 0;
      else heavyCalcValue++;
      
      float speed = grid.Physics.LinearVelocity.Length();
      switch(heavyCalcValue){
        case 0:    //Math for creating the vector modifier, and the face cross-section multiplier.
          if(grid.Physics.IsStatic || grid.BlocksCount < 1)
            return;
          if(atmosphere >= PlanetDynamics.ATMOSPHERE_CALCTHRESHOLD && speed > 3){
            vecMod = new Vector3D(  Vector3.Dot(grid.WorldMatrix.Left, grid.Physics.LinearVelocity), 
                        Vector3.Dot(grid.WorldMatrix.Up, grid.Physics.LinearVelocity), 
                        Vector3.Dot(grid.WorldMatrix.Forward, grid.Physics.LinearVelocity)) / speed;
            if(blocks != grid.BlocksCount){
              GenerateFaces();
              blocks = grid.BlocksCount;
            }
          }
          break;
            
        case 1:    //Re-Entry heating check.
          if(grid.Physics.IsStatic || grid.BlocksCount < 1)
            return;
          if(atmosphere >= PlanetDynamics.ATMOSPHERE_CALCTHRESHOLD && speed > PlanetDynamics.REENTRY_VELOCITY_BASE_INIT){
            float heatAdd = 0;
            float heatVelocity = PlanetDynamics.REENTRY_VELOCITY_BASE_INIT;
            float heatValue = PlanetDynamics.REENTRY_VELOCITY_BASE_AMP;
            if(atmosphere < PlanetDynamics.REENTRY_VELOCITY_MOD){
              float heatAtmoMod = 1 - ((atmosphere - PlanetDynamics.ATMOSPHERE_CALCTHRESHOLD) / (PlanetDynamics.REENTRY_VELOCITY_MOD - PlanetDynamics.ATMOSPHERE_CALCTHRESHOLD));
              heatVelocity = PlanetDynamics.REENTRY_VELOCITY_BASE_INIT + (PlanetDynamics.REENTRY_VELOCITY_END_INIT - PlanetDynamics.REENTRY_VELOCITY_BASE_INIT) * heatAtmoMod;
              heatValue = PlanetDynamics.REENTRY_VELOCITY_BASE_AMP + (PlanetDynamics.REENTRY_VELOCITY_END_AMP - PlanetDynamics.REENTRY_VELOCITY_BASE_AMP) * heatAtmoMod;
            }
            if(speed > heatVelocity){
              float speedMod = (speed - heatVelocity) / 100;
              heatAdd = ((speedMod * speedMod) / 2) * (heatValue * atmosphere);
              if(heatAdd > 10){
                if(vecMod.X > 0)
                  ApplyHeat(leftFace, (heatAdd * (float)vecMod.X) / 2);
                else 
                  ApplyHeat(rightFace, (-heatAdd * (float)vecMod.X) / 2);
                if(vecMod.Y < 0)
                  ApplyHeat(topFace, (-heatAdd * (float)vecMod.Y) / 2);
                else 
                  ApplyHeat(bottomFace, (heatAdd * (float)vecMod.Y) / 2);
                if(vecMod.Z > 0)
                  ApplyHeat(frontFace, (heatAdd * (float)vecMod.Z) / 2);
                else 
                  ApplyHeat(backFace, (-heatAdd * (float)vecMod.Z) / 2);
              }
              
              if (heatAdd > 20.0f && heatEffect == null && MyParticlesManager.TryCreateParticleEffect(502, out heatEffect, false)){}
              if (heatEffect != null){
                float scaleMultiplier = (float)Math.Pow(grid.BlocksCount,(1.0/3.0)) * MathHelper.Clamp(heatAdd / 150.0f, 0.0f, 2.0f);

                MatrixD locationMatrix = grid.WorldMatrix;
                locationMatrix.Translation = grid.Physics.CenterOfMassWorld;
                heatEffect.WorldMatrix = locationMatrix;
                heatEffect.Velocity = grid.Physics.LinearVelocity;
                heatEffect.UserScale = scaleMultiplier * 2.0f * grid.GridSize;
                //heatEffect.LowRes = true;
              }  
            } else if(heatEffect != null){
              heatEffect.Stop();
              heatEffect = null;
            }
            try{
              if (MyAPIGateway.Session.ControlledObject.Entity.Parent.EntityId == Entity.EntityId)
                MyAPIGateway.Utilities.ShowNotification("Warning: Heating detected! " + heatAdd, 490, MyFontEnum.Red);
            } catch (Exception e) {} //Ugh this game. 
          } else if(heatEffect != null){
            heatEffect.Stop();
            heatEffect = null;
          }
          break;
        
        case 2:    //HotBlock cooling.
          if(hotBlocks.Count > 0){
            float dissipation = PlanetDynamics.REENTRY_DISIPATION_BASE;
            if(atmosphere < PlanetDynamics.REENTRY_DISIPATION_MOD){
              float disiAtmoMod = atmosphere / PlanetDynamics.REENTRY_DISIPATION_MOD;
              dissipation = PlanetDynamics.REENTRY_DISIPATION_END + (PlanetDynamics.REENTRY_DISIPATION_BASE - PlanetDynamics.REENTRY_DISIPATION_END) * disiAtmoMod;
            }
  
            foreach(var id in hotBlocks){
              if(id.Value.block == null || id.Value.block.IsDestroyed){
                hotBlockRemove.Add(id.Key);
                continue;
              }
              HotBlock hotBlock = id.Value;
              float heat = hotBlock.Disipate(dissipation / 2);
              if(heat <= 0){
                hotBlockRemove.Add(id.Key);
                continue;
              }
            }
          }
          if(hotBlockRemove.Count > 0){
            foreach(var id in hotBlockRemove)
              hotBlocks.Remove(id); 
            hotBlockRemove.Clear();
          }
          break;
      }
    }

    private void RunAtmosphericCalculation(){
      var gridCenter = grid.Physics.CenterOfMassWorld;
      atmosphere = 0;
      foreach(var kv in PlanetDynamics.planets){
        var planet = kv.Value;
        if(planet.Closed || planet.MarkedForClose || !planet.HasAtmosphere){
          PlanetDynamics.removePlanets.Add(kv.Key);
          continue;
        }
        if(Vector3D.DistanceSquared(gridCenter, planet.WorldMatrix.Translation) < (planet.AtmosphereRadius * planet.AtmosphereRadius)){
          atmosphere = planet.GetAirDensity(gridCenter);
          break;
        }
      }
      if(PlanetDynamics.removePlanets.Count > 0){
        foreach(var id in PlanetDynamics.removePlanets)
          PlanetDynamics.planets.Remove(id);  
        PlanetDynamics.removePlanets.Clear();
      }
    }
    
    private void ApplyHeat(List<IMySlimBlock> blocks, float heatAdd){
      HotBlock hotBlock = null;
      for(int i = 0; i < blocks.Count; i++){
        if(blocks[i] == null || blocks[i].IsDestroyed)
          continue;
        Vector3I blockID = blocks[i].Position;
        if(hotBlocks.TryGetValue(blockID, out hotBlock))
          hotBlock.AddHeat(heatAdd);
        else{
          var obj = blocks[i].GetCopyObjectBuilder();
          var subType = obj.SubtypeName;
          if(subType != "")
            hotBlock = new HotBlock(blocks[i], grid, heatAdd, subType);
          else
            hotBlock = new HotBlock(blocks[i], grid, heatAdd);
          hotBlocks.Add(blockID, hotBlock);
        }
      }
    }

    private void GenerateFaces(){
      int i = 0;  int j = 0;  int k = 0;
      int crossX = 0;  int crossY = 0;  int crossZ = 0;
      bool foundBlock = false;
      leftFace.Clear();
      rightFace.Clear();
      topFace.Clear();
      bottomFace.Clear();
      frontFace.Clear();
      backFace.Clear();

      //Do left face
      for(i = grid.Min.Y; i <= grid.Max.Y; i++){
        for(j = grid.Min.Z; j <= grid.Max.Z; j++){
          foundBlock = false;
          for(k = grid.Min.X; k <= grid.Max.X && !foundBlock; k++){
            if(grid.GetCubeBlock(new Vector3I(k,i,j)) != null){
              foundBlock = true;
              leftFace.Add(grid.GetCubeBlock(new Vector3I(k,i,j)));
              crossX++;
              break;
            }
          }
          if(foundBlock){ //I added in the opposite face check here because if the positive check didn't find anything, the negative check wouldn't either.
            for(k = grid.Max.X; k >= grid.Min.X; k--){
              if(grid.GetCubeBlock(new Vector3I(k,i,j)) != null){
                 rightFace.Add(grid.GetCubeBlock(new Vector3I(k,i,j)));
                 break;
              }
            }
          }
        }
      }
      //Do front face
      for(i = grid.Min.X; i <= grid.Max.X; i++){
        for(j = grid.Min.Z; j <= grid.Max.Z; j++){
          foundBlock = false;
          for(k = grid.Min.Y; k <= grid.Max.Y && !foundBlock; k++){
            if(grid.GetCubeBlock(new Vector3I(i,k,j)) != null){
              foundBlock = true;
              topFace.Add(grid.GetCubeBlock(new Vector3I(i,k,j)));
              crossY++;
              break;
            }
          }
          if(foundBlock){ //I added in the opposite face check here because if the positive check didn't find anything, the negative check wouldn't either.
            for(k = grid.Max.Y; k >= grid.Min.Y; k--){
              if(grid.GetCubeBlock(new Vector3I(i,k,j)) != null){
                bottomFace.Add(grid.GetCubeBlock(new Vector3I(i,k,j)));
                break;
              }
            }
          }
        }
      }
      //Do top face
      for(i = grid.Min.X; i <= grid.Max.X; i++){
        for(j = grid.Min.Y; j <= grid.Max.Y; j++){
          foundBlock = false;
          for(k = grid.Min.Z; k <= grid.Max.Z && !foundBlock; k++){
            if(grid.GetCubeBlock(new Vector3I(i,j,k)) != null){
              foundBlock = true;
              frontFace.Add(grid.GetCubeBlock(new Vector3I(i,j,k)));
              crossZ++;
              break;
            }
          }
          if(foundBlock){ //I added in the opposite face check here because if the positive check didn't find anything, the negative check wouldn't either.
            for(k = grid.Max.Z; k >= grid.Min.Z; k--){
              if(grid.GetCubeBlock(new Vector3I(i,j,k)) != null){
                backFace.Add(grid.GetCubeBlock(new Vector3I(i,j,k)));
                break;
              }
            }
          }
        }
      }
      faceMod = new Vector3(crossX,crossY,crossZ) * (grid.GridSize * grid.GridSize);
    }
    
    private void onClose(IMyEntity obj){
      try{
        iGrid.OnClosing -= onClose;
        
        if(heatEffect != null){
          heatEffect.Stop();
          heatEffect = null;
        }
        
        grid = null;
        iGrid = null;
        leftFace.Clear();
        rightFace.Clear();
        topFace.Clear();
        bottomFace.Clear();
        frontFace.Clear();
        backFace.Clear();

        hotBlocks.Clear();
        hotBlockRemove.Clear();
      } catch(Exception e){
        Log.Error("AtmosphericGrid: Close "+e);
      }
    }
  }
}