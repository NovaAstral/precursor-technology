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

namespace PrecursorHandheldBattery{
	
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
		
	public class HealthEnergyConsumables : MySessionComponentBase{
		
		int tickTimer = 0;
		bool scriptInit = false;
		 
		MyObjectBuilder_PhysicalGunObject energyHalf;
		
		public override void UpdateBeforeSimulation(){
			if(scriptInit == false){
				scriptInit = true;
				var definitionId = new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), "PrecursorHandheldBattery");
				energyHalf = (MyObjectBuilder_PhysicalGunObject)MyObjectBuilderSerializer.CreateNewObject(definitionId);
			}
			tickTimer++;
			if(tickTimer < 180){
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
				var energy = MyVisualScriptLogicProvider.GetPlayersEnergyLevel(player.IdentityId);
				if(energy < 0.98f){
					var Inv = player.Character.GetInventory();
					if(Inv.ContainItems(1, energyHalf) == true){
						MyVisualScriptLogicProvider.SetPlayersEnergyLevel(player.IdentityId,1f);
					}
				}
			}
		}
	}
}