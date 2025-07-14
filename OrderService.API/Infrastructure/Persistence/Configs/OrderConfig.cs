using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.API.Domain.Entities;
namespace OrderService.API.Infrastructure.Persistence.Configs;
public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Province).IsRequired();
            address.Property(a => a.City).IsRequired();
            address.Property(a => a.Street).IsRequired();
            address.Property(a => a.ZipCode).IsRequired();
        });
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired();
        builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}