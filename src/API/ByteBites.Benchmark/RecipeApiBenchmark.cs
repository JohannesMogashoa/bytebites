using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ByteBites.Benchmark;

[SimpleJob(RuntimeMoniker.Net80,
    launchCount: 1, // start 1 process
    warmupCount: 1, // 1 “measurement run”
    invocationCount: 500)] // ms per iteration (ignored with invocationCount)
[GcServer(true)]
public class RecipeApiBenchmark
{
    private HttpClient _client;

    [GlobalSetup]
    public void Setup()
    {
        _client = new HttpClient { BaseAddress = new Uri("https://localhost:8111/") };
    }

    [Benchmark]
    public async Task GetRecipes()
    {
        var resp = await _client.PostAsJsonAsync("api/recipes/filter", new FilterRecipeDto());
        resp.EnsureSuccessStatusCode();
    }
}