using System.Web;
using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Infrastructure;

public class FnTechTestClient : IFnTechTestClient
{
  private readonly HttpClient _httpClient;
  private readonly ApiSettings _apiSettings;
  private readonly ILogger<FnTechTestClient> _logger;

  public FnTechTestClient(IOptionsSnapshot<ApiSettings> apiSettings, HttpClient httpClient, ILogger<FnTechTestClient> logger)
  {
    _apiSettings = apiSettings.Value;
    _httpClient = httpClient;
    _logger = logger;
  }

  public async Task<List<GetAllCompaniesResponse>> GetAllCompaniesAsync()
  {
    try
    {
      var url = $"{_apiSettings.BaseUrl}/exchanges/ASX/companies?code={_apiSettings.CompaniesKey}";
      _logger.LogInformation("Fetching companies from {Url}", url);

      var response = await _httpClient.GetAsync(url);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadFromJsonAsync<List<GetAllCompaniesResponse>>() ?? [];
    }
    catch (HttpRequestException ex)
    {
      _logger.LogError(ex, "Error fetching companies: {Message}", ex.Message);
      return [];
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching companies: {Message}", ex.Message);
      return [];
    }
  }

  public async Task<List<GetAllExecutivesDto>> GetAllExecutivesAsync(string companySymbol)
  {
    try
    {
      var encodedSymbol = HttpUtility.UrlEncode(companySymbol);
      var url = $"{_apiSettings.BaseUrl}/companies/{encodedSymbol}/executives?code={_apiSettings.ApiKey}";
      _logger.LogInformation("Fetching executives for company {Symbol}", companySymbol);

      var response = await _httpClient.GetAsync(url);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadFromJsonAsync<List<GetAllExecutivesDto>>() ?? [];
    }
    catch (HttpRequestException ex)
    {
      _logger.LogError(ex, "Error fetching executives for company {Symbol}: {Message}", companySymbol, ex.Message);
      return [];
    } catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching executives for company {Symbol}: {Message}", companySymbol, ex.Message);
      return [];
    }
  }

  public async Task<GetAverageCompensationDto?> GetAverageCompensationAsync(string industryTitle)
  {
    try
    {
      var encodedIndustry = HttpUtility.UrlEncode(industryTitle);
      var url = $"{_apiSettings.BaseUrl}/industries/{encodedIndustry}/benchmark?code={_apiSettings.ApiKey}";
      _logger.LogInformation("Fetching average compensation for industry {Industry}", industryTitle);

      var response = await _httpClient.GetAsync(url);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadFromJsonAsync<GetAverageCompensationDto>() ?? new GetAverageCompensationDto();
    }
    catch (HttpRequestException ex)
    {
      _logger.LogError(ex, "Error fetching average compensation for industry {Industry}: {Message}", industryTitle, ex.Message);
      return null;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching average compensation for industry {Industry}: {Message}", industryTitle, ex.Message);
      return null;
    }
  }
}
