namespace SistemaImobiliaria.Models;

public abstract class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    protected Pessoa() { }

    protected Pessoa(int id, string nome, string cpf, string telefone, string email)
    {
        Id = id;
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Email = email;
    }

    public abstract string TipoUsuario();

    public override string ToString() =>
        $"[{Id}] {Nome} | CPF: {CPF} | Tel: {Telefone} | Email: {Email}";
}
