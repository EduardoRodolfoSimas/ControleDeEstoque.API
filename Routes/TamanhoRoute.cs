using ControleDeEstoque.API.Data;
using ControleDeEstoque.API.Models.TamanhoModel;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Routes;

public static class TamanhoRoute
{
    public static void TamanhoRoutes(this WebApplication app)
    {
        var route = app.MapGroup("tamanho");
        
        //Adicionar
        route.MapPost("", async (TamanhoRequest tamanhoreq, DataBaseContext context) =>
        {
            var tamanhoModel = new TamanhoModel(tamanhoreq.Nome, tamanhoreq.Descricao);
            await context.Tamanhos.AddAsync(tamanhoModel);
            await context.SaveChangesAsync();
            
            return Results.Created($"/tamanho/{tamanhoModel.Id}", tamanhoModel);
        });
         
        //Listar todos
        route.MapGet("", async (DataBaseContext context) =>
        {
            var tamanhos = await context.Tamanhos
                .Where(t => t.DataExclusao == null)
                .ToListAsync();
            return Results.Ok(tamanhos);
        });
        
        //Listar por id
        route.MapGet("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var tamanho = await context.Tamanhos
                .Where(t => t.DataExclusao == null)
                .FirstOrDefaultAsync(p => p.Id == id);
            return tamanho is not null ? Results.Ok(tamanho) : Results.NotFound();
        });
        
        //Atualizar
        route.MapPut("{id:guid}", async (Guid id, TamanhoRequest tamanhoreq, DataBaseContext context) =>
        {
            var tamanho = await context.Tamanhos.FirstOrDefaultAsync(t => t.Id == id);
            
            if (tamanho is null) 
                return Results.NotFound();
            
            tamanho.AtualizarNome(tamanhoreq.Nome);
            tamanho.Descricao = tamanhoreq.Descricao;
            await context.SaveChangesAsync();
            return Results.Ok(tamanho);
        });
         
        //Deletar
        route.MapDelete("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var tamanho = await context.Tamanhos.FirstOrDefaultAsync(t => t.Id == id);
            
            if (tamanho is null) 
                return Results.NotFound();
            
            tamanho.ExcluirTamanho();
            await context.SaveChangesAsync();
            return Results.Ok(tamanho);
        });
    }
}