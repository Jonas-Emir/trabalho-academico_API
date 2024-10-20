using GestaoEstoque_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MovimentacaoEstoqueMap : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
    {
        builder.HasKey(e => e.MovimentacaoEstoqueId);

        builder.Property(e => e.Quantidade)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.DataMovimento)
            .IsRequired();

        builder.HasOne(d => d.Produto)
            .WithMany(p => p.MovimentacoesEstoque)
            .HasForeignKey(d => d.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Observacao)
            .HasMaxLength(500);
    }
}
