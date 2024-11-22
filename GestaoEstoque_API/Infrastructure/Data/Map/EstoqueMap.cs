using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class EstoqueMap : IEntityTypeConfiguration<Estoque>
{
    public void Configure(EntityTypeBuilder<Estoque> builder)
    {
        builder.HasKey(e => e.EstoqueId);

        builder.HasOne(e => e.Produto)
            .WithMany(p => p.Estoque) 
            .HasForeignKey(e => e.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Quantidade)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.Id_Tipo_Movimento)
            .IsRequired()
            .HasConversion<int>();
    }
}
