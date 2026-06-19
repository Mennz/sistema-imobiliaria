namespace SistemaImobiliaria.Models;

public class Cliente : Pessoa
{
    public string Preferencia { get; set; } = string.Empty;

    public Cliente() { }

    public Cliente(int id, string nome, string cpf, string telefone, string email, string preferencia)
        : base(id, nome, cpf, telefone, email)
    {
        Preferencia = preferencia;
    }

    public override string TipoUsuario() => "Cliente";

    public override string ToString() =>
        base.ToString() + $" | Interesse: {Preferencia}";
}
