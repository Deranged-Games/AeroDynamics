using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using Sandbox.Definitions;
using System.Text.RegularExpressions;
using System.Globalization;
using Digi.Utils;

namespace Dondelium.PlanetDynamics{
	public class HeatData{
		public int thresholdMod;
		public float damageMod;
		
		public HeatData(float inDamage, int inThres){
			thresholdMod = inThres;
			damageMod = inDamage;
		}
	}
	
	public class HeatDefinition{
		public Dictionary<string, HeatData> data = new Dictionary<string, HeatData>();
		public void Init(){
			Log.Info("HeatDefintion Initalizing:");
			//Basically Heat shields.
		    try{
				data.Add("LargeHeavyBlockArmorBlock",				new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorSlope",				new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorCorner",				new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorCornerInv",			new HeatData(0.2f, 2000));
			
				data.Add("SmallHeavyBlockArmorBlock",				new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorSlope",				new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorCorner",				new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorCornerInv",			new HeatData(0.2f, 2000));
			
				data.Add("LargeHeavyBlockArmorRoundedSlope",		new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorRoundedCorner",		new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorAngledSlope",			new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorAngledCorner",		new HeatData(0.2f, 2000));
			
				data.Add("SmallHeavyBlockArmorRoundedSlope",		new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorRoundedCorner",		new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorAngledSlope",			new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorAngledCorner",		new HeatData(0.2f, 2000));
			
				data.Add("LargeHeavyBlockArmorRoundSlope",			new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorRoundCorner",			new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorRoundCornerInv",		new HeatData(0.2f, 2000));
			
				data.Add("SmallHeavyBlockArmorRoundSlope",			new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorRoundCorner",			new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorRoundCornerInv",		new HeatData(0.2f, 2000));
			
				data.Add("LargeHeavyBlockArmorSlope2BaseSmooth",	new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorSlope2TipSmooth",		new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorCorner2BaseSmooth", 	new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorCorner2TipSmooth",	new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorInvCorner2BaseSmooth", new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorInvCorner2TipSmooth", new HeatData(0.2f, 2000));
			
				data.Add("SmallHeavyBlockArmorSlope2BaseSmooth",	new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorSlope2TipSmooth",		new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorCorner2BaseSmooth", 	new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorCorner2TipSmooth",	new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorInvCorner2BaseSmooth", new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorInvCorner2TipSmooth", new HeatData(0.2f, 2000));
			
				data.Add("LargeHeavyBlockArmorSlope2Base", 			new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorSlope2Tip", 			new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorCorner2Base", 		new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorCorner2Tip", 			new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorInvCorner2Base", 		new HeatData(0.2f, 2000));
				data.Add("LargeHeavyBlockArmorInvCorner2Tip", 		new HeatData(0.2f, 2000));
			
				data.Add("SmallHeavyBlockArmorSlope2Base", 			new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorSlope2Tip", 			new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorCorner2Base", 		new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorCorner2Tip", 			new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorInvCorner2Base", 		new HeatData(0.2f, 2000));
				data.Add("SmallHeavyBlockArmorInvCorner2Tip", 		new HeatData(0.2f, 2000));
			
				//data.Add("ArmorAlpha", 							new HeatData(0.2f, 1000));
				data.Add("ArmorCenter", 							new HeatData(0.2f, 2000));
				data.Add("ArmorCorner", 							new HeatData(0.2f, 2000));
				data.Add("ArmorInvCorner", 							new HeatData(0.2f, 2000));
				data.Add("ArmorSide", 								new HeatData(0.2f, 2000));
			
				//Default heating does well with light armor.
				//data.Add("LargeBlockArmorBlock",					new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorSlope",					new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorCorner",					new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorCornerInv",				new HeatData(1.0f, 1000));
				//data.Add("LargeRoundArmor_Slope",					new HeatData(1.0f, 1000));
				//data.Add("LargeRoundArmor_Corner",				new HeatData(1.0f, 1000));
				//data.Add("LargeRoundArmor_CornerInv",				new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorBlock",					new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorSlope",					new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorCorner",					new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorCornerInv",				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorRoundedSlope",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorRoundedCorner",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorAngledSlope",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorAngledCorner",			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorRoundedSlope",			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorRoundedCorner",			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorAngledSlope",			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorAngledCorner",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorRoundSlope",				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorRoundCorner",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorRoundCornerInv",			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorRoundSlope",				new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorRoundCorner",			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorRoundCornerInv",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorSlope2BaseSmooth",		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorSlope2TipSmooth",		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorCorner2BaseSmooth",		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorCorner2TipSmooth",		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorInvCorner2BaseSmooth",	new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorInvCorner2TipSmooth",	new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorSlope2BaseSmooth",		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorSlope2TipSmooth",		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorCorner2BaseSmooth",		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorCorner2TipSmooth",		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorInvCorner2BaseSmooth",	new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorInvCorner2TipSmooth",	new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorSlope2Base",				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorSlope2Tip",				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorCorner2Base",			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorCorner2Tip",				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorInvCorner2Base", 		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockArmorInvCorner2Tip", 			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorSlope2Base", 			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorSlope2Tip", 				new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorCorner2Base", 			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorCorner2Tip", 			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorInvCorner2Base", 		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockArmorInvCorner2Tip", 			new HeatData(1.0f, 1000));

				//Some items should be well shielded.
				data.Add("LargeBlockBatteryBlock",		new HeatData(2.5f, 800));
				data.Add("SmallBlockBatteryBlock",		new HeatData(2.5f, 800));
				data.Add("SmallProgrammableBlock", 		new HeatData(5.0f, 300));
				data.Add("LargeProgrammableBlock", 		new HeatData(5.0f, 300));
				data.Add("ControlPanel", 				new HeatData(5.0f, 300));
				data.Add("SmallControlPanel", 			new HeatData(5.0f, 300));
				data.Add("LargeWarhead", 				new HeatData(50.0f, 250));		//Warheads are highly violatile.
				data.Add("SmallWarhead", 				new HeatData(50.0f, 250));
				data.Add("LargeBlockInteriorWall", 		new HeatData(1.5f, 600));
				data.Add("LargeInteriorPillar", 		new HeatData(1.5f, 450));
				data.Add("LargeRefinery", 				new HeatData(1.2f, 850));
				data.Add("Blast Furnace", 				new HeatData(1.5f, 700));
				data.Add("LargeAssembler", 				new HeatData(1.6f, 750));
				data.Add("LargeSteelCatwalk", 			new HeatData(2.0f, 350));
				data.Add("LargeSteelCatwalk2Sides", 	new HeatData(2.0f, 350));
				data.Add("LargeSteelCatwalkCorner", 	new HeatData(2.0f, 350));
				data.Add("LargeSteelCatwalkPlate", 		new HeatData(2.0f, 350));
				data.Add("LargeMedicalRoom", 			new HeatData(3.0f, 700));
				data.Add("LargeJumpDrive", 				new HeatData(2.0f, 850));
				data.Add("LargeBlockCockpitSeat", 		new HeatData(2.0f, 450));
				data.Add("LargeProjector", 				new HeatData(2.0f, 600));
				data.Add("SmallProjector", 				new HeatData(2.0f, 600));
				data.Add("PassengerSeatLarge", 			new HeatData(2.0f, 450));
				data.Add("PassengerSeatSmall", 			new HeatData(2.0f, 450));
				data.Add("SmallTextPanel", 				new HeatData(4.0f, 350));
				data.Add("SmallLCDPanelWide", 			new HeatData(4.0f, 350));
				data.Add("SmallLCDPanel", 				new HeatData(4.0f, 350));
				data.Add("LargeTextPanel", 				new HeatData(4.0f, 350));
				data.Add("LargeLCDPanel", 				new HeatData(4.0f, 350));
				data.Add("LargeLCDPanelWide", 			new HeatData(4.0f, 350));
				data.Add("LargeBlockSolarPanel", 		new HeatData(15.0f, 100));		//Solar panels rip off with the slightest of ease.
				data.Add("SmallBlockSolarPanel", 		new HeatData(15.0f, 100));
				data.Add("SmallBlockSmallContainer", 	new HeatData(1.5f, 750));
				data.Add("SmallBlockMediumContainer", 	new HeatData(1.5f, 750));
				data.Add("SmallBlockLargeContainer", 	new HeatData(1.5f, 750));
				data.Add("LargeBlockSmallContainer", 	new HeatData(1.5f, 750));
				data.Add("LargeBlockLargeContainer", 	new HeatData(1.5f, 750));
				data.Add("SmallCameraBlock", 			new HeatData(0.5f, 1000));
				data.Add("LargeCameraBlock", 			new HeatData(0.5f, 1000));
				data.Add("LargeBlockGyro", 				new HeatData(10.0f, 750));
				data.Add("SmallBlockGyro", 				new HeatData(10.0f, 750));
				data.Add("SmallBlockSmallGenerator", 	new HeatData(2.0f, 750));
				data.Add("SmallBlockLargeGenerator", 	new HeatData(3.0f, 1200));
				data.Add("LargeBlockSmallGenerator", 	new HeatData(2.0f, 750));
				data.Add("LargeBlockLargeGenerator", 	new HeatData(3.0f, 1200));
				data.Add("ButtonPanelLarge", 			new HeatData(3.0f, 750));
				data.Add("ButtonPanelSmall", 			new HeatData(3.0f, 750));
				data.Add("TimerBlockLarge", 			new HeatData(3.0f, 800));
				data.Add("TimerBlockSmall", 			new HeatData(3.0f, 800));
				data.Add("VirtualMassLarge", 			new HeatData(2.0f, 850));
				data.Add("VirtualMassSmall", 			new HeatData(2.0f, 850));
			
				//While some should be re-entry friendly
				data.Add("LargeBlockLandingGear", 		new HeatData(0.6f, 1800));
				data.Add("LargeBlockCockpit", 			new HeatData(0.8f, 1300));
				data.Add("DBSmallBlockFighterCockpit", 	new HeatData(0.7f, 1450));
				data.Add("SmallBlockCockpit", 			new HeatData(0.8f, 1300));
				data.Add("SmallBlockLandingGear", 		new HeatData(0.6f, 1800));
				data.Add("SmallBlockDrill", 			new HeatData(0.5f, 1100));
				data.Add("LargeBlockDrill", 			new HeatData(0.5f, 1100));
			
				//Tanks, Made to deal with high pressure and heat, but once ruptured break real easy.
				data.Add("LargeHydrogenTank", 			new HeatData(8.0f, 1500));
				data.Add("SmallHydrogenTank", 			new HeatData(8.0f, 1500));
				data.Add("OxygenTankSmall", 			new HeatData(3.0f, 1300));
				data.Add("OxygenGeneratorSmall", 		new HeatData(3.0f, 1300));
			
				//Thrusters get special love depending.
				data.Add("SmallBlockSmallThrust", 			new HeatData(3f, 1800));	//Ions do not like overheating.
				data.Add("SmallBlockLargeThrust", 			new HeatData(3f, 1800));	//But are energy so it takes alot to cook them.
				data.Add("LargeBlockSmallThrust", 			new HeatData(3f, 1800));
				data.Add("LargeBlockLargeThrust", 			new HeatData(3f, 1800));
				data.Add("LargeBlockLargeHydrogenThrust", 	new HeatData(0.5f, 2000));	//Hydrogen are built for heat.
				data.Add("LargeBlockSmallHydrogenThrust", 	new HeatData(0.5f, 2000));
				data.Add("SmallBlockLargeHydrogenThrust", 	new HeatData(0.5f, 2000));
				data.Add("SmallBlockSmallHydrogenThrust", 	new HeatData(0.5f, 2000));
				data.Add("LargeBlockLargeAtmosphericThrust", new HeatData(0.6f, 800)); 	//Atmos handle heat well, but overheat easy.
				data.Add("LargeBlockSmallAtmosphericThrust", new HeatData(0.6f, 800));
				data.Add("SmallBlockLargeAtmosphericThrust", new HeatData(0.6f, 800));
				data.Add("SmallBlockSmallAtmosphericThrust", new HeatData(0.6f, 800));
			
				//Conveyors are not heat resistant.
				data.Add("ConveyorTube", 					new HeatData(2.0f, 700));
				data.Add("ConveyorTubeSmall", 				new HeatData(2.0f, 700));
				data.Add("ConveyorTubeMedium", 				new HeatData(2.0f, 700));
				data.Add("ConveyorFrameMedium", 			new HeatData(2.0f, 700));
				data.Add("ConveyorTubeCurved", 				new HeatData(2.0f, 700));
				data.Add("ConveyorTubeSmallCurved", 		new HeatData(2.0f, 700));
				data.Add("ConveyorTubeCurvedMedium", 		new HeatData(2.0f, 700));
				data.Add("SmallShipConveyorHub", 			new HeatData(2.0f, 700));
				data.Add("LargeBlockConveyorSorter", 		new HeatData(2.0f, 700));
				data.Add("MediumBlockConveyorSorter", 		new HeatData(2.0f, 700));
				data.Add("SmallBlockConveyorSorter", 		new HeatData(2.0f, 700));
			
				//data.Add("SmallGatlingTurret", 			new HeatData(1.0f, 1000));
				//data.Add("SmallMissileTurret", 			new HeatData(1.0f, 1000));
				//data.Add("LargeInteriorTurret", 			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockRadioAntenna", 		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockBeacon", 				new HeatData(1.0f, 1000));
				//data.Add("SmallBlockBeacon", 				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockFrontLight", 			new HeatData(1.0f, 1000));
				//data.Add("SmallLight", 					new HeatData(1.0f, 1000));
				//data.Add("LargeWindowSquare", 			new HeatData(1.0f, 1000));
				//data.Add("LargeWindowEdge", 				new HeatData(1.0f, 1000));
				//data.Add("LargeStairs", 					new HeatData(1.0f, 1000));
				//data.Add("LargeRamp", 					new HeatData(1.0f, 1000));
				//data.Add("LargeCoverWall", 				new HeatData(1.0f, 1000));
				//data.Add("LargeCoverWallHalf", 			new HeatData(1.0f, 1000));
				//data.Add("LargeDecoy", 					new HeatData(1.0f, 1000));
				//data.Add("SmallDecoy", 					new HeatData(1.0f, 1000));
				//data.Add("LargeOreDetector", 				new HeatData(1.0f, 1000));
				//data.Add("CockpitOpen", 					new HeatData(1.0f, 1000));
				//data.Add("LargeBlockCryoChamber", 		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockFrontLight", 			new HeatData(1.0f, 1000));
				//data.Add("LargeMissileLauncher", 			new HeatData(1.0f, 1000));
				//data.Add("SmallRocketLauncherReload", 	new HeatData(1.0f, 1000));
				//data.Add("SmallBlockOreDetector", 		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockSensor", 				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockSensor", 				new HeatData(1.0f, 1000));
				//data.Add("SmallBlockSoundBlock", 			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockSoundBlock", 			new HeatData(1.0f, 1000));
				//data.Add("SmallBlockRadioAntenna", 		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockRemoteControl", 		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockRemoteControl", 		new HeatData(1.0f, 1000));
				//data.Add("SmallAirVent", 					new HeatData(1.0f, 1000));
				//data.Add("LargeProductivityModule", 		new HeatData(1.0f, 1000));
				//data.Add("LargeEffectivenessModule", 		new HeatData(1.0f, 1000));
				//data.Add("LargeEnergyModule", 			new HeatData(1.0f, 1000));
				//data.Add("LargePistonBase", 				new HeatData(1.0f, 1000));
				//data.Add("LargePistonTop", 				new HeatData(1.0f, 1000));
				//data.Add("SmallPistonBase", 				new HeatData(1.0f, 1000));
				//data.Add("SmallPistonTop", 				new HeatData(1.0f, 1000));
				//data.Add("LargeStator", 					new HeatData(1.0f, 1000));
				//data.Add("Suspension3x3", 				new HeatData(1.0f, 1000));
				//data.Add("Suspension5x5", 				new HeatData(1.0f, 1000));
				//data.Add("Suspension1x1", 				new HeatData(1.0f, 1000));
				//data.Add("SmallSuspension3x3", 			new HeatData(1.0f, 1000));
				//data.Add("SmallSuspension5x5", 			new HeatData(1.0f, 1000));
				//data.Add("SmallSuspension1x1", 			new HeatData(1.0f, 1000));
				//data.Add("LargeRotor",					new HeatData(1.0f, 1000));
				//data.Add("SmallStator", 					new HeatData(1.0f, 1000));
				//data.Add("SmallRotor", 					new HeatData(1.0f, 1000));
				//data.Add("LargeAdvancedStator", 			new HeatData(1.0f, 1000));
				//data.Add("LargeAdvancedRotor", 			new HeatData(1.0f, 1000));
				//data.Add("SmallAdvancedStator", 			new HeatData(1.0f, 1000));
				//data.Add("SmallAdvancedRotor", 			new HeatData(1.0f, 1000));
				//data.Add("LargeRailStraight", 			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockOxygenFarm", 			new HeatData(1.0f, 1000));
				//data.Add("Window1x2Slope", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x2Inv", 					new HeatData(1.0f, 1000));
				//data.Add("Window1x2Face", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x2SideLeft", 			new HeatData(1.0f, 1000));
				//data.Add("Window1x2SideRight", 			new HeatData(1.0f, 1000));
				//data.Add("Window1x1Slope", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x1Face", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x1Side", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x1Inv", 					new HeatData(1.0f, 1000));
				//data.Add("Window1x2Flat", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x2FlatInv", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x1Flat", 				new HeatData(1.0f, 1000));
				//data.Add("Window1x1FlatInv", 				new HeatData(1.0f, 1000));
				//data.Add("Window3x3Flat", 				new HeatData(1.0f, 1000));
				//data.Add("Window3x3FlatInv", 				new HeatData(1.0f, 1000));
				//data.Add("Window2x3Flat", 				new HeatData(1.0f, 1000));
				//data.Add("Window2x3FlatInv", 				new HeatData(1.0f, 1000));
				//data.Add("SmallBlockConveyor", 			new HeatData(1.0f, 1000));
				//data.Add("LargeBlockConveyor", 			new HeatData(1.0f, 1000));
				//data.Add("Collector", 					new HeatData(1.0f, 1000));
				//data.Add("CollectorSmall", 				new HeatData(1.0f, 1000));
				//data.Add("Connector", 					new HeatData(1.0f, 1000));
				//data.Add("ConnectorSmall", 				new HeatData(1.0f, 1000));
				//data.Add("ConnectorMedium", 				new HeatData(1.0f, 1000));
				//data.Add("SpaceBallLarge", 				new HeatData(1.0f, 1000));
				//data.Add("SpaceBallSmall", 				new HeatData(1.0f, 1000));
				//data.Add("SmallRealWheel1x1", 			new HeatData(1.0f, 1000));
				//data.Add("SmallRealWheel", 				new HeatData(1.0f, 1000));
				//data.Add("SmallRealWheel5x5", 			new HeatData(1.0f, 1000));
				//data.Add("RealWheel1x1", 					new HeatData(1.0f, 1000));
				//data.Add("RealWheel", 					new HeatData(1.0f, 1000));
				//data.Add("RealWheel5x5", 					new HeatData(1.0f, 1000));
				//data.Add("Wheel1x1", 						new HeatData(1.0f, 1000));
				//data.Add("SmallWheel1x1", 				new HeatData(1.0f, 1000));
				//data.Add("Wheel3x3", 						new HeatData(1.0f, 1000));
				//data.Add("SmallWheel3x3", 				new HeatData(1.0f, 1000));
				//data.Add("Wheel5x5", 						new HeatData(1.0f, 1000));
				//data.Add("SmallWheel5x5", 				new HeatData(1.0f, 1000));
				//data.Add("LargeShipGrinder", 				new HeatData(1.0f, 1000));
				//data.Add("SmallShipGrinder", 				new HeatData(1.0f, 1000));
				//data.Add("LargeShipWelder", 				new HeatData(1.0f, 1000));
				//data.Add("SmallShipWelder", 				new HeatData(1.0f, 1000));
				//data.Add("LargeShipMergeBlock", 			new HeatData(1.0f, 1000));
				//data.Add("SmallShipMergeBlock", 			new HeatData(1.0f, 1000));
				//data.Add("SmallArmorCenter", 				new HeatData(1.0f, 1000));
				//data.Add("SmallArmorCorner", 				new HeatData(1.0f, 1000));
				//data.Add("SmallArmorInvCorner", 			new HeatData(1.0f, 1000));
				//data.Add("SmallArmorSide", 				new HeatData(1.0f, 1000));
				//data.Add("LargeBlockLaserAntenna", 		new HeatData(1.0f, 1000));
				//data.Add("SmallBlockLaserAntenna", 		new HeatData(1.0f, 1000));
				//data.Add("LargeBlockSlideDoor", 			new HeatData(1.0f, 1000));
				
				//-----------------------------------------------------------------------
				//--------------- Entering mod specific area ----------------------------
				//-----------------------------------------------------------------------
				
				data.Add("LargeConcreteArmorBlock",				new HeatData(5.0f, 1200));
				data.Add("LargeConcreteArmorSlope",				new HeatData(5.0f, 1200));
				data.Add("LargeConcreteArmorCorner",			new HeatData(5.0f, 1200));
				data.Add("LargeConcreteArmorCornerInv",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorRoundedSlope",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorRoundedCorner",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorAngledSlope",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorAngledCorner",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorRoundSlope",				new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorRoundCorner",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorRoundCornerInv",			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorSlope2BaseSmooth",		new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorSlope2TipSmooth",		new HeatData(5.0f, 1200)); //For mod ID: 303959338
				data.Add("ConcreteArmorCorner2BaseSmooth", 		new HeatData(5.0f, 1200)); //Concrete is no where near as good as Steel.
				data.Add("ConcreteArmorCorner2TipSmooth",		new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorInvCorner2BaseSmooth", 	new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorInvCorner2TipSmooth", 	new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorSlope2Base", 			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorSlope2Tip", 				new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorCorner2Base", 			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorCorner2Tip", 			new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorInvCorner2Base", 		new HeatData(5.0f, 1200));
				data.Add("ConcreteArmorInvCorner2Tip", 			new HeatData(5.0f, 1200));
				data.Add("LargeConcreteSlab", 					new HeatData(5.0f, 1200));
				
				data.Add("SuperLargeHeavyBlockArmorSlope", 		new HeatData(0.1f, 3000));
				data.Add("SuperLargeHeavyBlockArmorCorner", 	new HeatData(0.1f, 3000));
				data.Add("SuperLargeHeavyBlockArmorCornerInv", 	new HeatData(0.1f, 3000)); //For mod ID:571203255
				data.Add("SuperSmallHeavyBlockArmorBlock", 		new HeatData(0.1f, 3000)); //Super heavy armor mod. Basically a no re-entry damage mod.
				data.Add("SuperSmallHeavyBlockArmorSlope", 		new HeatData(0.1f, 3000));
				data.Add("SuperSmallHeavyBlockArmorCorner", 	new HeatData(0.1f, 3000));
				data.Add("SuperSmallHeavyBlockArmorCornerInv", 	new HeatData(0.1f, 3000));
				
				Log.Info("HeatDefinition Initialized, "+data.Count+" Definitions loaded.");
			} catch(Exception e){
				Log.Error("HeatDefinition Error: "+e);
				Log.Info("HeatDefinition Error location: "+(data.Count+1));
			}
		}
	}
}
