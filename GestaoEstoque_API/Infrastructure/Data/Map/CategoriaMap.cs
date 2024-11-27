using GestaoEstoque_API.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoriaMap : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(e => e.CategoriaId);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);
    }
}
