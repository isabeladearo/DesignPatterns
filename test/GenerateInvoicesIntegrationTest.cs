using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using src.Application.UseCase;
using src.Domain;
using Xunit;

namespace test;

public class GenerateInvoicesIntegrationTest
{    
    public Mock<HttpContext> httpContextMock = new();
    public Mock<HttpRequest> requestMock = new();
    public Mock<HttpResponse> responseMock = new();

    public GenerateInvoicesIntegrationTest()
    {
        requestMock.SetupGet(r => r.Headers["User-Agent"]).Returns("Test User-Agent");
        requestMock.SetupGet(r => r.Host).Returns(new HostString("testhost.com"));

        httpContextMock.SetupGet(c => c.Request).Returns(requestMock.Object);
        httpContextMock.SetupGet(c => c.Response).Returns(responseMock.Object);
    }

    [Fact]
    public async Task ShouldGenerateInvoicesForCashType_BasedOnJsonOutput()
    {
        var input = new Input() { InvoiceMonth = 03, InvoiceYear = 2024, InvoiceType = "cash", ResponseType = "json_response" };

        var response = await PostAsync(input);

        var output = await response.Content.ReadFromJsonAsync<List<Invoice>>();

        output.Should().NotBeNull();
        output.FirstOrDefault()?.Date.Month.Should().Be(input.InvoiceMonth);
        output.FirstOrDefault()?.Amount.Should().Be(16200); 
    }

    [Fact]
    public async Task ShouldGenerateInvoicesForAccrualType_BasedOnJsonOutput()
    {
        var input = new Input() { InvoiceMonth = 03, InvoiceYear = 2024, InvoiceType = "accrual", ResponseType = "json_response" };

        var response = await PostAsync(input);

        var output = await response.Content.ReadFromJsonAsync<List<Invoice>>();

        output.Should().NotBeNull();

        var currentYear = input.InvoiceYear;

        for (int i = 0; i < output.Count; i++)
        {
            int currentMonth = (input.InvoiceMonth + i - 1) % 12 + 1;

            if (currentMonth == 1 && i > 0)
                currentYear++;

            output[i].Date.Month.Should().Be(currentMonth);
            output[i].Date.Year.Should().Be(currentYear);
            output[i].Amount.Should().Be(1500);
        }
    }

    [Fact]
    public async Task ShouldGenerateInvoicesForCashType_BasedOnCvsOutput()
    {
        var input = new Input() { InvoiceMonth = 03, InvoiceYear = 2024, InvoiceType  = "cash", ResponseType = "csv_response" };

        var response = await PostAsync(input);

        var output = await response.Content.ReadAsStringAsync();

        output.Should().NotBeNull();
        output.Should().Contain($"{input.InvoiceMonth}");
        output.Should().Contain($"{input.InvoiceYear}");
        output.Should().Contain("16200");
    }

    [Fact]
    public async Task ShouldGenerateInvoicesForAccrualType_BasedOnCsvOutput()
    {
        var input = new Input() { InvoiceMonth = 03, InvoiceYear = 2024, InvoiceType  = "accrual", ResponseType = "csv_response" };

        var response = await PostAsync(input);

        var output = await response.Content.ReadAsStringAsync();

        output.Should().NotBeNull();

        var outputList = output.Split(Environment.NewLine).ToList();

        var currentYear = input.InvoiceYear;

        for (int i = 0; i < outputList.Count; i++)
        {
            int currentMonth = (input.InvoiceMonth + i - 1) % 12 + 1;

            if (currentMonth == 1 && i > 0)
                currentYear++;

            output.Should().Contain($"{input.InvoiceMonth}");
            output.Should().Contain($"{input.InvoiceYear}");
            outputList[i].Should().Contain("1500");
        }
    }

    private static async Task<HttpResponseMessage> PostAsync(Input requestBody)
    {
        var endpoint = "http://localhost:5016/generate_invoices";

        using var factory = new WebApplicationFactory<Program>();

        using var httpClient = ConfigureClient(factory);

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return await httpClient.PostAsync(endpoint, content);
    }

    private static HttpClient ConfigureClient(WebApplicationFactory<Program> factory)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services => 
            {
                services.AddHttpsRedirection(options => options.HttpsPort = 5016);
            });
        }).CreateClient();

        httpClient.DefaultRequestHeaders.Add("User-Agent", "IntegrationTest");

        return httpClient;
    }
}
