using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; }

    //Usar a abordagem de separação em arquivos para cada mapeamento via IEntityTypeConfiguration
    //torna o código mais modular e fácil de manter, seguindo o princípio de separação de responsabilidades.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProdutoMap());
        modelBuilder.ApplyConfiguration(new FornecedorMap());
        modelBuilder.ApplyConfiguration(new CategoriaMap());
        modelBuilder.ApplyConfiguration(new MovimentacaoEstoqueMap());

        base.OnModelCreating(modelBuilder);
    }

   //Outro ponto importante é sobre o uso de Entity vs o uso de SQL direto.

   //Quando Usar Entity Framework
   //Operações simples de CRUD, agilidade e facilidade de manutenção. Projetos onde a performance não é crítica para todas as operações.
    
   //Quando Usar SQL Direto
   //Consultas complexas que envolvem múltiplas tabelas, agregações e em cenários de alta performance, como relatórios ou processamento em massa.
}
