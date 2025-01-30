namespace ControleDeEstoque.API.Models.VendaItemModel;

public record VendaItemRequest(
    Guid VendaId,
    Guid ProdutoId,
    string ProdutoNome,
    int Quantidade,
    decimal ValorUnitario
);