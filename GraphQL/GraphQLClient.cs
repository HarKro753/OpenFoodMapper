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
            var errorDetails = string.Join(", ", graphQLResponse.Errors.Select(e =>
                $"{e.Message}" + (e.Extensions != null ? $" | Extensions: {JsonSerializer.Serialize(e.Extensions)}" : "")));

            // Log the full request and response for debugging
            Serilog.Log.Error("GraphQL Error - Request: {Request}", json);
            Serilog.Log.Error("GraphQL Error - Response: {Response}", responseJson);

            throw new Exception($"GraphQL errors: {errorDetails}");
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
        public Dictionary<string, object>? Extensions { get; set; }
    }
}
