using ControleDeEstoque.API.Data;
using ControleDeEstoque.API.Models.ProdutoModel;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Routes;

public static class ProdutoRoute
{
    public static void ProdutoRoutes(this WebApplication app)
    {
var route = app.MapGroup("produto");
        
        // Adicionar
        route.MapPost("", async (ProdutoRequest produtoreq, DataBaseContext context) =>
        {
            var tamanho = await context.Tamanhos.FindAsync(produtoreq.TamanhoId);
            var categoria = await context.Categorias.FindAsync(produtoreq.CategoriaId);
            if (categoria == null || tamanho == null)
            {
                return Results.NotFound("Not found");
            }

            var produtoModel = new ProdutoModel(produtoreq.Nome, produtoreq.ValorUnitario, produtoreq.Quantidade,
                produtoreq.TamanhoId, produtoreq.CategoriaId, produtoreq.Sku)
            {
                TamanhoNome = tamanho.Nome,
                CategoriaNome = categoria.Nome
            };

            await context.Produtos.AddAsync(produtoModel);
            await context.SaveChangesAsync();
            
            return Results.Created($"/produto/{produtoModel.Id}", produtoModel);
        });
        
        // Listar todos
        route.MapGet("", async (DataBaseContext context) =>
        {
            var produtos = await context.Produtos
                .Where(p => p.DataExclusao == null)
                .ToListAsync();
            return Results.Ok(produtos);
        });
        
        // Listar por id
        route.MapGet("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var produto = await context.Produtos
                .Where(p => p.DataExclusao == null)
                .FirstOrDefaultAsync(p => p.Id == id);
            return produto is not null ? Results.Ok(produto) : Results.NotFound();
        });
        
        // Atualizar
        route.MapPut("{id:guid}", async (Guid id, ProdutoRequest produtoreq, DataBaseContext context) =>
        {
            var produto = await context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto is null) 
                return Results.NotFound();

            var categoria = await context.Categorias.FindAsync(produtoreq.CategoriaId);
            if (categoria == null)
            {
                return Results.NotFound("Categoria not found");
            }

            produto.AtualizarProduto(produtoreq.Nome, produtoreq.ValorUnitario, produtoreq.Quantidade,
                produtoreq.TamanhoId, produtoreq.CategoriaId, produtoreq.Sku);
            produto.CategoriaNome = categoria.Nome;

            await context.SaveChangesAsync();
            return Results.Ok(produto);
        });
        
        // Deletar
        route.MapDelete("{id:guid}", async (Guid id, DataBaseContext context) =>
        {
            var produto = await context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto is null) 
                return Results.NotFound();
            
            produto.ExcluirProduto();
            await context.SaveChangesAsync();
            return Results.Ok(produto);
        });    
    }
}