using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.HasKey(e => e.FornecedorId);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Endereco)
            .HasMaxLength(200);
    }
}
