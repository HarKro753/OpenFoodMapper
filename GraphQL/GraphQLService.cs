namespace OpenFood.GraphQL;

public class GraphQLService
{
    private readonly GraphQLClient _client;

    public GraphQLService(string endpoint)
    {
        _client = new GraphQLClient(endpoint);
    }

    public async Task<int> UpsertProductsAsync(List<ProductInput> products)
    {
        const string mutation = @"
            mutation UpsertProducts($input: UpsertProductsInput!) {
                upsertProducts(input: $input) {
                    productsUpserted
                    message
                }
            }";

        var variables = new
        {
            input = new UpsertProductsInput(products)
        };

        var response = await _client.SendMutationAsync<UpsertProductsResponse>(mutation, variables);
        return response?.UpsertProducts.ProductsUpserted ?? 0;
    }

    public async Task LinkProductCategoriesAsync(decimal productCode, List<string> categoryNames)
    {
        if (categoryNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductCategories($input: LinkProductCategoriesInput!) {
                linkProductCategories(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductCategoriesInput(productCode, categoryNames)
        };

        await _client.SendMutationAsync<LinkProductCategoriesResponse>(mutation, variables);
    }

    public async Task LinkProductAdditivesAsync(decimal productCode, List<string> additiveNames)
    {
        if (additiveNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductAdditives($input: LinkProductAdditivesInput!) {
                linkProductAdditives(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductAdditivesInput(productCode, additiveNames)
        };

        await _client.SendMutationAsync<LinkProductAdditivesResponse>(mutation, variables);
    }

    public async Task LinkProductIngredientsAsync(decimal productCode, List<string> ingredientNames)
    {
        if (ingredientNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductIngredients($input: LinkProductIngredientsInput!) {
                linkProductIngredients(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductIngredientsInput(productCode, ingredientNames)
        };

        await _client.SendMutationAsync<LinkProductIngredientsResponse>(mutation, variables);
    }

    public async Task LinkProductCountriesAsync(decimal productCode, List<string> countryNames)
    {
        if (countryNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductCountries($input: LinkProductCountriesInput!) {
                linkProductCountries(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductCountriesInput(productCode, countryNames)
        };

        await _client.SendMutationAsync<LinkProductCountriesResponse>(mutation, variables);
    }

    public async Task LinkProductAllergensAsync(decimal productCode, List<string> allergenNames)
    {
        if (allergenNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductAllergens($input: LinkProductAllergensInput!) {
                linkProductAllergens(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductAllergensInput(productCode, allergenNames)
        };

        await _client.SendMutationAsync<LinkProductAllergensResponse>(mutation, variables);
    }

    public async Task LinkProductFoodGroupsAsync(decimal productCode, List<string> foodGroupNames)
    {
        if (foodGroupNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductFoodGroups($input: LinkProductFoodGroupsInput!) {
                linkProductFoodGroups(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductFoodGroupsInput(productCode, foodGroupNames)
        };

        await _client.SendMutationAsync<LinkProductFoodGroupsResponse>(mutation, variables);
    }

    public async Task LinkProductLabelsAsync(decimal productCode, List<string> labelNames)
    {
        if (labelNames.Count == 0) return;

        const string mutation = @"
            mutation LinkProductLabels($input: LinkProductLabelsInput!) {
                linkProductLabels(input: $input) {
                    success
                }
            }";

        var variables = new
        {
            input = new LinkProductLabelsInput(productCode, labelNames)
        };

        await _client.SendMutationAsync<LinkProductLabelsResponse>(mutation, variables);
    }
}
