using System;
using System.IO;

namespace SystemMonitoring
{
    class FileManager
    {
        public static string GetPathConfig()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = $@"{appdataPath}\SystemMonitoring\config.txt";
            if (!File.Exists(path))
            {
                if (!Directory.Exists(appdataPath + @"\SystemMonitoring\"))
                    Directory.CreateDirectory(appdataPath + @"\SystemMonitoring\");
                File.Create(path).Dispose();
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i <= 3; i++) writer.WriteLine();
                }
            }
            string config = appdataPath + @"\SystemMonitoring\config.txt";
            return config;
        }
    }
}
