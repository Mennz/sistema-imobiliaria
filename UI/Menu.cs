using SistemaImobiliaria.Models;
using SistemaImobiliaria.Services;

namespace SistemaImobiliaria.UI;

public static class Menu
{
    private static ImobiliariaService _svc = null!;

    public static void Iniciar(ImobiliariaService svc)
    {
        _svc = svc;
        while (true)
        {
            Console.Clear();
            Titulo("SISTEMA DE GESTÃO IMOBILIÁRIA");
            Console.WriteLine("  [1] Gerenciar Clientes");
            Console.WriteLine("  [2] Gerenciar Corretores");
            Console.WriteLine("  [3] Gerenciar Imóveis");
            Console.WriteLine("  [4] Propostas");
            Console.WriteLine("  [5] Visitas");
            Console.WriteLine("  [0] Sair");
            Console.Write("\n  Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": MenuClientes(); break;
                case "2": MenuCorretores(); break;
                case "3": MenuImoveis(); break;
                case "4": MenuPropostas(); break;
                case "5": MenuVisitas(); break;
                case "0": Console.WriteLine("\n  Até logo!"); return;
                default: Erro("Opção inválida."); break;
            }
        }
    }

    // ── Clientes ──────────────────────────────────────────────────────────────

    private static void MenuClientes()
    {
        while (true)
        {
            Console.Clear();
            Titulo("CLIENTES");
            Console.WriteLine("  [1] Listar clientes");
            Console.WriteLine("  [2] Cadastrar cliente");
            Console.WriteLine("  [3] Remover cliente");
            Console.WriteLine("  [0] Voltar");
            Console.Write("\n  Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ListarClientes(); break;
                case "2": CadastrarCliente(); break;
                case "3": RemoverCliente(); break;
                case "0": return;
                default: Erro("Opção inválida."); break;
            }
        }
    }

    private static void ListarClientes()
    {
        Console.Clear();
        Titulo("LISTA DE CLIENTES");
        if (!_svc.Clientes.Any()) { Info("Nenhum cliente cadastrado."); Pausar(); return; }
        foreach (var c in _svc.Clientes) Console.WriteLine($"  {c}");
        Pausar();
    }

