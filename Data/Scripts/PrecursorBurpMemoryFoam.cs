using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Common;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace PrecursorBurpMemoryFoam{
	
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
		
	public class HealthEnergyConsumables : MySessionComponentBase{
		
		int tickTimer = 0;
		bool scriptInit = false;
		 
		MyObjectBuilder_PhysicalGunObject energyHalf;
		
		public override void UpdateBeforeSimulation(){
			if(scriptInit == false){
				scriptInit = true;
				var definitionId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "PrecursorBurpMemoryFoam");
				energyHalf = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(definitionId);
			}
			tickTimer++;
			if(tickTimer < 60){
				return;
			}
			tickTimer = 0;
			var playerList = new List<IMyPlayer>();
			MyAPIGateway.Players.GetPlayers(playerList);

			if(playerList.Count == 0){
				return;
			}

			foreach(var player in playerList){
				if(player.IsBot == true){
					continue;
				}

				if(player.Character == null){
					continue;
				}

				var health = MyVisualScriptLogicProvider.GetPlayersHealth(player.IdentityId); 
				var oxygen = MyVisualScriptLogicProvider.GetPlayersOxygenLevel(player.IdentityId);
                var energy = MyVisualScriptLogicProvider.GetPlayersEnergyLevel(player.IdentityId);
				var hydrogen = MyVisualScriptLogicProvider.GetPlayersHydrogenLevel(player.IdentityId);
				
				var Inv = player.Character.GetInventory();

				if(health < 100f){
					if(Inv.ContainItems(1, energyHalf) == true){
						MyVisualScriptLogicProvider.SetPlayersHealth(player.IdentityId,100f);
					}
				}

				if(oxygen < 1f){
					if(Inv.ContainItems(1, energyHalf) == true){
						MyVisualScriptLogicProvider.SetPlayersOxygenLevel(player.IdentityId,1f);
					}
				}

                if(energy < 1f){
					if(Inv.ContainItems(1, energyHalf) == true){
						MyVisualScriptLogicProvider.SetPlayersEnergyLevel(player.IdentityId,1f);
					}
				}

				if(hydrogen < 1f){
					if(Inv.ContainItems(1, energyHalf) == true){
						MyVisualScriptLogicProvider.SetPlayersHydrogenLevel(player.IdentityId,1f);
					}
				}
			}
		}
	}
}