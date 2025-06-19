namespace Utility;

public class ConfigReader
{
    public string ConfigFilePath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config" , "shopsync", "config.json");

    public DbConfig GetDatabaseConfig()
    {
        Config config = ReadConfig();
        return config.Database;
    }

    public List<ShopConfig> GetShopConfig()
    {
        Config config = ReadConfig();
        return config.Shops;
    }

    private Config ReadConfig()
    {
        if (!File.Exists(ConfigFilePath))
        {
            throw new FileNotFoundException($"Config file '{ConfigFilePath}' not found.");
        }

        string json = File.ReadAllText(ConfigFilePath);
        var config = System.Text.Json.JsonSerializer.Deserialize<Config>(json);
        if (config == null)
        {
            throw new InvalidDataException("Config file is invalid or empty.");
        }
        return config;
    }
}