    private static void CadastrarCliente()
    {
        Console.Clear();
        Titulo("CADASTRAR CLIENTE");
        try
        {
            var nome = LerObrigatorio("Nome");
            var cpf = LerCPF();
            var telefone = LerObrigatorio("Telefone");
            var email = LerObrigatorio("Email");
            var pref = LerOpcao("Interesse", new[] { "Compra", "Aluguel" });

            var c = _svc.CadastrarCliente(nome, cpf, telefone, email, pref);
            Sucesso($"Cliente '{c.Nome}' cadastrado com ID {c.Id}.");
        }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void RemoverCliente()
    {
        Console.Clear();
        Titulo("REMOVER CLIENTE");
        ListarClientes();
        int id = LerInt("ID do cliente a remover");
        try { _svc.RemoverCliente(id); Sucesso("Cliente removido."); }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    // ── Corretores ────────────────────────────────────────────────────────────

    private static void MenuCorretores()
    {
        while (true)
        {
            Console.Clear();
            Titulo("CORRETORES");
            Console.WriteLine("  [1] Listar corretores");
            Console.WriteLine("  [2] Cadastrar corretor");
            Console.WriteLine("  [0] Voltar");
            Console.Write("\n  Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ListarCorretores(); break;
                case "2": CadastrarCorretor(); break;
                case "0": return;
                default: Erro("Opção inválida."); break;
            }
        }
    }

    private static void ListarCorretores()
    {
        Console.Clear();
        Titulo("LISTA DE CORRETORES");
        if (!_svc.Corretores.Any()) { Info("Nenhum corretor cadastrado."); Pausar(); return; }
        foreach (var c in _svc.Corretores) Console.WriteLine($"  {c}");
        Pausar();
    }

    private static void CadastrarCorretor()
    {
        Console.Clear();
        Titulo("CADASTRAR CORRETOR");
        try
        {
            var nome = LerObrigatorio("Nome");
            var cpf = LerCPF();
            var telefone = LerObrigatorio("Telefone");
            var email = LerObrigatorio("Email");
            var creci = LerObrigatorio("CRECI");

            var c = _svc.CadastrarCorretor(nome, cpf, telefone, email, creci);
            Sucesso($"Corretor '{c.Nome}' cadastrado com ID {c.Id}.");
        }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    // ── Imóveis ───────────────────────────────────────────────────────────────

    private static void MenuImoveis()
    {
        while (true)
        {
            Console.Clear();
            Titulo("IMÓVEIS");
            Console.WriteLine("  [1] Listar todos os imóveis");
            Console.WriteLine("  [2] Buscar imóveis (filtros)");
            Console.WriteLine("  [3] Cadastrar casa");
            Console.WriteLine("  [4] Cadastrar apartamento");
            Console.WriteLine("  [5] Ver custos mensais de um imóvel");
            Console.WriteLine("  [6] Remover imóvel");
            Console.WriteLine("  [0] Voltar");
            Console.Write("\n  Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ListarImoveis(); break;
                case "2": BuscarImoveis(); break;
                case "3": CadastrarCasa(); break;
                case "4": CadastrarApartamento(); break;
                case "5": VerCustos(); break;
                case "6": RemoverImovel(); break;
                case "0": return;
                default: Erro("Opção inválida."); break;
            }
        }
    }

    private static void ListarImoveis(List<Imovel>? lista = null)
    {
        Console.Clear();
        Titulo("IMÓVEIS");
        var imoveis = lista ?? _svc.Imoveis;
        if (!imoveis.Any()) { Info("Nenhum imóvel encontrado."); Pausar(); return; }
        foreach (var i in imoveis) Console.WriteLine($"  {i}");
        Pausar();
    }

    private static void BuscarImoveis()
    {
        Console.Clear();
        Titulo("BUSCAR IMÓVEIS");

        Console.Write("  Tipo (Casa/Apartamento ou Enter para todos): ");
        var tipo = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(tipo)) tipo = null;

        TipoNegocio? tipoNeg = null;
        Console.Write("  Negócio (1=Venda, 2=Aluguel, Enter=todos): ");
        var negStr = Console.ReadLine()?.Trim();
        if (negStr == "1") tipoNeg = TipoNegocio.Venda;
        else if (negStr == "2") tipoNeg = TipoNegocio.Aluguel;

        Console.Write("  Valor mínimo (Enter para ignorar): ");
        decimal? vMin = decimal.TryParse(Console.ReadLine(), out var vm) ? vm : null;

        Console.Write("  Valor máximo (Enter para ignorar): ");
        decimal? vMax = decimal.TryParse(Console.ReadLine(), out var vx) ? vx : null;

        var resultado = _svc.BuscarImoveis(tipo, tipoNeg, vMin, vMax, StatusImovel.Disponivel);
        Console.Clear();
        Titulo($"RESULTADO — {resultado.Count} imóvel(is) encontrado(s)");
        foreach (var i in resultado) Console.WriteLine($"  {i}");
        Pausar();
    }

    private static void CadastrarCasa()
    {
        Console.Clear();
        Titulo("CADASTRAR CASA");
        if (!_svc.Corretores.Any()) { Erro("Cadastre um corretor primeiro."); Pausar(); return; }
        try
        {
            var end = LerEndereco();
            var valor = LerDecimal("Valor (R$)");
            var area = LerDouble("Área (m²)");
            var quartos = LerInt("Quartos");
            var banheiros = LerInt("Banheiros");
            var garagem = LerBool("Tem garagem? (s/n)");
            var negocio = LerOpcao("Tipo de negócio", new[] { "Venda", "Aluguel" }) == "Venda"
                ? TipoNegocio.Venda : TipoNegocio.Aluguel;
            var iptu = LerDouble("IPTU mensal (R$)");

            ListarCorretores();
            var corretorId = LerInt("ID do corretor responsável");
            var quintal = LerBool("Tem quintal? (s/n)");
            double areaQuintal = 0;
            if (quintal) areaQuintal = LerDouble("Área do quintal (m²)");
            var areaExt = LerBool("Tem área externa? (s/n)");

            var casa = _svc.CadastrarCasa(end, valor, area, quartos, banheiros, garagem,
                negocio, iptu, corretorId, quintal, areaQuintal, areaExt);
            Sucesso($"Casa cadastrada com ID {casa.Id}.");
        }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void CadastrarApartamento()
    {
        Console.Clear();
        Titulo("CADASTRAR APARTAMENTO");
        if (!_svc.Corretores.Any()) { Erro("Cadastre um corretor primeiro."); Pausar(); return; }
        try
        {
            var end = LerEndereco();
            var valor = LerDecimal("Valor (R$)");
            var area = LerDouble("Área (m²)");
            var quartos = LerInt("Quartos");
            var banheiros = LerInt("Banheiros");
            var garagem = LerBool("Tem garagem? (s/n)");
            var negocio = LerOpcao("Tipo de negócio", new[] { "Venda", "Aluguel" }) == "Venda"
                ? TipoNegocio.Venda : TipoNegocio.Aluguel;
            var iptu = LerDouble("IPTU mensal (R$)");

            ListarCorretores();
            var corretorId = LerInt("ID do corretor responsável");
            var andar = LerInt("Andar");
            var cond = LerDecimal("Valor do condomínio (R$)");
            var elevador = LerBool("Tem elevador? (s/n)");
            var lazer = LerBool("Tem área de lazer? (s/n)");

            var apto = _svc.CadastrarApartamento(end, valor, area, quartos, banheiros, garagem,
                negocio, iptu, corretorId, andar, cond, elevador, lazer);
            Sucesso($"Apartamento cadastrado com ID {apto.Id}.");
        }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void VerCustos()
    {
        Console.Clear();
        Titulo("CUSTOS MENSAIS DO IMÓVEL");
        ListarImoveis(_svc.Imoveis);
        int id = LerInt("ID do imóvel");
        var imovel = _svc.BuscarImovel(id);
        if (imovel == null) { Erro("Imóvel não encontrado."); Pausar(); return; }

        // Polimorfismo em ação: chama CalcularCustosMensais() sem saber o tipo concreto
        Console.WriteLine($"\n  Imóvel: {imovel.TipoImovel()} — {imovel.Endereco}");
        Console.WriteLine($"  Custos mensais: R${imovel.CalcularCustosMensais():N2}");
        if (imovel is Apartamento apto)
            Console.WriteLine($"  (IPTU: R${apto.IPTU:N2} + Condomínio: R${apto.ValorCondominio:N2})");
        Pausar();
    }

    private static void RemoverImovel()
    {
        Console.Clear();
        Titulo("REMOVER IMÓVEL");
        ListarImoveis();
        int id = LerInt("ID do imóvel a remover");
        try { _svc.RemoverImovel(id); Sucesso("Imóvel removido."); }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    // ── Propostas ─────────────────────────────────────────────────────────────

    private static void MenuPropostas()
    {
        while (true)
        {
            Console.Clear();
            Titulo("PROPOSTAS");
            Console.WriteLine("  [1] Listar propostas");
            Console.WriteLine("  [2] Enviar proposta");
            Console.WriteLine("  [3] Aceitar proposta");
            Console.WriteLine("  [4] Recusar proposta");
            Console.WriteLine("  [0] Voltar");
            Console.Write("\n  Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ListarPropostas(); break;
                case "2": EnviarProposta(); break;
                case "3": AceitarProposta(); break;
                case "4": RecusarProposta(); break;
                case "0": return;
                default: Erro("Opção inválida."); break;
            }
        }
    }

    private static void ListarPropostas()
    {
        Console.Clear();
        Titulo("PROPOSTAS");
        if (!_svc.Propostas.Any()) { Info("Nenhuma proposta registrada."); Pausar(); return; }
        foreach (var p in _svc.Propostas) Console.WriteLine($"  {p}");
        Pausar();
    }

    private static void EnviarProposta()
    {
        Console.Clear();
        Titulo("ENVIAR PROPOSTA");
        try
        {
            ListarClientes();
            var clienteId = LerInt("ID do cliente");
            ListarImoveis(_svc.BuscarImoveis(status: StatusImovel.Disponivel));
            var imovelId = LerInt("ID do imóvel");
            var valor = LerDecimal("Valor ofertado (R$)");
            Console.Write("  Observações (Enter para pular): ");
            var obs = Console.ReadLine() ?? "";

            var p = _svc.EnviarProposta(clienteId, imovelId, valor, obs);
            Sucesso($"Proposta #{p.Id} enviada com sucesso!");
        }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void AceitarProposta()
    {
        Console.Clear();
        Titulo("ACEITAR PROPOSTA");
        ListarPropostas();
        int id = LerInt("ID da proposta a aceitar");
        try { _svc.AceitarProposta(id); Sucesso("Proposta aceita! Imóvel atualizado automaticamente."); }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void RecusarProposta()
    {
        Console.Clear();
        Titulo("RECUSAR PROPOSTA");
        ListarPropostas();
        int id = LerInt("ID da proposta a recusar");
        try { _svc.RecusarProposta(id); Sucesso("Proposta recusada."); }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    // ── Visitas ───────────────────────────────────────────────────────────────

    private static void MenuVisitas()
    {
        while (true)
        {
            Console.Clear();
            Titulo("VISITAS");
            Console.WriteLine("  [1] Listar visitas");
            Console.WriteLine("  [2] Agendar visita");
            Console.WriteLine("  [3] Marcar como realizada");
            Console.WriteLine("  [4] Cancelar visita");
            Console.WriteLine("  [0] Voltar");
            Console.Write("\n  Opção: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": ListarVisitas(); break;
                case "2": AgendarVisita(); break;
                case "3": RealizarVisita(); break;
                case "4": CancelarVisita(); break;
                case "0": return;
                default: Erro("Opção inválida."); break;
            }
        }
    }

    private static void ListarVisitas()
    {
        Console.Clear();
        Titulo("VISITAS");
        if (!_svc.Visitas.Any()) { Info("Nenhuma visita registrada."); Pausar(); return; }
        foreach (var v in _svc.Visitas) Console.WriteLine($"  {v}");
        Pausar();
    }

    private static void AgendarVisita()
    {
        Console.Clear();
        Titulo("AGENDAR VISITA");
        try
        {
            ListarClientes();
            var clienteId = LerInt("ID do cliente");
            ListarImoveis(_svc.BuscarImoveis(status: StatusImovel.Disponivel));
            var imovelId = LerInt("ID do imóvel");

            Console.Write("  Data e hora (dd/MM/yyyy HH:mm): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
                null, System.Globalization.DateTimeStyles.None, out var data))
                throw new FormatException("Formato de data inválido. Use dd/MM/yyyy HH:mm");

            int? corretorId = null;
            Console.Write("  Associar corretor? (s/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "s")
            {
                ListarCorretores();
                corretorId = LerInt("ID do corretor");
            }

            Console.Write("  Observações (Enter para pular): ");
            var obs = Console.ReadLine() ?? "";

            var v = _svc.AgendarVisita(clienteId, imovelId, data, corretorId, obs);
            Sucesso($"Visita #{v.Id} agendada para {v.DataHora:dd/MM/yyyy HH:mm}.");
        }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void RealizarVisita()
    {
        Console.Clear();
        Titulo("MARCAR VISITA COMO REALIZADA");
        ListarVisitas();
        int id = LerInt("ID da visita");
        try { _svc.RealizarVisita(id); Sucesso("Visita marcada como realizada."); }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    private static void CancelarVisita()
    {
        Console.Clear();
        Titulo("CANCELAR VISITA");
        ListarVisitas();
        int id = LerInt("ID da visita");
        try { _svc.CancelarVisita(id); Sucesso("Visita cancelada."); }
        catch (Exception ex) { Erro(ex.Message); }
        Pausar();
    }

    // ── Helpers de UI ─────────────────────────────────────────────────────────

    private static void Titulo(string texto)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n  ══ {texto} ══\n");
        Console.ResetColor();
    }

    private static void Sucesso(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n  ✔ {msg}");
        Console.ResetColor();
    }

    private static void Erro(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  ✘ Erro: {msg}");
        Console.ResetColor();
    }

    private static void Info(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  {msg}");
        Console.ResetColor();
    }

    private static void Pausar()
    {
        Console.Write("\n  Pressione Enter para continuar...");
        Console.ReadLine();
    }

    private static string LerObrigatorio(string campo)
    {
        string? valor;
        do
        {
            Console.Write($"  {campo}: ");
            valor = Console.ReadLine()?.Trim();
        } while (string.IsNullOrEmpty(valor));
        return valor;
    }

    private static string LerCPF()
    {
        string cpf;
        do
        {
            cpf = LerObrigatorio("CPF (somente números)");
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
        } while (cpf.Length != 11);
        return cpf;
    }

    private static int LerInt(string campo)
    {
        int val;
        while (true)
        {
            Console.Write($"  {campo}: ");
            if (int.TryParse(Console.ReadLine(), out val)) return val;
            Erro("Digite um número inteiro válido.");
        }
    }

    private static decimal LerDecimal(string campo)
    {
        decimal val;
        while (true)
        {
            Console.Write($"  {campo}: ");
            if (decimal.TryParse(Console.ReadLine()?.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out val)) return val;
            Erro("Digite um valor numérico válido.");
        }
    }

    private static double LerDouble(string campo)
    {
        double val;
        while (true)
        {
            Console.Write($"  {campo}: ");
            if (double.TryParse(Console.ReadLine()?.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out val)) return val;
            Erro("Digite um valor numérico válido.");
        }
    }

    private static bool LerBool(string campo)
    {
        while (true)
        {
            Console.Write($"  {campo}: ");
            var r = Console.ReadLine()?.Trim().ToLower();
            if (r == "s") return true;
            if (r == "n") return false;
            Erro("Digite 's' ou 'n'.");
        }
    }

    private static string LerOpcao(string campo, string[] opcoes)
    {
        while (true)
        {
            Console.Write($"  {campo} ({string.Join("/", opcoes)}): ");
            var r = Console.ReadLine()?.Trim();
            if (opcoes.Any(o => string.Equals(o, r, StringComparison.OrdinalIgnoreCase)))
                return opcoes.First(o => string.Equals(o, r, StringComparison.OrdinalIgnoreCase));
            Erro($"Digite uma das opções: {string.Join(", ", opcoes)}");
        }
    }

    private static Endereco LerEndereco()
    {
        Console.WriteLine("  --- Endereço ---");
        return new Endereco(
            LerObrigatorio("  Rua"),
            LerObrigatorio("  Número"),
            LerObrigatorio("  Bairro"),
            LerObrigatorio("  CEP"),
            LerObrigatorio("  Cidade")
        );
    }
}
