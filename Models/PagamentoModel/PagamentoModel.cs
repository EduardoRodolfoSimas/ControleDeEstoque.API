using System.ComponentModel.DataAnnotations;

namespace ControleDeEstoque.API.Models.PagamentoModel
{
    public class PagamentoModel
    {
        public PagamentoModel(string tipo, string descricao)
        {
            Id = Guid.NewGuid();
            Tipo = tipo;
            Descricao = descricao;
            DataCadastro = DateTime.Now;
        }

        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Tipo { get; set; }

        [MaxLength(100)]
        public string? Descricao { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataExclusao { get; set; }

        public void AtualizarPagamento(string tipo, string descricao)
        {
            Tipo = tipo;
            Descricao = descricao;
        }

        public void ExcluirPagamento()
        {
            DataExclusao = DateTime.Now;
        }
    }
}