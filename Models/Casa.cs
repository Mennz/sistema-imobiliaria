namespace SistemaImobiliaria.Models;

public class Casa : Imovel
{
    public bool TemQuintal { get; set; }
    public double AreaQuintalM2 { get; set; }
    public bool TemAreaExterna { get; set; }

    public Casa() { }

    public Casa(int id, Endereco endereco, decimal valor, double areaM2,
        int quartos, int banheiros, bool temGaragem, TipoNegocio tipoNegocio,
        double iptu, int corretorId, bool temQuintal, double areaQuintalM2, bool temAreaExterna)
        : base(id, endereco, valor, areaM2, quartos, banheiros, temGaragem, tipoNegocio, iptu, corretorId)
    {
        TemQuintal = temQuintal;
        AreaQuintalM2 = areaQuintalM2;
        TemAreaExterna = temAreaExterna;
    }

    public override decimal CalcularCustosMensais() => (decimal)IPTU;

    public override string TipoImovel() => "Casa";

    public override string ToString() =>
        base.ToString() +
        $" | Quintal: {(TemQuintal ? $"Sim ({AreaQuintalM2}m²)" : "Não")} | Área Externa: {(TemAreaExterna ? "Sim" : "Não")}";
}
