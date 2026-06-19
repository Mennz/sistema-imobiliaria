namespace SistemaImobiliaria.Models;

public enum StatusVisita { Agendada, Realizada, Cancelada }

public class Visita
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int ImovelId { get; set; }
    public int? CorretorId { get; set; }
    public DateTime DataHora { get; set; }
    public string Observacoes { get; set; } = string.Empty;

    private StatusVisita _status = StatusVisita.Agendada;
    public StatusVisita Status
    {
        get => _status;
        private set => _status = value;
    }

    public Visita() { }

    public Visita(int id, int clienteId, int imovelId, DateTime dataHora, int? corretorId = null, string observacoes = "")
    {
        Id = id;
        ClienteId = clienteId;
        ImovelId = imovelId;
        DataHora = dataHora;
        CorretorId = corretorId;
        Observacoes = observacoes;
    }

    public void Realizar()
    {
        if (_status != StatusVisita.Agendada)
            throw new InvalidOperationException("Apenas visitas agendadas podem ser marcadas como realizadas.");
        _status = StatusVisita.Realizada;
    }

    public void Cancelar()
    {
        if (_status == StatusVisita.Realizada)
            throw new InvalidOperationException("Não é possível cancelar uma visita já realizada.");
        _status = StatusVisita.Cancelada;
    }

    public override string ToString() =>
        $"[{Id}] Cliente #{ClienteId} → Imóvel #{ImovelId} | " +
        $"Data: {DataHora:dd/MM/yyyy HH:mm} | Status: {Status}" +
        (CorretorId.HasValue ? $" | Corretor #{CorretorId}" : "");
}
