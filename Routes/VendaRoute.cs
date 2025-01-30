    using ControleDeEstoque.API.Data;
    using ControleDeEstoque.API.Models.VendaItemModel;
    using ControleDeEstoque.API.Models.VendaModel;
    using Microsoft.EntityFrameworkCore;

    namespace ControleDeEstoque.API.Routes;

    public static class VendaRoute
    {
        public static void VendaRoutes(this WebApplication app)
        {
            var route = app.MapGroup("vendas");

            route.MapPost("", async (VendaRequest vendaReq, DataBaseContext context) =>
            {
                // Create the new sale
                var venda = new VendaModel(vendaReq.Id, vendaReq.MetodoPagamentoId, 
                    vendaReq.Itens.Select(item => new VendaItemModel(vendaReq.Id, item.ProdutoId, item.Quantidade, item.ValorUnitario)).ToList());

                // Save the sale and items to the database
                await context.Vendas.AddAsync(venda);
                await context.SaveChangesAsync();

                // Return the result
                return Results.Created($"/vendas/{venda.Id}", venda);
            });
            // Listar todas as vendas
            route.MapGet("", async (DataBaseContext context) =>
            {
                var vendas = await context.Vendas
                    .Include(v => v.Itens)
                    .ThenInclude(vi => vi.Produto)
                    .Select(v => new
                    {
                        v.Id,
                        v.DataVenda,
                        v.ValorTotal,
                        v.MetodoPagamento,
                        v.MetodoPagamentoTipo,
                        Itens = v.Itens.Select(i => new
                        {
                            i.Id,
                            i.ProdutoId,
                            i.Quantidade,
                            i.ValorUnitario,
                            i.Subtotal,
                            Produto = new
                            {
                                i.Produto.Id,
                                i.Produto.Nome
                            }
                        })
                    })
                    .ToListAsync();
                return Results.Ok(vendas);
            });

            // Listar uma venda por ID
            route.MapGet("{id:guid}", async (Guid id, DataBaseContext context) =>
            {
                var venda = await context.Vendas
                    .Include(v => v.Itens)
                    .ThenInclude(vi => vi.Produto)
                    .FirstOrDefaultAsync(v => v.Id == id);

                return venda is not null ? Results.Ok(venda) : Results.NotFound();
            });

            route.MapPut("{id:guid}", async (Guid id, VendaRequest vendaReq, DataBaseContext context) =>
            {
                var venda = await context.Vendas
                    .Include(v => v.Itens)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venda is null)
                    return Results.NotFound();

                // Remove itens antigos
                context.VendaItems.RemoveRange(venda.Itens);

                // Adiciona os novos itens
                foreach (var itemReq in vendaReq.Itens)
                {
                    var vendaItem = new VendaItemModel(venda.Id, itemReq.ProdutoId, itemReq.Quantidade, itemReq.ValorUnitario);
                    venda.AdicionarItem(vendaItem);
                }

                await context.SaveChangesAsync();
                return Results.Ok(venda);
            });
            // Deletar uma venda
            route.MapDelete("{id:guid}", async (Guid id, DataBaseContext context) =>
            {
                var venda = await context.Vendas
                    .Include(v => v.Itens)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venda is null)
                    return Results.NotFound();

                context.VendaItems.RemoveRange(venda.Itens);
                context.Vendas.Remove(venda);

                await context.SaveChangesAsync();
                return Results.Ok(venda);
            });

            // Adicionar item à venda existente
            route.MapPost("{vendaId:guid}/itens", async (Guid vendaId, VendaItemRequest itemReq, DataBaseContext context) =>
            {
                var venda = await context.Vendas
                    .Include(v => v.Itens)
                    .FirstOrDefaultAsync(v => v.Id == vendaId);

                if (venda is null)
                    return Results.NotFound();

                var vendaItem = new VendaItemModel(vendaId, itemReq.ProdutoId, itemReq.Quantidade, itemReq.ValorUnitario);
                venda.AdicionarItem(vendaItem);
                await context.VendaItems.AddAsync(vendaItem);
                await context.SaveChangesAsync();

                return Results.Created($"/venda/{vendaId}/itens/{vendaItem.Id}", vendaItem);
            });
        }
    }