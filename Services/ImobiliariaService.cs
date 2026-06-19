using SistemaImobiliaria.Models;

namespace SistemaImobiliaria.Services;

public class ImobiliariaService
{
    private readonly JsonService _json;

    public List<Cliente> Clientes { get; private set; }
    public List<Corretor> Corretores { get; private set; }
    public List<Imovel> Imoveis { get; private set; }
    public List<Proposta> Propostas { get; private set; }
    public List<Visita> Visitas { get; private set; }

    public ImobiliariaService(JsonService json)
    {
        _json = json;
        Clientes = _json.Carregar<Cliente>();
        Corretores = _json.Carregar<Corretor>();
        Imoveis = _json.CarregarImoveis();
        Propostas = _json.Carregar<Proposta>();
        Visitas = _json.Carregar<Visita>();
    }

    //IDs 

    private int ProximoId<T>(List<T> lista) where T : class
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop == null) return 1;
        return lista.Count == 0 ? 1 : lista.Max(x => (int)prop.GetValue(x)!) + 1;
    }

    //Clientes 

    public Cliente CadastrarCliente(string nome, string cpf, string telefone, string email, string preferencia)
    {
        if (Clientes.Any(c => c.CPF == cpf))
            throw new InvalidOperationException("Já existe um cliente com esse CPF.");

        var cliente = new Cliente(ProximoId(Clientes), nome, cpf, telefone, email, preferencia);
        Clientes.Add(cliente);
        _json.Salvar(Clientes);
        return cliente;
    }

    public Cliente? BuscarCliente(int id) => Clientes.FirstOrDefault(c => c.Id == id);

    public void RemoverCliente(int id)
    {
        var cliente = BuscarCliente(id) ?? throw new KeyNotFoundException("Cliente não encontrado.");
        Clientes.Remove(cliente);
        _json.Salvar(Clientes);
    }

    //Corretores
    public Corretor CadastrarCorretor(string nome, string cpf, string telefone, string email, string creci)
    {
        if (Corretores.Any(c => c.CPF == cpf))
            throw new InvalidOperationException("Já existe um corretor com esse CPF.");
        if (Corretores.Any(c => c.CRECI == creci))
            throw new InvalidOperationException("Já existe um corretor com esse CRECI.");

        var corretor = new Corretor(ProximoId(Corretores), nome, cpf, telefone, email, creci);
        Corretores.Add(corretor);
        _json.Salvar(Corretores);
        return corretor;
    }

    public Corretor? BuscarCorretor(int id) => Corretores.FirstOrDefault(c => c.Id == id);

    //Imóveis

    public Casa CadastrarCasa(Endereco endereco, decimal valor, double areaM2,
        int quartos, int banheiros, bool temGaragem, TipoNegocio tipoNegocio,
        double iptu, int corretorId, bool temQuintal, double areaQuintal, bool temAreaExterna)
    {
        ValidarCorretor(corretorId);
        var id = ProximoId(Imoveis);
        var casa = new Casa(id, endereco, valor, areaM2, quartos, banheiros, temGaragem,
            tipoNegocio, iptu, corretorId, temQuintal, areaQuintal, temAreaExterna);
        Imoveis.Add(casa);
        AdicionarImovelAoCorretor(corretorId, id);
        _json.SalvarImoveis(Imoveis);
        return casa;
    }

    public Apartamento CadastrarApartamento(Endereco endereco, decimal valor, double areaM2,
        int quartos, int banheiros, bool temGaragem, TipoNegocio tipoNegocio,
        double iptu, int corretorId, int andar, decimal valorCondominio,
        bool temElevador, bool temAreaLazer)
    {
        ValidarCorretor(corretorId);
        var id = ProximoId(Imoveis);
        var apto = new Apartamento(id, endereco, valor, areaM2, quartos, banheiros, temGaragem,
            tipoNegocio, iptu, corretorId, andar, valorCondominio, temElevador, temAreaLazer);
        Imoveis.Add(apto);
        AdicionarImovelAoCorretor(corretorId, id);
        _json.SalvarImoveis(Imoveis);
        return apto;
    }

    public Imovel? BuscarImovel(int id) => Imoveis.FirstOrDefault(i => i.Id == id);

    public List<Imovel> BuscarImoveis(string? tipo = null, TipoNegocio? tipoNegocio = null,
        decimal? valorMin = null, decimal? valorMax = null, StatusImovel? status = null)
    {
        var query = Imoveis.AsEnumerable();
        if (tipo != null) query = query.Where(i => i.TipoImovel().ToLower() == tipo.ToLower());
        if (tipoNegocio.HasValue) query = query.Where(i => i.TipoNegocio == tipoNegocio);
        if (valorMin.HasValue) query = query.Where(i => i.Valor >= valorMin);
        if (valorMax.HasValue) query = query.Where(i => i.Valor <= valorMax);
        if (status.HasValue) query = query.Where(i => i.Status == status);
        return query.ToList();
    }

    public void RemoverImovel(int id)
    {
        var imovel = BuscarImovel(id) ?? throw new KeyNotFoundException("Imóvel não encontrado.");
        if (imovel.Status != StatusImovel.Disponivel)
            throw new InvalidOperationException("Não é possível remover imóvel que não está disponível.");
        Imoveis.Remove(imovel);
        _json.SalvarImoveis(Imoveis);
    }

    //Propostas

    public Proposta EnviarProposta(int clienteId, int imovelId, decimal valorOfertado, string obs = "")
    {
        var cliente = BuscarCliente(clienteId) ?? throw new KeyNotFoundException("Cliente não encontrado.");
        var imovel = BuscarImovel(imovelId) ?? throw new KeyNotFoundException("Imóvel não encontrado.");

        if (imovel.Status != StatusImovel.Disponivel)
            throw new InvalidOperationException("Este imóvel não está disponível.");

        // Regra: cliente só pode ter uma proposta pendente por imóvel
        if (Propostas.Any(p => p.ClienteId == clienteId && p.ImovelId == imovelId && p.Status == StatusProposta.Pendente))
            throw new InvalidOperationException("Você já possui uma proposta pendente para este imóvel.");

        var proposta = new Proposta(ProximoId(Propostas), clienteId, imovelId, valorOfertado, obs);
        Propostas.Add(proposta);
        _json.Salvar(Propostas);
        return proposta;
    }

    public void AceitarProposta(int propostaId)
    {
        var proposta = Propostas.FirstOrDefault(p => p.Id == propostaId)
            ?? throw new KeyNotFoundException("Proposta não encontrada.");
        var imovel = BuscarImovel(proposta.ImovelId)
            ?? throw new KeyNotFoundException("Imóvel não encontrado.");

        proposta.Aceitar();

        if (imovel.TipoNegocio == TipoNegocio.Aluguel)
            imovel.MarcarComoAlugado();
        else
            imovel.MarcarComoVendido();

        foreach (var outra in Propostas.Where(p => p.ImovelId == imovel.Id && p.Id != propostaId && p.Status == StatusProposta.Pendente))
            outra.Recusar();

        _json.Salvar(Propostas);
        _json.SalvarImoveis(Imoveis);
    }

    public void RecusarProposta(int propostaId)
    {
        var proposta = Propostas.FirstOrDefault(p => p.Id == propostaId)
            ?? throw new KeyNotFoundException("Proposta não encontrada.");
        proposta.Recusar();
        _json.Salvar(Propostas);
    }

    //Visitas

    public Visita AgendarVisita(int clienteId, int imovelId, DateTime dataHora, int? corretorId = null, string obs = "")
    {
        if (BuscarCliente(clienteId) == null) throw new KeyNotFoundException("Cliente não encontrado.");
        var imovel = BuscarImovel(imovelId) ?? throw new KeyNotFoundException("Imóvel não encontrado.");
        if (imovel.Status != StatusImovel.Disponivel)
            throw new InvalidOperationException("Imóvel não está disponível para visita.");
        if (dataHora <= DateTime.Now)
            throw new ArgumentException("A data da visita deve ser futura.");

        var visita = new Visita(ProximoId(Visitas), clienteId, imovelId, dataHora, corretorId, obs);
        Visitas.Add(visita);
        _json.Salvar(Visitas);
        return visita;
    }

    public void RealizarVisita(int visitaId)
    {
        var visita = Visitas.FirstOrDefault(v => v.Id == visitaId)
            ?? throw new KeyNotFoundException("Visita não encontrada.");
        visita.Realizar();
        _json.Salvar(Visitas);
    }

    public void CancelarVisita(int visitaId)
    {
        var visita = Visitas.FirstOrDefault(v => v.Id == visitaId)
            ?? throw new KeyNotFoundException("Visita não encontrada.");
        visita.Cancelar();
        _json.Salvar(Visitas);
    }

    //Helpers

    private void ValidarCorretor(int corretorId)
    {
        if (BuscarCorretor(corretorId) == null)
            throw new KeyNotFoundException("Corretor não encontrado.");
    }

    private void AdicionarImovelAoCorretor(int corretorId, int imovelId)
    {
        var corretor = BuscarCorretor(corretorId)!;
        corretor.ImoveisResponsavel.Add(imovelId);
        _json.Salvar(Corretores);
    }
}
