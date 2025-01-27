namespace ControleDeEstoque.API.Models.ProdutoModel;

public record ProdutoRequest(

    string Nome,
    string Sku,
    decimal ValorUnitario,
    int Quantidade,
    Guid TamanhoId,
    Guid CategoriaId
    );