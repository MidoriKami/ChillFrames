namespace ChillFrames.Utilities;

internal static class TerritoryIntendedUseHelper
{
    public static string GetUseDescription(byte territoryIntendedUse)
    {
        return territoryIntendedUse switch
        {
            0 => "City",
            1 => "Open World",
            2 => "Inn",
            3 => "Dungeon",
            8 => "Alliance Raid",
            10 => "Trial",
            13 or 14 => "Housing",
            16 or 17 => "Raid",
            30 => "Grand Company",
            31 => "PotD/HoH",
            41 => "Eureka",
            48 => "Bozja",

            _ => "Unknown"
        };
    }
}