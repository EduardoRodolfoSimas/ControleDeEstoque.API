using ControleDeEstoque.API.Data;
using ControleDeEstoque.API.Models.CategoriaModel;
using Microsoft.EntityFrameworkCore;


namespace ControleDeEstoque.API.Routes;

public static class CategoriaRoute
{
    public static void CategoriaRoutes(this WebApplication app)
    {
        var route = app.MapGroup("categoria");
        
        //Adicionar
        route.MapPost("", async (CategoriaRequest categoriareq, DataBaseContext context) =>
        {
            var categoriaModel = new CategoriaModel(categoriareq.Nome, categoriareq.Descricao);
            await context.Categorias.AddAsync(categoriaModel);
            await context.SaveChangesAsync();
            
            return Results.Created($"/categoria/{categoriaModel.Id}", categoriaModel);
        });
         
        //Listar todos
        route.MapGet("", async (DataBaseContext context) =>
        {
            var categoria = await context.Categorias
                .Where(c => c.DataExclusao == null)
                .ToListAsync();
            return Results.Ok(categoria);
        });
        
        //Listar por id
        route.MapGet("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var categoria = await context.Categorias
                .Where(c => c.DataExclusao == null)
                .FirstOrDefaultAsync(c => c.Id == id);
            return categoria is not null ? Results.Ok(categoria) : Results.NotFound();
        });
        
        //Atualizar
        route.MapPut("{id:guid}", async (Guid id, CategoriaRequest categoriareq , DataBaseContext context) =>
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            
            if (categoria is null) 
                return Results.NotFound();
            
            categoria.AtualizarNome(categoriareq.Nome);
            categoria.Descricao = categoriareq.Descricao;
            await context.SaveChangesAsync();
            return Results.Ok(categoria);
        });
         
        //Deletar
        route.MapDelete("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            
            if (categoria is null) 
                return Results.NotFound();
            
            categoria.ExcluirCategoria();
            await context.SaveChangesAsync();
            return Results.Ok(categoria);
        });
    }
}