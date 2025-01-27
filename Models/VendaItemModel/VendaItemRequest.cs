namespace ControleDeEstoque.API.Models.VendaItemModel;

public record VendaItemRequest(
    Guid VendaId,
    Guid ProdutoId,
    int Quantidade,
    decimal ValorUnitario
);