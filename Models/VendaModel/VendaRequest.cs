using ControleDeEstoque.API.Models.VendaItemModel;

namespace ControleDeEstoque.API.Models.VendaModel;

public record VendaRequest(
    Guid Id,
    Guid MetodoPagamentoId,
    List<VendaItemRequest> Itens
);