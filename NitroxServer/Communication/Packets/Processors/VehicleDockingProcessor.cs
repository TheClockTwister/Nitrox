﻿using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel.Packets;
using NitroxServer.Communication.Packets.Processors.Abstract;
using NitroxServer.GameLogic;
using NitroxServer.GameLogic.Vehicles;

namespace NitroxServer.Communication.Packets.Processors
{
    class VehicleDockingProcessor : AuthenticatedPacketProcessor<VehicleDocking>
    {
        private readonly PlayerManager playerManager;
        private readonly VehicleManager vehicleManager;

        public VehicleDockingProcessor(PlayerManager playerManager, VehicleManager vehicleManager)
        {
            this.playerManager = playerManager;
            this.vehicleManager = vehicleManager;
        }

        public override void Process(VehicleDocking packet, Player player)
        {
            Optional<VehicleModel> vehicle = vehicleManager.GetVehicleModel(packet.VehicleId);

            if (!vehicle.IsPresent())
            {
                Log.Error("VehicleDocking received for vehicle id {0} that does not exist!", packet.VehicleId);
                return;
            }

            VehicleModel vehicleModel = vehicle.Get();
            vehicleModel.DockingBayId = Optional<NitroxId>.Of(packet.DockId);

            playerManager.SendPacketToOtherPlayers(packet, player);
        }
    }
}
