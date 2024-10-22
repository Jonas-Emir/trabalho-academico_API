using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProdutoMap : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(e => e.ProdutoId);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Preco)
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.QuantidadeEstoque)
            .IsRequired();

        builder.Property(e => e.Ativo)
            .IsRequired();

        builder.Property(e => e.DataCriacao)
            .IsRequired();

        builder.Property(e => e.DataAtualizacao)
            .IsRequired(false);

        builder.HasOne(d => d.Categoria)
            .WithMany(p => p.Produtos)
            .HasForeignKey(d => d.CategoriaId);

        builder.HasOne(d => d.Fornecedor)
            .WithMany(p => p.Produtos)
            .HasForeignKey(d => d.FornecedorId);
    }
}
