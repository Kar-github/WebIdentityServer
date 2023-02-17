using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using WebApiDemo.Application.Entity;

namespace WebApiDemo.Infrastructure.DB
{
    public partial class WebApiDemoDbContext : DbContext
    {
        void ConfigureProduct(EntityTypeConfiguration<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Code)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
