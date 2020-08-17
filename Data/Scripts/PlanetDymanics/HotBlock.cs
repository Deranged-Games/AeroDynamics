using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRageMath;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Game.Components;
using Sandbox.Definitions;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;

using Digi.Utils;

namespace Dondelium.PlanetDynamics{
  public class HotBlock{
    public IMySlimBlock block;
    private IMyCubeGrid grid;
    private MyCubeGrid grid2;
    private float heat;
    private float modifier;
    private float dmgModifier;
    private Vector3 color;
    private readonly Vector3 COLOR_RED = new Vector3(0, 1, 1);
    private int threshold = 1000;

    public HotBlock(IMySlimBlock inBlock, MyCubeGrid inGrid, float inHeat){
      block = inBlock;
      grid2 = inGrid;
      grid = inGrid as IMyCubeGrid;
      heat = inHeat;
      color = block.GetColorMask();
      
      //Handle uneven size heating.
      Vector3 modVec = Vector3.Zero;
      block.ComputeScaledHalfExtents(out modVec);
      modVec = (modVec * 2) / grid.GridSize;
      modifier = (float)Math.Sqrt(modVec.X * modVec.Y * modVec.Z);
      
      //Make small, and large grid burn up more evenly, but not entirely ;-)
      dmgModifier = grid.GridSize * grid.GridSize * 1.5f; //*1.5 added because calculation went from every 20 frames to every 30 frames.
      threshold = PlanetDynamics.REENTRY_DAMAGE_HEAT;
    }
    
    public HotBlock(IMySlimBlock inBlock, MyCubeGrid inGrid, float inHeat, string subType){
      block = inBlock;
      grid2 = inGrid;
      grid = inGrid as IMyCubeGrid;
      heat = inHeat;
      color = block.GetColorMask();
      
      //Handle uneven size heating.
      Vector3 modVec = Vector3.Zero;
      block.ComputeScaledHalfExtents(out modVec);
      modVec = (modVec * 2) / grid.GridSize;
      modifier = (float)Math.Sqrt(modVec.X * modVec.Y * modVec.Z);
      
      //Make small, and large grid burn up more evenly, but not entirely ;-)
      dmgModifier = grid.GridSize * grid.GridSize * 1.5f; //*1.5 added because calculation went from every 20 frames to every 30 frames.
      
      //Handle subType
      try{
        HeatData heatType = null;
        if(PlanetDynamics.instance.h_definitions.data.TryGetValue(subType, out heatType)){
          threshold = heatType.thresholdMod;
          dmgModifier *= heatType.damageMod;
        } else {
          threshold = PlanetDynamics.REENTRY_DAMAGE_HEAT;
        }
      } catch(Exception e){
        Log.Error("HotBlock: Init "+e);
      }
    }

    public void AddHeat(float appliedHeat){
      heat += appliedHeat / modifier;
    }

    public float Disipate(float percent){
      if(block == null || block.IsDestroyed)
        return 0;
      heat -= heat * percent;
      if(heat < (percent * 50)){
        heat = 0;
        grid2.ColorBlocks(block.Position, block.Position, color, false, false);
      } else {
        float colorValue = MathHelper.Clamp(heat,0,PlanetDynamics.REENTRY_DAMAGE_HEAT);
        Vector3 currentColor = Vector3.Lerp(color,COLOR_RED,(colorValue / PlanetDynamics.REENTRY_DAMAGE_HEAT));
        grid2.ColorBlocks(block.Position, block.Position, currentColor, false, false);
        if(heat > threshold){
          IMyDestroyableObject damageTarget = block as IMyDestroyableObject;
          damageTarget.DoDamage((heat - threshold) * dmgModifier, Sandbox.Common.ObjectBuilders.Definitions.MyDamageType.Environment, false);
        }
      }

      return heat;
    }
  }
}