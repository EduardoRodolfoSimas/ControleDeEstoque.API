using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Models.VendaModel;
    public class VendaModel
    {
            public Guid Id { get; private set; }
            public Guid MetodoPagamentoId { get; private set; }
            public PagamentoModel.PagamentoModel MetodoPagamento { get; set; }

            public decimal ValorTotal { get; private set; }
            public DateTime DataVenda { get; private set; }
            [JsonIgnore]
            public List<VendaItemModel> Itens { get; private set; }

            public VendaModel()
            {
                Itens = new List<VendaItemModel>();
            }
            
            public VendaModel(Guid id, Guid metodoPagamentoId, List<VendaItemModel> items)
            {
                Id = id;
                MetodoPagamentoId = metodoPagamentoId;
                DataVenda = DateTime.Now;
                Itens = items;
            }

            public void AdicionarItem(VendaItemModel item)
            {
                Itens.Add(item);
                AtualizarValorTotal();
            }

            public void AtualizarVenda(Guid metodoPagamentoId, List<VendaItemModel> itens)
            {
                MetodoPagamentoId = metodoPagamentoId;
                Itens = itens;
                AtualizarValorTotal();
            }

            private void AtualizarValorTotal()
            {
                ValorTotal = Itens.Sum(i => i.Subtotal);
            }
    }