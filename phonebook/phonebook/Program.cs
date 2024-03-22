using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class Programa
{
    static string caminhoArquivo = "./contatos.json";
    static List<Contato> contatos = new List<Contato>();

    static void Main(string[] args)
    {
        CarregarContatos();
        bool sair = false;
        while (!sair)
        {
            Console.WriteLine("\nAgenda Telefônica");
            Console.WriteLine("1. Adicionar Contato");
            Console.WriteLine("2. Remover Contato");
            Console.WriteLine("3. Atualizar Contato");
            Console.WriteLine("4. Listar Contatos");
            Console.WriteLine("5. Buscar Contato");
            Console.WriteLine("6. Sair");
            Console.Write("Escolha uma opção: ");
            switch (Console.ReadLine())
            {
                case "1":
                    AdicionarContato();
                    break;
                case "2":
                    RemoverContato();
                    break;
                case "3":
                    AtualizarContato();
                    break;
                case "4":
                    ListarContatos();
                    break;
                case "5":
                    BuscarContato();
                    break;
                case "6":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
            SalvarContatos();
        }
    }

    static void AdicionarContato()
    {
        var contato = new Contato();
        Console.Write("Nome: ");
        contato.Nome = Console.ReadLine().Trim();
        while (string.IsNullOrEmpty(contato.Nome))
        {
            Console.WriteLine("O nome não pode ser vazio. Por favor, insira um nome válido.");
            Console.Write("Nome: ");
            contato.Nome = Console.ReadLine().Trim();
        }

        Console.Write("Telefone: ");
        contato.Telefone = Console.ReadLine().Trim();
        while (string.IsNullOrEmpty(contato.Telefone))
        {
            Console.WriteLine("O telefone não pode ser vazio. Por favor, insira um telefone válido.");
            Console.Write("Telefone: ");
            contato.Telefone = Console.ReadLine().Trim();
        }

        Console.Write("E-mail (opcional): ");
        contato.Email = Console.ReadLine().Trim();
        while (!string.IsNullOrEmpty(contato.Email) && !ValidarEmail(contato.Email))
        {
            Console.WriteLine("E-mail inválido. Por favor, insira um e-mail válido ou deixe em branco.");
            Console.Write("E-mail (opcional): ");
            contato.Email = Console.ReadLine().Trim();
        }

        Console.Write("Endereço (opcional): ");
        contato.Endereco = Console.ReadLine().Trim();

        contatos.Add(contato);
        Console.WriteLine("Contato adicionado com sucesso.");
    }


    static bool EmailValido(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    static void RemoverContato()
    {
        Console.Write("Digite o início do nome do contato a ser removido: ");
        string termoBusca = Console.ReadLine().ToLower();
        var contatosEncontrados = contatos.Where(c => c.Nome.ToLower().StartsWith(termoBusca))
                                          .OrderBy(c => c.Nome)
                                          .ToList();

        if (contatosEncontrados.Any())
        {
            Console.WriteLine("Contatos encontrados:");
            int index = 1;
            foreach (var contato in contatosEncontrados)
            {
                Console.WriteLine($"{index}. {contato.Nome}");
                index++;
            }

            Console.Write("\nEscolha o número do contato a ser removido: ");
            if (int.TryParse(Console.ReadLine(), out int escolha) && escolha >= 1 && escolha <= contatosEncontrados.Count)
            {
                // Ajusta o índice para base 0 para corresponder à lista
                var contatoRemover = contatosEncontrados[escolha - 1];
                // Remove o contato da lista original baseado em uma propriedade única, como o Nome (assumindo que não há nomes duplicados)
                contatos.RemoveAll(c => c.Nome.Equals(contatoRemover.Nome, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine("Contato removido com sucesso.");
            }
            else
            {
                Console.WriteLine("Seleção inválida.");
            }
        }
        else
        {
            Console.WriteLine("Nenhum contato correspondente à busca inicial foi encontrado.");
        }
    }

    static void AtualizarContato()
    {
        Console.Write("Digite o nome do contato a ser atualizado: ");
        string nome = Console.ReadLine().Trim();
        var contato = contatos.Find(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        if (contato != null)
        {
            Console.Write($"Novo Nome (anterior: {contato.Nome}): ");
            string novoNome = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(novoNome))
            {
                contato.Nome = novoNome;
            }

            Console.Write($"Novo telefone (anterior: {contato.Telefone}): ");
            string novoTelefone = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(novoTelefone))
            {
                contato.Telefone = novoTelefone;
            }

            Console.Write($"Novo e-mail (opcional, anterior: {contato.Email}): ");
            string novoEmail = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(novoEmail) && ValidarEmail(novoEmail))
            {
                contato.Email = novoEmail;
            }

            Console.Write($"Novo endereço (opcional, anterior: {contato.Endereco}): ");
            string novoEndereco = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(novoEndereco))
            {
                contato.Endereco = novoEndereco;
            }

            Console.WriteLine("Contato atualizado com sucesso.");
        }
        else
        {
            Console.WriteLine("Contato não encontrado.");
        }
    }
     static void ListarContatos()
        {
            var contatosOrdenados = contatos.OrderBy(c => c.Nome).ToList();

            foreach (var contato in contatosOrdenados)
            {
                Console.WriteLine($"\n--------------------------\nNome: {contato.Nome}\nTelefone: {contato.Telefone}\nE-mail: {contato.Email}\nEndereço: {contato.Endereco}\n--------------------------\n");
            }
        }

    static void BuscarContato()
    {
        Console.Write("Digite o nome do contato a ser buscado: ");
        string nomeBuscado = Console.ReadLine().ToLower(); 
        var contatosEncontrados = contatos.Where(c => c.Nome.ToLower().StartsWith(nomeBuscado))
                                          .OrderBy(c => c.Nome)
                                          .ToList();

        if (contatosEncontrados.Any())
        {
            foreach (var contato in contatosEncontrados)
            {
                Console.WriteLine($"--------------------------\nNome: {contato.Nome}\nTelefone: {contato.Telefone}\nE-mail: {contato.Email}\nEndereço: {contato.Endereco}\n--------------------------");
            }
        }
        else
        {
            Console.WriteLine("Contato não encontrado.");
        }
    }

    static void CarregarContatos()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            contatos = JsonConvert.DeserializeObject<List<Contato>>(json) ?? new List<Contato>();
        }
    }

    static void SalvarContatos()
    {
        string json = JsonConvert.SerializeObject(contatos, Formatting.Indented);
        File.WriteAllText(caminhoArquivo, json);
    }
    static bool ValidarEmail(string email)
    {
        var regex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return regex.IsMatch(email);
    }
}

class Contato
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Endereco { get; set; }
}
