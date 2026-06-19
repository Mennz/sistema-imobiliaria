namespace SistemaImobiliaria.Models;

public enum StatusProposta { Pendente, Aceita, Recusada }

public class Proposta
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int ImovelId { get; set; }
    public decimal ValorOfertado { get; set; }
    public DateTime DataProposta { get; set; }
    public string Observacoes { get; set; } = string.Empty;

    private StatusProposta _status = StatusProposta.Pendente;
    public StatusProposta Status
    {
        get => _status;
        private set => _status = value;
    }

    public Proposta() { }

    public Proposta(int id, int clienteId, int imovelId, decimal valorOfertado, string observacoes = "")
    {
        Id = id;
        ClienteId = clienteId;
        ImovelId = imovelId;
        ValorOfertado = valorOfertado;
        DataProposta = DateTime.Now;
        Observacoes = observacoes;
    }

    public void Aceitar()
    {
        if (_status != StatusProposta.Pendente)
            throw new InvalidOperationException("Apenas propostas pendentes podem ser aceitas.");
        _status = StatusProposta.Aceita;
    }

    public void Recusar()
    {
        if (_status != StatusProposta.Pendente)
            throw new InvalidOperationException("Apenas propostas pendentes podem ser recusadas.");
        _status = StatusProposta.Recusada;
    }

    public override string ToString() =>
        $"[{Id}] Cliente #{ClienteId} → Imóvel #{ImovelId} | " +
        $"Valor: R${ValorOfertado:N2} | Status: {Status} | Data: {DataProposta:dd/MM/yyyy}";
}
