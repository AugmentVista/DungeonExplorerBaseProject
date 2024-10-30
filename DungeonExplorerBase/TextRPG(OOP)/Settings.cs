using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace TextRPG_OOP_
{
    /// <summary>
    /// Holds the base values for all Character
    /// </summary>
    internal class Settings
    {
        // Base values for all stats. 
        public int PlasmoidBaseHP { get; set; } = 3;
        public int PlasmoidBaseDamage { get; set; } = 0;
        public int ConstructBaseHP { get; set; } = 3;
        public int ConstructBaseDamage { get; set; } = 0;
        public int GoblinFolkBaseHP { get; set; } = 3;
        public int GoblinFolkBaseDamage { get; set; } = 0;

        /// <summary>
        /// Player stats are static as there is only ever one player
        /// </summary>

        // Configuration properties
        public int playerMaxHPConfig { get; set; } = 25;
        public int playerMaxArmourConfig { get; set; } = 5;
        public int playerMaxDmgConfig { get; set; } = 10;

        // Static properties
        public static int playerMaxHP { get; set; } = 25;
        public static int playerMaxArmour { get; set; } = 5;
        public static int playerMaxDamage { get; set; } = 10;

        public class StaticSettingsDTO
        {
            public int PlayerMaxHP { get; set; }
            public int PlayerMaxArmour { get; set; }
            public int PlayerMaxDamage { get; set; }
        }

        public void SaveSettings()
        {
            string filePath = "settingsdata.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);

            // Save static settings as well
            SaveStaticSettings(this); // Pass the current instance
        }

        public static void SaveStaticSettings(Settings settingsInstance)
        {
            var staticSettings = new StaticSettingsDTO
            {
                PlayerMaxHP = settingsInstance.playerMaxHPConfig, // Access instance property
                PlayerMaxArmour = settingsInstance.playerMaxArmourConfig, // Access instance property
                PlayerMaxDamage = settingsInstance.playerMaxDmgConfig // Access instance property
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(staticSettings, options);
            File.WriteAllText("staticsettings.json", json);
        }

        public static void LoadStaticSettings()
        {
            string filePath = "staticsettings.json";

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var staticSettings = JsonSerializer.Deserialize<StaticSettingsDTO>(json);

                playerMaxHP = staticSettings.PlayerMaxHP;
                playerMaxArmour = staticSettings.PlayerMaxArmour;
                playerMaxDamage = staticSettings.PlayerMaxDamage;
            }
        }

        public static Settings LoadSettings()
        {
            string filePath = "settingsdata.json";
            var json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<Settings>(json);
            LoadStaticSettings();

            return settings; // Return the deserialized settings object
        }
    }
}
