using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Models.VendaModel
{
    public class VendaItemModel
    {
        public VendaItemModel() { }

        public VendaItemModel(Guid vendaId, Guid produtoId, int quantidade, decimal valorUnitario)
        {
            Id = Guid.NewGuid();
            VendaId = vendaId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            Subtotal = quantidade * valorUnitario;
        }

        public Guid Id { get; init; }

        public Guid ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual ProdutoModel.ProdutoModel? Produto { get; set; }

        public int Quantidade { get; private set; }

        [Precision(18, 2)]
        public decimal ValorUnitario { get; private set; }

        [Precision(18, 2)]
        public decimal Subtotal { get; private set; }

        [JsonIgnore] // Ignora o VendaId na serialização do item
        public Guid VendaId { get; set; }

        [ForeignKey("VendaId")]
        public virtual VendaModel Venda { get; set; }

        public void AtualizarItem(int quantidade, decimal valorUnitario)
        {
            Quantidade = quantidade > 0 ? quantidade : throw new ArgumentException("Quantidade deve ser maior que zero.");
            ValorUnitario = valorUnitario > 0 ? valorUnitario : throw new ArgumentException("Valor unitário deve ser maior que zero.");
            Subtotal = Quantidade * ValorUnitario;
        }

        public void SetVendaId(Guid vendaId)
        {
            VendaId = vendaId;
        }
    }
}