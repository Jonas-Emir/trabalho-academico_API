using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FornecedorMap : IEntityTypeConfiguration<Fornecedor>
{
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        builder.HasKey(f => f.FornecedorId);

        builder.Property(f => f.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.CNPJ)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(f => f.Telefone)
            .IsRequired()
            .HasMaxLength(15); 

        builder.Property(f => f.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Endereco)
            .IsRequired()
            .HasMaxLength(200);
    }
}
