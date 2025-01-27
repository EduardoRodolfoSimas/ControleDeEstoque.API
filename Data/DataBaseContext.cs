using ControleDeEstoque.API.Models.CategoriaModel;
using ControleDeEstoque.API.Models.ProdutoModel;
using ControleDeEstoque.API.Models.TamanhoModel;
using ControleDeEstoque.API.Models.VendaModel;
using ControleDeEstoque.API.Models.PagamentoModel;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.API.Data;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }
    
    public DbSet<VendaModel> Vendas { get; set; }
    public DbSet<VendaItemModel> VendaItems { get; set; }
    public DbSet<ProdutoModel> Produtos { get; set; }
    public DbSet<CategoriaModel> Categorias { get; set; }
    public DbSet<TamanhoModel> Tamanhos { get; set; }
    public DbSet<PagamentoModel> Pagamentos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Mapeamento da entidade PagamentoModel
    modelBuilder.Entity<PagamentoModel>()
        .ToTable("Pagamentos")
        .HasKey(p => p.Id);

    modelBuilder.Entity<PagamentoModel>()
        .Property(p => p.Tipo)
        .IsRequired()
        .HasMaxLength(100);

    modelBuilder.Entity<PagamentoModel>()
        .Property(p => p.Descricao)
        .HasMaxLength(100);

    // Mapeamento da entidade ProdutoModel
    modelBuilder.Entity<ProdutoModel>()
        .ToTable("Produtos")
        .HasKey(p => p.Id);

    modelBuilder.Entity<ProdutoModel>()
        .Property(p => p.Nome)
        .IsRequired()
        .HasMaxLength(100);

    modelBuilder.Entity<ProdutoModel>()
        .Property(p => p.Sku)
        .IsRequired()
        .HasMaxLength(100);

    modelBuilder.Entity<ProdutoModel>()
        .Property(p => p.ValorUnitario)
        .HasPrecision(18, 2);

// Relacionamento Produto -> Categoria (HasOne -> WithMany)
    modelBuilder.Entity<ProdutoModel>()
        .HasOne(p => p.Categoria) // Relacionamento com a entidade Categoria
        .WithMany()  // Categoria não possui uma coleção de Produtos (1:N)
        .HasForeignKey(p => p.CategoriaId) // A chave estrangeira é CategoriaId
        .OnDelete(DeleteBehavior.Restrict); // Evitar deleção em cascata

// Relacionamento Produto -> Tamanho (HasOne -> WithMany)
    modelBuilder.Entity<ProdutoModel>()
        .HasOne(p => p.Tamanho) // Relacionamento com a entidade Tamanho
        .WithMany() // Tamanho não possui uma coleção de Produtos
        .HasForeignKey(p => p.TamanhoId) // A chave estrangeira é TamanhoId
        .OnDelete(DeleteBehavior.Restrict); // Evitar deleção em cascata

    // Mapeamento da entidade VendaModel
    modelBuilder.Entity<VendaModel>()
        .ToTable("Vendas")
        .HasKey(v => v.Id);

    modelBuilder.Entity<VendaModel>()
        .Property(v => v.DataVenda)
        .IsRequired();

    // Relacionamento Venda -> Pagamento (HasOne -> WithMany)
    modelBuilder.Entity<VendaModel>()
        .HasOne(v => v.MetodoPagamento)
        .WithMany() // Pagamento não tem uma coleção de Vendas
        .HasForeignKey(v => v.MetodoPagamentoId)
        .OnDelete(DeleteBehavior.Restrict);

    // Mapeamento da entidade VendaItemModel
    modelBuilder.Entity<VendaItemModel>()
        .ToTable("VendaItens")
        .HasKey(vi => vi.Id);

    modelBuilder.Entity<VendaItemModel>()
        .Property(vi => vi.Quantidade)
        .IsRequired();

    modelBuilder.Entity<VendaItemModel>()
        .Property(vi => vi.ValorUnitario)
        .HasPrecision(18, 2);

    modelBuilder.Entity<VendaItemModel>()
        .Property(vi => vi.Subtotal)
        .HasPrecision(18, 2);

    // Relacionamento VendaItem -> Produto (HasOne -> WithMany)
    modelBuilder.Entity<VendaItemModel>()
        .HasOne(vi => vi.Produto)
        .WithMany() // Produto não tem uma coleção de VendaItens
        .HasForeignKey(vi => vi.ProdutoId)
        .OnDelete(DeleteBehavior.Restrict);

    // Relacionamento VendaItem -> Venda (HasOne -> WithMany)
    modelBuilder.Entity<VendaItemModel>()
        .HasOne(vi => vi.Venda)
        .WithMany(v => v.Itens) // Venda tem uma coleção de VendaItens
        .HasForeignKey(vi => vi.VendaId)
        .OnDelete(DeleteBehavior.Cascade); // Deleção em cascata
}
}
    