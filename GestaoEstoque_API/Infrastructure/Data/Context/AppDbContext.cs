using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public AppDbContext() { }
    public virtual DbSet<Produto> Produtos { get; set; }
    public virtual DbSet<Fornecedor> Fornecedores { get; set; }
    public virtual DbSet<Categoria> Categorias { get; set; }
    public virtual DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProdutoMap());
        modelBuilder.ApplyConfiguration(new FornecedorMap());
        modelBuilder.ApplyConfiguration(new CategoriaMap());
        modelBuilder.ApplyConfiguration(new MovimentacaoEstoqueMap());

        base.OnModelCreating(modelBuilder);
    }
}
