using ControleDeEstoque.API.Data;
using ControleDeEstoque.API.Models.VendaItemModel;
using ControleDeEstoque.API.Models.VendaModel;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Routes;

public static class VendaItemRoute
{
    public static void VendaItemRoutes(this WebApplication app)
    {
        var route = app.MapGroup("vendaitem");
        
        // Adicionar
        route.MapPost("{vendaId:guid}/item", async (Guid vendaId, VendaItemRequest vendaItemReq, DataBaseContext context) =>
        {
            var venda = await context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.Id == vendaId);

            if (venda is null)
                return Results.NotFound();

            var vendaItem = new VendaItemModel(vendaId, vendaItemReq.ProdutoId, vendaItemReq.Quantidade, vendaItemReq.ValorUnitario);
            venda.AdicionarItem(vendaItem);
            await context.VendaItems.AddAsync(vendaItem);

            await context.SaveChangesAsync();
            return Results.Created($"/venda/{vendaId}/item", vendaItem);
        });
        
        // Listar todos
        route.MapGet("", async (DataBaseContext context) =>
        {
            var vendaItems = await context.VendaItems.Include(v => v.Produto).ToListAsync();
            return Results.Ok(vendaItems);
        });
        
        // Listar por Vendaid
        route.MapGet("/{vendaId:guid}/itens", async (Guid vendaId, DataBaseContext context) =>
        {
            var vendaItems = await context.VendaItems
                .Include(v => v.Produto)
                .Where(v => v.VendaId == vendaId)
                .ToListAsync();

            return vendaItems.Any() ? Results.Ok(vendaItems) : Results.NotFound();
        });
        
        // Atualizar
        route.MapPut("{id:guid}", async (Guid id, VendaItemRequest vendaItemReq, DataBaseContext context) =>
        {
            var vendaItem = await context.VendaItems.FirstOrDefaultAsync(v => v.Id == id);
            
            if (vendaItem is null) 
                return Results.NotFound();
            
            vendaItem.AtualizarItem(vendaItemReq.Quantidade, vendaItemReq.ValorUnitario);

            await context.SaveChangesAsync();
            return Results.Ok(vendaItem);
        });
        
        // Deletar
        route.MapDelete("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var vendaItem = await context.VendaItems.FirstOrDefaultAsync(v => v.Id == id);
            
            if (vendaItem is null) 
                return Results.NotFound();
            
            context.VendaItems.Remove(vendaItem);            
            await context.SaveChangesAsync();
            return Results.Ok(vendaItem);
        });
    }
}