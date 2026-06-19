namespace SistemaImobiliaria.Models;

public class Apartamento : Imovel
{
    public int Andar { get; set; }
    public decimal ValorCondominio { get; set; }
    public bool TemElevador { get; set; }
    public bool TemAreaLazer { get; set; }

    public Apartamento() { }

    public Apartamento(int id, Endereco endereco, decimal valor, double areaM2,
        int quartos, int banheiros, bool temGaragem, TipoNegocio tipoNegocio,
        double iptu, int corretorId, int andar, decimal valorCondominio,
        bool temElevador, bool temAreaLazer)
        : base(id, endereco, valor, areaM2, quartos, banheiros, temGaragem, tipoNegocio, iptu, corretorId)
    {
        Andar = andar;
        ValorCondominio = valorCondominio;
        TemElevador = temElevador;
        TemAreaLazer = temAreaLazer;
    }

    public override decimal CalcularCustosMensais() => (decimal)IPTU + ValorCondominio;

    public override string TipoImovel() => "Apartamento";

    public override string ToString() =>
        base.ToString() +
        $" | Andar: {Andar}º | Cond: R${ValorCondominio:N2} | Elevador: {(TemElevador ? "Sim" : "Não")} | Lazer: {(TemAreaLazer ? "Sim" : "Não")}";
}
