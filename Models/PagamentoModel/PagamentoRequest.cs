namespace ControleDeEstoque.API.Models.PagamentoModel;

public record PagamentoRequest(
    string Tipo,
    string Descricao
);