namespace SistemaImobiliaria.Models;

public class Endereco
{
    public string Rua { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;

    public Endereco() { }

    public Endereco(string rua, string numero, string bairro, string cep, string cidade)
    {
        Rua = rua;
        Numero = numero;
        Bairro = bairro;
        CEP = cep;
        Cidade = cidade;
    }

    public override string ToString() =>
        $"{Rua}, {Numero} - {Bairro}, {Cidade} - CEP: {CEP}";
}
