using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Sandbox.Common;
using Sandbox.Common.Components;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game;
using VRage.Common.Utils;
using VRageMath;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Library.Utils;
using Digi.Utils;

using System.Text.RegularExpressions;
using Ingame = Sandbox.ModAPI.Ingame;

namespace Dondelium.PlanetDynamics{

  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class PlanetDynamics : MySessionComponentBase{
    //Planet Variables
    public static Dictionary<long, MyPlanet> planets = new Dictionary<long, MyPlanet>();
    public static List<long> removePlanets = new List<long>();
    //Planet update check.
    private int skip = 0;
    private const int SKIP_TICKS = 180;
    
    //-------------------------------------------------------------------------------
    //Global variables used for, well, global reference 
    //-------------------------------------------------------------------------------
    public static float ATMOSPHERE_CALCTHRESHOLD = 0.02f;

    public static int REENTRY_VELOCITY_BASE_INIT = 300; //Lower Atmo base speed for Atmospheric heating.
    public static int REENTRY_VELOCITY_BASE_AMP = 150; //Heating per 100m/s over INIT. Done at atmosphere threshold. (Modified by atmosphere, and angle from velocity)
    public static float REENTRY_VELOCITY_MOD = 0.7f; //Atmosphere level where BASE velocity values begin to curve toward END values.
    public static int REENTRY_VELOCITY_END_INIT = 250; //Speed where heating occurs at 0.02 atmosphere.
    public static int REENTRY_VELOCITY_END_AMP = 100; //Heating per 100m/s over INIT. Done at atmosphere threshold. (Modified by atmosphere, and angle from velocity)

    public static float REENTRY_DISIPATION_BASE = 0.25f; //Percent of stored heat released a second.
    public static float REENTRY_DISIPATION_MOD = 0.9f; //Atmosphere level where BASE disipation values begin to curve toward END values.
    public static float REENTRY_DISIPATION_END = 0.05f; //Percent of stored heat released a second.
    
    public static int REENTRY_DAMAGE_HEAT = 1000; //Default heat level where damage begins to occur.

    public static float DRAGVALUE = 1.0f; //Multiplier for ships.
    //-------------------------------------------------------------------------------
    
    //Used to handle AtmosphericGrid entities. 
    private static HashSet<IMyEntity> ents = new HashSet<IMyEntity>(); // this is always empty
    public Dictionary<long, AtmosphericGrid> dragDictionary = new Dictionary<long, AtmosphericGrid>();
    internal static Action UpdateHook;
    
    //Heat reference
    public HeatDefinition h_definitions = new HeatDefinition();
    
    //Admin values.
    private bool init = false;
    public static PlanetDynamics instance;
    
    public void Init(){}
    
    public override void UpdateAfterSimulation(){
      if(!init){
        Log.Info("PlanetDynamics Initalizing:");
        init = true;
        MyAPIGateway.Entities.GetEntities(ents, delegate (IMyEntity e) {
          Entities_OnEntityAdd(e);
          return false;
        });
        MyAPIGateway.Entities.OnEntityAdd += Entities_OnEntityAdd;
        MyAPIGateway.Entities.OnEntityRemove += Entities_OnEntityRemove;
        
        instance = this;
        h_definitions.Init();
        Log.Info("PlanetDynamics Initalized:");
      }
      
      if(UpdateHook != null)
        UpdateHook();
      
      if(++skip >= SKIP_TICKS){
        skip = 0;
        MyAPIGateway.Entities.GetEntities(ents, delegate(IMyEntity e){
          if(e is MyPlanet){
            if(!PlanetDynamics.planets.ContainsKey(e.EntityId)){
              PlanetDynamics.planets.Add(e.EntityId, e as MyPlanet);
            }
          }
          return false;
        });
      }
    }
    
    private void Entities_OnEntityRemove(IMyEntity obj){
      if(obj == null)
        return;
      if(obj is IMyCubeGrid){
        AtmosphericGrid drag;
        if(dragDictionary.TryGetValue(obj.EntityId, out drag)){
          drag.Close();
          dragDictionary.Remove(obj.EntityId);
        }
      }
    }

    private void Entities_OnEntityAdd(IMyEntity obj){
      if(obj == null)
        return;
      if(obj is IMyCubeGrid){
        AtmosphericGrid drag;
        if (dragDictionary.TryGetValue(obj.EntityId, out drag)){
          drag.Close();
          dragDictionary.Remove(obj.EntityId);
        }
        drag = new AtmosphericGrid(obj);
        dragDictionary.Add(obj.EntityId, drag);
      }
    }
    
    protected override void UnloadData(){
      Log.Info("Closing PlanetDynamics.");
      MyAPIGateway.Entities.OnEntityAdd -= Entities_OnEntityAdd;
      MyAPIGateway.Entities.OnEntityRemove -= Entities_OnEntityRemove;
      planets.Clear();
      removePlanets.Clear();
      dragDictionary.Clear();
      ents.Clear();
      Log.Close();
      UpdateHook = null;
    }
  }
}