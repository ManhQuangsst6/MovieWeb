using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieWeb.Controllers;
using MovieWeb.Models;
using Xunit;

namespace MovieWeb.Tests;

public class DatabaseConfigurationTests
{
    private const string Connection = "Server=127.0.0.1,15433;Database=ConfigurationTest;User Id=test;Password=unused;Encrypt=True;Connect Timeout=1";

    [Fact]
    public void ContextPreservesConfiguredSqlServerConnection()
    {
        var options = new DbContextOptionsBuilder<MovieDbContext>()
            .UseSqlServer(Connection).Options;
        using var context = new MovieDbContext(options);

        Assert.Equal(Connection, context.Database.GetConnectionString());
        Assert.Equal("ConfigurationTest", context.Database.GetDbConnection().Database);
    }

    [Theory]
    [InlineData("/Home/Privacy", "Privacy Policy")]
    [InlineData("/Access/Login", "<form")]
    [InlineData("/Access/Register", "<form")]
    public async Task ConfiguredApplicationRendersPagesWithoutOpeningDatabase(string path, string content)
    {
        using var app = new ConfiguredApplication();
        using var client = app.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
        using var response = await client.GetAsync(path);

        response.EnsureSuccessStatusCode();
        Assert.Contains(content, await response.Content.ReadAsStringAsync());
        using var scope = app.Services.CreateScope();
        Assert.Equal(Connection, scope.ServiceProvider.GetRequiredService<MovieDbContext>()
            .Database.GetConnectionString());
    }

    private sealed class ConfiguredApplication : WebApplicationFactory<HomeController>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("ConnectionStrings:MovieDbContext", Connection);
        }
    }
}
