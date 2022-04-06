using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Balalayka.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Balalayka.Api.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client; 
    public ApiTests(WebApplicationFactory<Program> fixture)
    {
        fixture = fixture.WithWebHostBuilder(builder =>
        {
            var root = new InMemoryDatabaseRoot();
 
            builder.ConfigureServices(services => 
            {
                services.AddScoped(sp => new DbContextOptionsBuilder<BalalaykaDbContext>()
                    .UseInMemoryDatabase("Tests", root)
                    .UseApplicationServiceProvider(sp)
                    .Options);
            });
        });
        
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task TestAddAndList()
    {
        var list = await _client.GetFromJsonAsync<List<Domain.Models.Balalayka>>("/objects");

        Assert.NotNull(list);
        Assert.Equal(0, list!.Count);

        await _client.PutAsJsonAsync("/objects", new List<IDictionary<string, string>>
        {
            new Dictionary<string, string> {{"1", "value1"}, {"5", "val32"}, {"10", "sdfgsdf7"},},
        });
        
        list = await _client.GetFromJsonAsync<List<Domain.Models.Balalayka>>("/objects");
        Assert.NotNull(list);
        Assert.Collection(list, b =>
        {
            Assert.Equal(1, b.Code);
            Assert.Equal("value1", b.Value);
        }, b =>
        {
            Assert.Equal(5, b.Code);
            Assert.Equal("val32", b.Value);
        }, b =>
        {
            Assert.Equal(10, b.Code);
            Assert.Equal("sdfgsdf7", b.Value);
        });
        
        var filtered = await _client.GetFromJsonAsync<List<Domain.Models.Balalayka>>("/objects/?codeUpperLimit=3&valueMask=val");
        Assert.Single(filtered!);
        Assert.Equal(1, filtered!.First().Code);
        Assert.Equal("value1", filtered!.First().Value);
    }
}