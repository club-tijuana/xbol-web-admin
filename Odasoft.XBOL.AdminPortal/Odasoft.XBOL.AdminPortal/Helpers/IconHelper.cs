using MudBlazor;

namespace Odasoft.XBOL.AdminPortal.Helpers
{
    public static class IconHelper
    {
        public static string GetIconFromIdentifier(string identifier)
        {
            // TODO: We are using the identifiers from the DB, since this is the only place we use them,
            // we can change the identifiers to be the same as the icon names and remove this mapping.
            // That is way we also not using a constant class for the identifiers, since they are only used in the DB and here.
            return identifier switch
            {
                "Wifi" => Icons.Material.Outlined.Wifi,
                "Wc" => Icons.Material.Outlined.Wc,
                "DirectionsCar" => Icons.Material.Outlined.DirectionsCar,
                "Accessible" => Icons.Material.Outlined.Accessible,
                "FireExtinguisher" => Icons.Material.Outlined.FireExtinguisher,
                "Sensors" => Icons.Material.Outlined.Sensors,
                "SmokingRooms" => Icons.Material.Outlined.SmokingRooms,
                "DoorSliding" => Icons.Material.Outlined.DoorSliding,
                "MedicalServices" => Icons.Material.Outlined.MedicalServices,
                "Fastfood" => Icons.Material.Outlined.Fastfood,
                "Elevator" => Icons.Material.Outlined.Elevator,
                "Escalator" => Icons.Material.Outlined.Escalator,
                "Videocam" => Icons.Material.Outlined.Videocam,
                "Badge" => Icons.Material.Outlined.Badge,
                "DirectionsRun" => Icons.Material.Outlined.DirectionsRun,
                "Groups" => Icons.Material.Outlined.Groups,
                "BatteryChargingFull" => Icons.Material.Outlined.BatteryChargingFull,
                "Info" => Icons.Material.Outlined.Info,
                _ => Icons.Material.Outlined.CheckCircleOutline
            };
        }
    }
}
