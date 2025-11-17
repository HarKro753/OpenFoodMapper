using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenFoodUploader.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "additive_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additive_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "allergens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allergens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "food_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_food_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "health_risks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_health_risks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsVegetable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_labels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    code = table.Column<decimal>(type: "numeric", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    product_name = table.Column<string>(type: "text", nullable: true),
                    brands = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    nutriscore_score = table.Column<int>(type: "integer", nullable: true),
                    nova_group = table.Column<int>(type: "integer", nullable: true),
                    environmental_score_score = table.Column<decimal>(type: "numeric", nullable: true),
                    ingredients_tags = table.Column<string>(type: "text", nullable: true),
                    ingredients_text = table.Column<string>(type: "text", nullable: true),
                    completeness = table.Column<decimal>(type: "numeric", nullable: true),
                    last_image_datetime = table.Column<string>(type: "text", nullable: true),
                    last_modified_datetime = table.Column<string>(type: "text", nullable: true),
                    energykcal_100g = table.Column<decimal>(name: "energy-kcal_100g", type: "numeric", nullable: true),
                    energyfromfat_100g = table.Column<decimal>(name: "energy-from-fat_100g", type: "numeric", nullable: true),
                    fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    saturatedfat_100g = table.Column<decimal>(name: "saturated-fat_100g", type: "numeric", nullable: true),
                    transfat_100g = table.Column<decimal>(name: "trans-fat_100g", type: "numeric", nullable: true),
                    cholesterol_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    carbohydrates_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    sugars_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    addedsugars_100g = table.Column<decimal>(name: "added-sugars_100g", type: "numeric", nullable: true),
                    fiber_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    proteins_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    salt_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    sodium_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    alcohol_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamina_100g = table.Column<decimal>(name: "vitamin-a_100g", type: "numeric", nullable: true),
                    vitaminc_100g = table.Column<decimal>(name: "vitamin-c_100g", type: "numeric", nullable: true),
                    calcium_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    iron_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    magnesium_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    zinc_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    potassium_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    butyric_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    caproic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    caprylic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    capric_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    lauric_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    myristic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    palmitic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    stearic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    arachidic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    behenic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    lignoceric_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    cerotic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    montanic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    melissic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    unsaturated_fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    monounsaturated_fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    omega_9_fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    polyunsaturated_fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    omega_3_fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    omega_6_fat_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    alpha_linolenic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    eicosapentaenoic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    docosahexaenoic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    linoleic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    arachidonic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    gamma_linolenic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    dihomo_gamma_linolenic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    oleic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    elaidic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    gondoic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    mead_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    erucic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    nervonic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    sucrose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    glucose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fructose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    galactose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    lactose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    maltose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    maltodextrins_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    psicose_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    starch_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    polyols_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    erythritol_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    isomalt_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    maltitol_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    sorbitol_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    soluble_fiber_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    insoluble_fiber_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    casein_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    serum_proteins_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    nucleotides_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    beta_carotene_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_d_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_e_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_k_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_b1_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_b2_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_pp_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_b6_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_b9_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    folates_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    vitamin_b12_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    biotin_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    pantothenic_acid_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    silica_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    bicarbonate_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    chloride_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    phosphorus_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    copper_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    manganese_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fluoride_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    selenium_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    chromium_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    molybdenum_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    iodine_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    caffeine_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    taurine_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    methylsulfonylmethane_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    ph_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fruits_vegetables_nuts_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fruits_vegetables_nuts_dried_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fruits_vegetables_nuts_estimate_100g = table.Column<decimal>(type: "numeric", nullable: true),
                    fruits_vegetables_nuts_estimate_from_ingredients_100g = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "additives",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    risk = table.Column<int>(type: "integer", nullable: true),
                    additive_type_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additives", x => x.id);
                    table.CheckConstraint("CK_Additives_Risk_ValidValues", "risk IN (0, 1, 2, 3)");
                    table.ForeignKey(
                        name: "FK_additives_additive_types_additive_type_id",
                        column: x => x.additive_type_id,
                        principalTable: "additive_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "product_allergens",
                columns: table => new
                {
                    product_code = table.Column<decimal>(type: "numeric", nullable: false),
                    allergen_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_allergens", x => new { x.product_code, x.allergen_id });
                    table.ForeignKey(
                        name: "FK_product_allergens_allergens_allergen_id",
                        column: x => x.allergen_id,
                        principalTable: "allergens",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_allergens_products_product_code",
                        column: x => x.product_code,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    product_code = table.Column<decimal>(type: "numeric", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => new { x.product_code, x.category_id });
                    table.ForeignKey(
                        name: "FK_product_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_categories_products_product_code",
                        column: x => x.product_code,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_countries",
                columns: table => new
                {
                    product_code = table.Column<decimal>(type: "numeric", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_countries", x => new { x.product_code, x.country_id });
                    table.ForeignKey(
                        name: "FK_product_countries_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_countries_products_product_code",
                        column: x => x.product_code,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_food_groups",
                columns: table => new
                {
                    product_code = table.Column<decimal>(type: "numeric", nullable: false),
                    food_group_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_food_groups", x => new { x.product_code, x.food_group_id });
                    table.ForeignKey(
                        name: "FK_product_food_groups_food_groups_food_group_id",
                        column: x => x.food_group_id,
                        principalTable: "food_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_food_groups_products_product_code",
                        column: x => x.product_code,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_ingredients",
                columns: table => new
                {
                    ProductCode = table.Column<decimal>(type: "numeric", nullable: false),
                    IngredientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_ingredients", x => new { x.ProductCode, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_product_ingredients_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_ingredients_products_ProductCode",
                        column: x => x.ProductCode,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_labels",
                columns: table => new
                {
                    product_code = table.Column<decimal>(type: "numeric", nullable: false),
                    label_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_labels", x => new { x.product_code, x.label_id });
                    table.ForeignKey(
                        name: "FK_product_labels_labels_label_id",
                        column: x => x.label_id,
                        principalTable: "labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_labels_products_product_code",
                        column: x => x.product_code,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "additive_health_risks",
                columns: table => new
                {
                    additive_id = table.Column<int>(type: "integer", nullable: false),
                    health_risk_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_additive_health_risks", x => new { x.additive_id, x.health_risk_id });
                    table.ForeignKey(
                        name: "FK_additive_health_risks_additives_additive_id",
                        column: x => x.additive_id,
                        principalTable: "additives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_additive_health_risks_health_risks_health_risk_id",
                        column: x => x.health_risk_id,
                        principalTable: "health_risks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_additives",
                columns: table => new
                {
                    product_code = table.Column<decimal>(type: "numeric", nullable: false),
                    additive_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_additives", x => new { x.product_code, x.additive_id });
                    table.ForeignKey(
                        name: "FK_product_additives_additives_additive_id",
                        column: x => x.additive_id,
                        principalTable: "additives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_additives_products_product_code",
                        column: x => x.product_code,
                        principalTable: "products",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_additive_health_risks_health_risk",
                table: "additive_health_risks",
                column: "health_risk_id");

            migrationBuilder.CreateIndex(
                name: "IX_additive_types_name",
                table: "additive_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_additives_additive_type_id",
                table: "additives",
                column: "additive_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_additives_name",
                table: "additives",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_allergens_name",
                table: "allergens",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_name",
                table: "categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_name",
                table: "countries",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_food_groups_name",
                table: "food_groups",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_health_risks_name",
                table: "health_risks",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_labels_name",
                table: "labels",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_product_additives_additive",
                table: "product_additives",
                column: "additive_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_allergens_allergen",
                table: "product_allergens",
                column: "allergen_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_categories_category",
                table: "product_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_countries_country",
                table: "product_countries",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_food_groups_food_group",
                table: "product_food_groups",
                column: "food_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_ingredients_IngredientId",
                table: "product_ingredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "idx_product_labels_label",
                table: "product_labels",
                column: "label_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "additive_health_risks");

            migrationBuilder.DropTable(
                name: "product_additives");

            migrationBuilder.DropTable(
                name: "product_allergens");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "product_countries");

            migrationBuilder.DropTable(
                name: "product_food_groups");

            migrationBuilder.DropTable(
                name: "product_ingredients");

            migrationBuilder.DropTable(
                name: "product_labels");

            migrationBuilder.DropTable(
                name: "health_risks");

            migrationBuilder.DropTable(
                name: "additives");

            migrationBuilder.DropTable(
                name: "allergens");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "food_groups");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "labels");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "additive_types");
        }
    }
}
