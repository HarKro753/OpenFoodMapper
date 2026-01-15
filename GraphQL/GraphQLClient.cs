using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFood.GraphQL;

public class GraphQLClient
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private static readonly JsonSerializerOptions DeserializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GraphQLClient(string endpoint)
    {
        _endpoint = endpoint;
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };
    }

    public async Task<TResponse?> SendMutationAsync<TResponse>(string mutation, object variables) where TResponse : class
    {
        var request = new
        {
            query = mutation,
            variables
        };

        var json = JsonSerializer.Serialize(request, SerializerOptions);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_endpoint, content);

        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var graphQLResponse = JsonSerializer.Deserialize<GraphQLResponse<TResponse>>(responseJson, DeserializerOptions);

        if (graphQLResponse?.Errors != null && graphQLResponse.Errors.Count > 0)
        {
            var errorMessages = string.Join(", ", graphQLResponse.Errors.Select(e => e.Message));
            throw new Exception($"GraphQL errors: {errorMessages}");
        }

        return graphQLResponse?.Data;
    }

    private class GraphQLResponse<T>
    {
        public T? Data { get; set; }
        public List<GraphQLError>? Errors { get; set; }
    }

    private class GraphQLError
    {
        public string Message { get; set; } = string.Empty;
    }
}
