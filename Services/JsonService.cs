using System.Text.Json;
using System.Text.Json.Serialization;
using SistemaImobiliaria.Models;

namespace SistemaImobiliaria.Services;

public class JsonService
{
    private readonly string _dataDir;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter(), new ImovelJsonConverter() }
    };

    public JsonService(string dataDir = "Data")
    {
        _dataDir = dataDir;
        Directory.CreateDirectory(_dataDir);
    }

    //Genérico
    private string Path<T>() => System.IO.Path.Combine(_dataDir, $"{typeof(T).Name}.json");

    public List<T> Carregar<T>() where T : class
    {
        var path = Path<T>();
        if (!File.Exists(path)) return new List<T>();
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
    }

    public void Salvar<T>(List<T> lista) where T : class
    {
        File.WriteAllText(Path<T>(), JsonSerializer.Serialize(lista, _options));
    }

    //Imovel (polimórfico)
    private string ImovelPath => System.IO.Path.Combine(_dataDir, "Imovel.json");

    public List<Imovel> CarregarImoveis()
    {
        if (!File.Exists(ImovelPath)) return new();
        var json = File.ReadAllText(ImovelPath);
        return JsonSerializer.Deserialize<List<Imovel>>(json, _options) ?? new();
    }

    public void SalvarImoveis(List<Imovel> imoveis)
    {
        File.WriteAllText(ImovelPath, JsonSerializer.Serialize(imoveis, _options));
    }
}

public class ImovelJsonConverter : JsonConverter<Imovel>
{
    public override Imovel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var tipo = root.GetProperty("TipoImovelTag").GetString();

        var innerOptions = new JsonSerializerOptions(options);
        innerOptions.Converters.Remove(innerOptions.Converters.First(c => c is ImovelJsonConverter));

        return tipo switch
        {
            "Casa" => root.Deserialize<Casa>(innerOptions),
            "Apartamento" => root.Deserialize<Apartamento>(innerOptions),
            _ => throw new JsonException($"Tipo desconhecido: {tipo}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Imovel value, JsonSerializerOptions options)
    {
        var innerOptions = new JsonSerializerOptions(options);
        innerOptions.Converters.Remove(innerOptions.Converters.First(c => c is ImovelJsonConverter));

        writer.WriteStartObject();
        writer.WriteString("TipoImovelTag", value.TipoImovel());

        // Serializa as propriedades do objeto concreto
        var json = value switch
        {
            Casa c => JsonSerializer.Serialize(c, innerOptions),
            Apartamento a => JsonSerializer.Serialize(a, innerOptions),
            _ => throw new JsonException("Tipo não suportado")
        };

        using var doc = JsonDocument.Parse(json);
        foreach (var prop in doc.RootElement.EnumerateObject())
            prop.WriteTo(writer);

        writer.WriteEndObject();
    }
}
