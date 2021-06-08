namespace SystemMonitoring
{
    class DB
    {
        public static string Host { get; set; } = "localhost";
        public static string Db { get; set; } = "informationSystem";
        public static string User { get; set; } = "root";
        public static string Password { get; set; } = "1234";
        public static uint Port { get; set; } = 3306;
        public static string Access { get; set; } = "User";
        public static string DistrictName { get; set; }
        public static string DistrictField { get; set; }
        public static string SelectDistrict { get; set; }   
        public static double SizeWindow { get; set; }
        public static string Path { get; set; }
        public static string ConfigName { get; set; } = "settings";
        public static string ConfigFormat { get; set; } = "json";
        public static string DirectoryName { get; set; } = "SystemMonitoring";
    }
}