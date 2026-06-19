# Sistema de Gestão Imobiliária

Sistema orientado a objetos desenvolvido em C# para gerenciamento de operações de uma imobiliária, implementando os quatro pilares da POO.

**Grupo 7:** Cauã Moreira Martins, Davi Mendoza Rezende Couto, Luísa Machado Antunes Santos, Matheus Henrique Moreira e Pedro Nunes Cruz

---

## Pré-requisitos

- .NET SDK 10.0 ou superior

Verifique a instalação: dotnet --version

---

## Como compilar e executar

# Restaurar dependências
dotnet restore

# Executar
dotnet run

---

## Estrutura do Projeto


sistema-imobiliaria/
├── Models/
│   ├── Pessoa.cs          # Classe abstrata base para usuários
│   ├── Cliente.cs         # Subclasse de Pessoa
│   ├── Corretor.cs        # Subclasse de Pessoa
│   ├── Endereco.cs        # Objeto de valor
│   ├── Imovel.cs          # Classe abstrata base para imóveis
│   ├── Casa.cs            # Subclasse de Imovel
│   ├── Apartamento.cs     # Subclasse de Imovel
│   ├── Proposta.cs        # Entidade de negociação
│   └── Visita.cs          # Entidade de agendamento
├── Services/
│   ├── ImobiliariaService.cs  # Lógica de negócio
│   └── JsonService.cs         # Persistência em JSON
├── UI/
│   └── Menu.cs            # Interface de console interativa
├── Data/                  # Arquivos JSON gerados automaticamente
└── Program.cs             # Ponto de entrada


---

## Funcionalidades

- **Clientes:** cadastro, listagem e remoção
- **Corretores:** cadastro e listagem
- **Imóveis:** cadastro de casas e apartamentos, busca com filtros (tipo, negócio, faixa de valor), cálculo de custos mensais e remoção
- **Propostas:** envio, aceite e recusa (com atualização automática do status do imóvel)
- **Visitas:** agendamento, realização e cancelamento

---

## Pilares da POO aplicados

### Abstração
`Pessoa` e `Imovel` são classes abstratas — não é possível instanciá-las diretamente. Elas definem o contrato que as subclasses devem seguir, como o método abstrato `CalcularCustosMensais()` em `Imovel` e `TipoUsuario()` em `Pessoa`.

### Encapsulamento
Os atributos `Status` em `Imovel` e `StatusProposta` em `Proposta` são privados e não possuem setters públicos. A mudança de estado só ocorre por métodos de negócio como `AceitarProposta()`, `MarcarComoAlugado()` e `Cancelar()`, garantindo integridade dos dados.

### Herança
- `Cliente` e `Corretor` herdam de `Pessoa` (nome, CPF, telefone, email)
- `Casa` e `Apartamento` herdam de `Imovel` (valor, endereço, status, quartos etc.)

### Polimorfismo
O método `CalcularCustosMensais()` é implementado diferentemente em cada subclasse:
- `Casa` → retorna apenas o IPTU
- `Apartamento` → retorna IPTU + valor do condomínio

O sistema chama o mesmo método em objetos de tipos distintos e cada um responde corretamente.

---

## Armazenamento de Dados

Os dados são persistidos em arquivos `.json` na pasta `Data/`, criada automaticamente na primeira execução:

- `Data/Cliente.json`
- `Data/Corretor.json`
- `Data/Imovel.json`
- `Data/Proposta.json`
- `Data/Visita.json`

---

## Regras de Negócio

- Um cliente só pode ter **uma proposta pendente por imóvel**
- Ao aceitar uma proposta, o imóvel muda automaticamente para **Alugado** ou **Vendido** conforme o tipo de negócio
- As demais propostas pendentes para o mesmo imóvel são **recusadas automaticamente**
- Imóveis não disponíveis não podem receber novas propostas ou agendamentos de visita
- Visitas só podem ser agendadas para datas **futuras**
- Imóveis só podem ser removidos se estiverem com status **Disponível**
