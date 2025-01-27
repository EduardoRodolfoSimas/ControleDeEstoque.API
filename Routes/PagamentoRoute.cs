using ControleDeEstoque.API.Data;
using ControleDeEstoque.API.Models.PagamentoModel;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Routes;

public static class PagamentoRoute
{
        public static void PagamentoRoutes(this WebApplication app)
    {
        var route = app.MapGroup("pagamento");
        
        //Adicionar
        route.MapPost("", async (PagamentoRequest pagamentoareq, DataBaseContext context) =>
        {
            var pagamentoModel = new PagamentoModel(pagamentoareq.Tipo, pagamentoareq.Descricao);
            await context.Pagamentos.AddAsync(pagamentoModel);
            await context.SaveChangesAsync();
            
            return Results.Created($"/pagamento/{pagamentoModel.Id}", pagamentoModel);
        });
         
        //Listar todos
        route.MapGet("", async (DataBaseContext context) =>
        {
            var pagamento = await context.Pagamentos
                .Where(p => p.DataExclusao == null)
                .ToListAsync();
            return Results.Ok(pagamento);
        });
        
        //Listar por id
        route.MapGet("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var pagamento = await context.Pagamentos
                .Where(p => p.DataExclusao == null)
                .FirstOrDefaultAsync(p => p.Id == id);
            return pagamento is not null ? Results.Ok(pagamento) : Results.NotFound();
        });
        
        //Atualizar
        route.MapPut("{id:guid}", async (Guid id, PagamentoRequest pagamentoareq , DataBaseContext context) =>
        {
            var pagamento = await context.Pagamentos.FirstOrDefaultAsync(c => c.Id == id);
            
            if (pagamento is null) 
                return Results.NotFound();
            
            pagamento.AtualizarPagamento(pagamentoareq.Tipo, pagamentoareq.Descricao);
            await context.SaveChangesAsync();
            return Results.Ok(pagamento);
        });
         
        //Deletar
        route.MapDelete("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var pagamento = await context.Pagamentos.FirstOrDefaultAsync(c => c.Id == id);
            
            if (pagamento is null) 
                return Results.NotFound();
            
            pagamento.ExcluirPagamento();
            await context.SaveChangesAsync();
            return Results.Ok(pagamento);
        });
    }
}