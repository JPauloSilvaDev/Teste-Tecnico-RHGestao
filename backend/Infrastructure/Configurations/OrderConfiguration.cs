using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            // Relacionamento 1:N com Cliente
            builder.HasOne(o => o.Client)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.ClientId)
                   .IsRequired(); // Requisito: Pedido deve possuir 1 cliente 

            // TotalValue é uma propriedade calculada (não mapeada como coluna física)
            builder.Ignore(o => o.TotalValue);
        }
    }
}
