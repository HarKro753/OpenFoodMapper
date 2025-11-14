namespace OpenFood.Database.Models;

namespace Backend.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Additive> Additives { get; set; }
    public DbSet<ProductAdditive> ProductAdditives { get; set; }
    public DbSet<AdditiveType> AdditiveTypes { get; set; }
    public DbSet<HealthRisk> HealthRisks { get; set; }
    public DbSet<AdditiveHealthRisk> AdditiveHealthRisks { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<ProductIngredient> ProductIngredients { get; set; } = null!;
    public DbSet<Country> Countries { get; set; }
    public DbSet<ProductCountry> ProductCountries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProductHistory> ProductHistories { get; set; }
    public DbSet<UserFavoriteProduct> UserFavoriteProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code).HasColumnName("code");

            // Basic Information
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProductBrand).HasColumnName("brands");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");

            // Scores
            entity.Property(e => e.nutriScore).HasColumnName("nutriscore_score");
            entity.Property(e => e.EnvironmentalScore).HasColumnName("environmental_score_score");
            entity.Property(e => e.NovaGroup).HasColumnName("nova_group");
            
            // Data Quality
            entity.Property(e => e.Completeness).HasColumnName("completeness");
            entity.Property(e => e.LastImageDatetime).HasColumnName("last_image_datetime");
            entity.Property(e => e.LastModifiedDatetime).HasColumnName("last_modified_datetime");

            // Ingredients
            entity.Property(e => e.IngredientsTags).HasColumnName("ingredients_tags");
            entity.Property(e => e.LabelsEn).HasColumnName("labels_en");
            entity.Property(e => e.IngredientsText).HasColumnName("ingredients_text");
            entity.Property(e => e.AllergensEn).HasColumnName("allergens_en");
            entity.Property(e => e.FoodGroupsEn).HasColumnName("food_groups_en");

            // Nutrients per 100g
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

            // Specific Fatty Acids
            entity.Property(e => e.ButyricAcid100g).HasColumnName("butyric_acid_100g");
            entity.Property(e => e.CaproicAcid100g).HasColumnName("caproic_acid_100g");
            entity.Property(e => e.CaprylicAcid100g).HasColumnName("caprylic_acid_100g");
            entity.Property(e => e.CapricAcid100g).HasColumnName("capric_acid_100g");
            entity.Property(e => e.LauricAcid100g).HasColumnName("lauric_acid_100g");
            entity.Property(e => e.MyristicAcid100g).HasColumnName("myristic_acid_100g");
            entity.Property(e => e.PalmiticAcid100g).HasColumnName("palmitic_acid_100g");
            entity.Property(e => e.StearicAcid100g).HasColumnName("stearic_acid_100g");
            entity.Property(e => e.ArachidicAcid100g).HasColumnName("arachidic_acid_100g");
            entity.Property(e => e.BehenicAcid100g).HasColumnName("behenic_acid_100g");
            entity.Property(e => e.LignocericAcid100g).HasColumnName("lignoceric_acid_100g");
            entity.Property(e => e.CeroticAcid100g).HasColumnName("cerotic_acid_100g");
            entity.Property(e => e.MontanicAcid100g).HasColumnName("montanic_acid_100g");
            entity.Property(e => e.MelissicAcid100g).HasColumnName("melissic_acid_100g");
            entity.Property(e => e.UnsaturatedFat100g).HasColumnName("unsaturated_fat_100g");
            entity.Property(e => e.MonounsaturatedFat100g).HasColumnName("monounsaturated_fat_100g");
            entity.Property(e => e.Omega9Fat100g).HasColumnName("omega_9_fat_100g");
            entity.Property(e => e.PolyunsaturatedFat100g).HasColumnName("polyunsaturated_fat_100g");
            entity.Property(e => e.Omega3Fat100g).HasColumnName("omega_3_fat_100g");
            entity.Property(e => e.Omega6Fat100g).HasColumnName("omega_6_fat_100g");
            entity.Property(e => e.AlphaLinolenicAcid100g).HasColumnName("alpha_linolenic_acid_100g");
            entity.Property(e => e.EicosapentaenoicAcid100g).HasColumnName("eicosapentaenoic_acid_100g");
            entity.Property(e => e.DocosahexaenoicAcid100g).HasColumnName("docosahexaenoic_acid_100g");
            entity.Property(e => e.LinoleicAcid100g).HasColumnName("linoleic_acid_100g");
            entity.Property(e => e.ArachidonicAcid100g).HasColumnName("arachidonic_acid_100g");
            entity.Property(e => e.GammaLinolenicAcid100g).HasColumnName("gamma_linolenic_acid_100g");
            entity.Property(e => e.DihomoGammaLinolenicAcid100g).HasColumnName("dihomo_gamma_linolenic_acid_100g");
            entity.Property(e => e.OleicAcid100g).HasColumnName("oleic_acid_100g");
            entity.Property(e => e.ElaidicAcid100g).HasColumnName("elaidic_acid_100g");
            entity.Property(e => e.GondoicAcid100g).HasColumnName("gondoic_acid_100g");
            entity.Property(e => e.MeadAcid100g).HasColumnName("mead_acid_100g");
            entity.Property(e => e.ErucicAcid100g).HasColumnName("erucic_acid_100g");
            entity.Property(e => e.NervonicAcid100g).HasColumnName("nervonic_acid_100g");

            // Specific Carbohydrates
            entity.Property(e => e.Sucrose100g).HasColumnName("sucrose_100g");
            entity.Property(e => e.Glucose100g).HasColumnName("glucose_100g");
            entity.Property(e => e.Fructose100g).HasColumnName("fructose_100g");
            entity.Property(e => e.Galactose100g).HasColumnName("galactose_100g");
            entity.Property(e => e.Lactose100g).HasColumnName("lactose_100g");
            entity.Property(e => e.Maltose100g).HasColumnName("maltose_100g");
            entity.Property(e => e.Maltodextrins100g).HasColumnName("maltodextrins_100g");
            entity.Property(e => e.Psicose100g).HasColumnName("psicose_100g");
            entity.Property(e => e.Starch100g).HasColumnName("starch_100g");
            entity.Property(e => e.Polyols100g).HasColumnName("polyols_100g");
            entity.Property(e => e.Erythritol100g).HasColumnName("erythritol_100g");
            entity.Property(e => e.Isomalt100g).HasColumnName("isomalt_100g");
            entity.Property(e => e.Maltitol100g).HasColumnName("maltitol_100g");
            entity.Property(e => e.Sorbitol100g).HasColumnName("sorbitol_100g");

            // Additional Fiber & Proteins
            entity.Property(e => e.SolubleFiber100g).HasColumnName("soluble_fiber_100g");
            entity.Property(e => e.InsolubleFiber100g).HasColumnName("insoluble_fiber_100g");
            entity.Property(e => e.Casein100g).HasColumnName("casein_100g");
            entity.Property(e => e.SerumProteins100g).HasColumnName("serum_proteins_100g");
            entity.Property(e => e.Nucleotides100g).HasColumnName("nucleotides_100g");

            // Additional Vitamins
            entity.Property(e => e.BetaCarotene100g).HasColumnName("beta_carotene_100g");
            entity.Property(e => e.VitaminD100g).HasColumnName("vitamin_d_100g");
            entity.Property(e => e.VitaminE100g).HasColumnName("vitamin_e_100g");
            entity.Property(e => e.VitaminK100g).HasColumnName("vitamin_k_100g");
            entity.Property(e => e.VitaminB1_100g).HasColumnName("vitamin_b1_100g");
            entity.Property(e => e.VitaminB2_100g).HasColumnName("vitamin_b2_100g");
            entity.Property(e => e.VitaminPp100g).HasColumnName("vitamin_pp_100g");
            entity.Property(e => e.VitaminB6_100g).HasColumnName("vitamin_b6_100g");
            entity.Property(e => e.VitaminB9_100g).HasColumnName("vitamin_b9_100g");
            entity.Property(e => e.Folates100g).HasColumnName("folates_100g");
            entity.Property(e => e.VitaminB12_100g).HasColumnName("vitamin_b12_100g");
            entity.Property(e => e.Biotin100g).HasColumnName("biotin_100g");
            entity.Property(e => e.PantothenicAcid100g).HasColumnName("pantothenic_acid_100g");

            // Additional Minerals
            entity.Property(e => e.Silica100g).HasColumnName("silica_100g");
            entity.Property(e => e.Bicarbonate100g).HasColumnName("bicarbonate_100g");
            entity.Property(e => e.Chloride100g).HasColumnName("chloride_100g");
            entity.Property(e => e.Phosphorus100g).HasColumnName("phosphorus_100g");
            entity.Property(e => e.Copper100g).HasColumnName("copper_100g");
            entity.Property(e => e.Manganese100g).HasColumnName("manganese_100g");
            entity.Property(e => e.Fluoride100g).HasColumnName("fluoride_100g");
            entity.Property(e => e.Selenium100g).HasColumnName("selenium_100g");
            entity.Property(e => e.Chromium100g).HasColumnName("chromium_100g");
            entity.Property(e => e.Molybdenum100g).HasColumnName("molybdenum_100g");
            entity.Property(e => e.Iodine100g).HasColumnName("iodine_100g");

            // Other Nutrients
            entity.Property(e => e.Caffeine100g).HasColumnName("caffeine_100g");
            entity.Property(e => e.Taurine100g).HasColumnName("taurine_100g");
            entity.Property(e => e.Methylsulfonylmethane100g).HasColumnName("methylsulfonylmethane_100g");
            entity.Property(e => e.Ph100g).HasColumnName("ph_100g");

            // Fruits/Vegetables
            entity.Property(e => e.FruitsVegetablesNuts100g).HasColumnName("fruits_vegetables_nuts_100g");
            entity.Property(e => e.FruitsVegetablesNutsDried100g).HasColumnName("fruits_vegetables_nuts_dried_100g");
            entity.Property(e => e.FruitsVegetablesNutsEstimate100g).HasColumnName("fruits_vegetables_nuts_estimate_100g");
            entity.Property(e => e.FruitsVegetablesNutsEstimateFromIngredients100g).HasColumnName("fruits_vegetables_nuts_estimate_from_ingredients_100g");
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

            entity.HasKey(e => new { e.ProductCode, e.CategoryId });

            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductCategories)
                .HasForeignKey(e => e.ProductCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Category)
                .WithMany(e => e.ProductCategories)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.CategoryId).HasDatabaseName("idx_product_categories_category");
        });

        modelBuilder.Entity<Additive>(entity =>
        {
            entity.ToTable("additives");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Risk)
                .HasColumnName("risk")
                .HasConversion<int>()
                .IsRequired(false);
            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Additives_Risk_ValidValues", "risk IN (0, 1, 2, 3)");
            });
            entity.Property(e => e.AdditiveTypeId).HasColumnName("additive_type_id");

            entity.HasOne(e => e.AdditiveType)
                .WithMany(e => e.Additives)
                .HasForeignKey(e => e.AdditiveTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<ProductAdditive>(entity =>
        {
            entity.ToTable("product_additives");

            entity.HasKey(e => new { e.ProductCode, e.AdditiveId });

            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.AdditiveId).HasColumnName("additive_id");

            entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductAdditives)
                .HasForeignKey(e => e.ProductCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.AdditiveId).HasDatabaseName("idx_product_additives_additive");
        });

        modelBuilder.Entity<AdditiveType>(entity =>
        {
            entity.ToTable("additive_types");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(50);

            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<HealthRisk>(entity =>
        {
            entity.ToTable("health_risks");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();

            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<AdditiveHealthRisk>(entity =>
        {
            entity.ToTable("additive_health_risks");

            entity.HasKey(e => new { e.AdditiveId, e.HealthRiskId });

            entity.Property(e => e.AdditiveId).HasColumnName("additive_id");
            entity.Property(e => e.HealthRiskId).HasColumnName("health_risk_id");

            entity.HasOne(e => e.Additive)
                .WithMany(e => e.AdditiveHealthRisks)
                .HasForeignKey(e => e.AdditiveId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.HealthRisk)
                .WithMany(e => e.AdditiveHealthRisks)
                .HasForeignKey(e => e.HealthRiskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.HealthRiskId).HasDatabaseName("idx_additive_health_risks_health_risk");
        });

        modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("ingredients");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IsVegetable).IsRequired();

                entity.HasMany(e => e.ProductIngredients)
                    .WithOne(pi => pi.Ingredient)
                    .HasForeignKey(pi => pi.IngredientId);
            });

        modelBuilder.Entity<ProductIngredient>(entity =>
            {
                entity.ToTable("product_ingredients");

                entity.HasKey(e => new { e.ProductCode, e.IngredientId });

                entity.HasOne(pi => pi.Product)
                    .WithMany(p => p.ProductIngredients)
                    .HasForeignKey(pi => pi.ProductCode);

                entity.HasOne(pi => pi.Ingredient)
                    .WithMany(i => i.ProductIngredients)
                    .HasForeignKey(pi => pi.IngredientId);
            });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("countries");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();

            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<ProductCountry>(entity =>
        {
            entity.ToTable("product_countries");
            entity.HasKey(pc => new { pc.ProductCode, pc.CountryId });

            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.CountryId).HasColumnName("country_id");

            entity.HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCountries)
                .HasForeignKey(pc => pc.ProductCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pc => pc.Country)
                .WithMany(c => c.ProductCountries)
                .HasForeignKey(pc => pc.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(pc => pc.CountryId)
                .HasDatabaseName("idx_product_countries_country");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Username).HasColumnName("username").IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<ProductHistory>(entity =>
        {
            entity.ToTable("product_history");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.ScannedAt).HasColumnName("scanned_at");

            entity.HasOne(e => e.User)
                .WithMany(e => e.ProductHistory)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductHistories)
                .HasForeignKey(e => e.ProductCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ProductCode);
            entity.HasIndex(e => e.ScannedAt);
        });

        modelBuilder.Entity<UserFavoriteProduct>(entity =>
        {
            entity.ToTable("user_favorite_products");
            entity.HasKey(e => new { e.UserId, e.ProductCode });

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ProductCode).HasColumnName("product_code");
            entity.Property(e => e.FavoritedAt).HasColumnName("favorited_at");

            entity.HasOne(e => e.User)
                .WithMany(e => e.FavoriteProducts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Product)
                .WithMany(e => e.UserFavorites)
                .HasForeignKey(e => e.ProductCode)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProductCode);
            entity.HasIndex(e => e.FavoritedAt);
        });
    }
}
