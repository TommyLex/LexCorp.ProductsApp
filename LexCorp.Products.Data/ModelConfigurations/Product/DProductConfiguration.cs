using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexCorp.Products.Data.Models.Product.Configurations
{
  /// <summary>
  /// Configures the database mapping for the DProduct entity.
  /// </summary>
  public class DProductConfiguration : IEntityTypeConfiguration<DProduct>
  {
    /// <summary>
    /// Configures the entity of type DProduct.
    /// </summary>
    /// <param name="builder">Entity type builder.</param>
    public void Configure(EntityTypeBuilder<DProduct> builder)
    {
      builder.ToTable("d_product", "products");

      builder.HasKey(t => t.Guid);

      builder.Property(t => t.Guid)
        .IsRequired()
        .HasColumnName("guid");

      builder.Property(t => t.Name)
        .IsRequired()
        .HasMaxLength(100)
        .HasColumnName("name");

      builder.Property(t => t.ImageUrl)
        .IsRequired()
        .HasMaxLength(255)
        .HasColumnName("image_url");

      builder.Property(t => t.Price)
        .IsRequired(false)
        .HasColumnType("decimal(18,2)")
        .HasColumnName("price");

      builder.Property(t => t.Description)
        .IsRequired(false)
        .HasColumnType("nvarchar(max)")
        .HasColumnName("description");

      builder.Property(t => t.Quantity)
        .IsRequired(true)
        .HasColumnName("quantity");
    }
  }
}