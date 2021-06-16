using System.Collections.Generic;

namespace SystemMonitoring
{
    class DB
    {
        public static string DistrictName { get; set; } 
        public static double SizeWindow { get; set; }
        public static string Path { get; set; }
        public static string ConfigName { get; set; } = "settings";
        public static string ConfigFormat { get; set; } = "json";
        public static string DirectoryName { get; set; } = "SystemMonitoring";
        public static Seeding SelectSeeding { get; set; }
        public static List<SensorDetails> Childs { get; set; }
    }
}