namespace SistemaImobiliaria.Models;

public enum StatusImovel { Disponivel, Alugado, Vendido }
public enum TipoNegocio { Venda, Aluguel }

public abstract class Imovel
{
    public int Id { get; set; }
    public Endereco Endereco { get; set; } = new();
    public decimal Valor { get; set; }
    public double AreaM2 { get; set; }
    public int Quartos { get; set; }
    public int Banheiros { get; set; }
    public bool TemGaragem { get; set; }
    public TipoNegocio TipoNegocio { get; set; }
    public double IPTU { get; set; }
    public int CorretorId { get; set; }

    private StatusImovel _status = StatusImovel.Disponivel;
    public StatusImovel Status
    {
        get => _status;
        private set => _status = value;
    }

    protected Imovel() { }

    protected Imovel(int id, Endereco endereco, decimal valor, double areaM2,
        int quartos, int banheiros, bool temGaragem, TipoNegocio tipoNegocio,
        double iptu, int corretorId)
    {
        Id = id;
        Endereco = endereco;
        Valor = valor;
        AreaM2 = areaM2;
        Quartos = quartos;
        Banheiros = banheiros;
        TemGaragem = temGaragem;
        TipoNegocio = tipoNegocio;
        IPTU = iptu;
        CorretorId = corretorId;
    }

    public void MarcarComoAlugado()
    {
        if (_status != StatusImovel.Disponivel)
            throw new InvalidOperationException("Imóvel não está disponível para aluguel.");
        _status = StatusImovel.Alugado;
    }

    public void MarcarComoVendido()
    {
        if (_status != StatusImovel.Disponivel)
            throw new InvalidOperationException("Imóvel não está disponível para venda.");
        _status = StatusImovel.Vendido;
    }

    public void MarcarComoDisponivel()
    {
        _status = StatusImovel.Disponivel;
    }
    
    public abstract decimal CalcularCustosMensais();

    public abstract string TipoImovel();

    public override string ToString() =>
        $"[{Id}] {TipoImovel()} | {Endereco} | Valor: R${Valor:N2} | " +
        $"Status: {Status} | {TipoNegocio} | {Quartos} qts, {Banheiros} bhs";
}
