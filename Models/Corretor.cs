namespace SistemaImobiliaria.Models;

public class Corretor : Pessoa
{
    public string CRECI { get; set; } = string.Empty;
    public List<int> ImoveisResponsavel { get; set; } = new();

    public Corretor() { }

    public Corretor(int id, string nome, string cpf, string telefone, string email, string creci)
        : base(id, nome, cpf, telefone, email)
    {
        CRECI = creci;
    }

    public override string TipoUsuario() => "Corretor";

    public override string ToString() =>
        base.ToString() + $" | CRECI: {CRECI} | Imóveis: {ImoveisResponsavel.Count}";
}
