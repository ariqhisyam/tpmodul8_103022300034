using System;
using System.IO;
using System.Text.Json;

class CovidConfig
{
    public string CONFIG1 { get; set; } = "celcius";
    public int CONFIG2 { get; set; } = 14;
    public string CONFIG3 { get; set; } = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
    public string CONFIG4 { get; set; } = "Anda dipersilahkan untuk masuk ke dalam gedung ini";

    public void UbahSatuan()
    {
        CONFIG1 = CONFIG1 == "celcius" ? "fahrenheit" : "celcius";
    }

    public static CovidConfig LoadConfig(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<CovidConfig>(json) ?? new CovidConfig();
        }
        return new CovidConfig();
    }

    public void SaveConfig(string filePath)
    {
        string json = JsonSerializer.Serialize(this);
        File.WriteAllText(filePath, json);
    }
}

class Program
{
    static void Main()
    {
        string configPath = "config.json";
        CovidConfig config = CovidConfig.LoadConfig(configPath);

        while (true)
        {
            Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.CONFIG1}");
            double suhuBadan = double.Parse(Console.ReadLine());

            Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
            int hariGejala = int.Parse(Console.ReadLine());

            bool suhuValid = config.CONFIG1 == "celcius"
                ? suhuBadan >= 36.5 && suhuBadan <= 37.5
                : suhuBadan >= 97.7 && suhuBadan <= 99.5;

            if (suhuValid && hariGejala < config.CONFIG2)
            {
                Console.WriteLine(config.CONFIG4);
            }
            else
            {
                Console.WriteLine(config.CONFIG3);
            }

            Console.WriteLine("Apakah Anda ingin mengubah satuan suhu? (y/n)");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                config.UbahSatuan();
                config.SaveConfig(configPath);
                Console.WriteLine($"Satuan suhu telah diubah menjadi {config.CONFIG1}");
            }
            else
            {
                break;
            }
        }
    }
}
