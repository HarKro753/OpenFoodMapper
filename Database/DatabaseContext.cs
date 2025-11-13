using Microsoft.EntityFrameworkCore;
using OpenFood.Database.Models;

namespace OpenFood.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.Brands).HasColumnName("brands");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.NutriscoreScore).HasColumnName("nutriscore_score");
            entity.Property(e => e.NovaGroup).HasColumnName("nova_group");
            entity.Property(e => e.EnvironmentalScoreScore).HasColumnName("environmental_score_score");
            entity.Property(e => e.Completeness).HasColumnName("completeness");
            entity.Property(e => e.LastImageDatetime).HasColumnName("last_image_datetime");
            entity.Property(e => e.LastModifiedDatetime).HasColumnName("last_modified_datetime");
            entity.Property(e => e.AdditivesEn).HasColumnName("additives_en");
            entity.Property(e => e.IngredientsTags).HasColumnName("ingredients_tags");
            entity.Property(e => e.EnergyKcal100g).HasColumnName("energy-kcal_100g");
            entity.Property(e => e.EnergyFromFat100g).HasColumnName("energy-from-fat_100g");
            entity.Property(e => e.Fat100g).HasColumnName("fat_100g");
            entity.Property(e => e.SaturatedFat100g).HasColumnName("saturated-fat_100g");
            entity.Property(e => e.TransFat100g).HasColumnName("trans-fat_100g");
            entity.Property(e => e.Cholesterol100g).HasColumnName("cholesterol_100g");
            entity.Property(e => e.Carbohydrates100g).HasColumnName("carbohydrates_100g");
            entity.Property(e => e.Sugars100g).HasColumnName("sugars_100g");
            entity.Property(e => e.AddedSugars100g).HasColumnName("added-sugars_100g");
            entity.Property(e => e.Fiber100g).HasColumnName("fiber_100g");
            entity.Property(e => e.Proteins100g).HasColumnName("proteins_100g");
            entity.Property(e => e.Salt100g).HasColumnName("salt_100g");
            entity.Property(e => e.Sodium100g).HasColumnName("sodium_100g");
            entity.Property(e => e.Alcohol100g).HasColumnName("alcohol_100g");
            entity.Property(e => e.VitaminA100g).HasColumnName("vitamin-a_100g");
            entity.Property(e => e.VitaminC100g).HasColumnName("vitamin-c_100g");
            entity.Property(e => e.Calcium100g).HasColumnName("calcium_100g");
            entity.Property(e => e.Iron100g).HasColumnName("iron_100g");
            entity.Property(e => e.Magnesium100g).HasColumnName("magnesium_100g");
            entity.Property(e => e.Zinc100g).HasColumnName("zinc_100g");
            entity.Property(e => e.Potassium100g).HasColumnName("potassium_100g");
            entity.Property(e => e.CategoriesEn).HasColumnName("categories_en");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();

            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("product_categories");
            entity.HasKey(pc => new { pc.ProductCode, pc.CategoryId });

            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(pc => pc.CategoryId)
                .HasDatabaseName("idx_product_categories_category");
        });
    }
}
