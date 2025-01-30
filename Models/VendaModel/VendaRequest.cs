using ControleDeEstoque.API.Models.VendaItemModel;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace ControleDeEstoque.API.Models.VendaModel;

public record VendaRequest(
    Guid Id,
    string MetodoPagamentoTipo,
    Guid MetodoPagamentoId,
    List<VendaItemRequest> Itens
);