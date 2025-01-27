using System.ComponentModel.DataAnnotations;

namespace ControleDeEstoque.API.Models.TamanhoModel;

public class TamanhoModel
{
    public TamanhoModel(string nome, string? descricao)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        DataCadastro = DateTime.Now;
        DataExclusao = null;
    }
    
    public Guid Id { get; init; }
    [MaxLength(100)]
    public string Nome { get; private set; }
    [MaxLength(100)]
    public string? Descricao { get; set; }
    public DateTime DataCadastro { get; set; }
    
    public DateTime? DataExclusao { get; set; }
    
    public void AtualizarNome(string nome)
    {
        Nome = nome;
    }
    
    public void ExcluirTamanho()
    {
        DataExclusao = DateTime.Now;
    }
}