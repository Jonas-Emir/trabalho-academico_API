using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto> Produto { get; set; }
    public DbSet<Fornecedor> Fornecedor { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Estoque> Estoque { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProdutoMap());
        modelBuilder.ApplyConfiguration(new FornecedorMap());
        modelBuilder.ApplyConfiguration(new CategoriaMap());
        modelBuilder.ApplyConfiguration(new EstoqueMap());

        base.OnModelCreating(modelBuilder);
    }
}
