using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Models.ProdutoModel
{
    public class ProdutoModel
    {
        public ProdutoModel(string nome, decimal valorUnitario, int quantidade, Guid tamanhoId, Guid categoriaId, string sku)
        {
            Nome = nome;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
            TamanhoId = tamanhoId;
            CategoriaId = categoriaId;
            Sku = sku;
        }

        public Guid Id { get; init; }

        [MaxLength(100)]
        public string Sku { get; private set; }

        [MaxLength(100)]
        public string Nome { get; private set; }

        [Precision(18, 2)]
        public decimal ValorUnitario { get; private set; }

        public int Quantidade { get; private set; }

        public Guid TamanhoId { get; private set; }

        [ForeignKey("TamanhoId")]
        public TamanhoModel.TamanhoModel Tamanho { get; set; } // Alterado para a entidade TamanhoModel
        public string TamanhoNome { get; set; }

        public Guid CategoriaId { get; private set; }

        [ForeignKey("CategoriaId")]
        public CategoriaModel.CategoriaModel Categoria { get; set; } // Alterado para a entidade CategoriaModel

        public string CategoriaNome { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataExclusao { get; set; }

        public void AtualizarProduto(string nome, decimal valorunitario, int quantidade, Guid tamanhoId, Guid categoriaId, string sku)
        {
            Nome = nome;
            ValorUnitario = valorunitario;
            Quantidade = quantidade;
            TamanhoId = tamanhoId;
            CategoriaId = categoriaId;
            Sku = sku;
        }

        public void ExcluirProduto()
        {
            DataExclusao = DateTime.Now;
        }
    }
}