using GestaoEstoque_API.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProdutoMap : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(p => p.ProdutoId);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Preco)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(p => p.Ativo)
            .IsRequired();

        builder.HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Fornecedor)
            .WithMany()
            .HasForeignKey(p => p.FornecedorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Estoque)
            .WithOne(e => e.Produto)
            .HasForeignKey(e => e.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
