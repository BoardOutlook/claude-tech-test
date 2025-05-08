using System;
using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Infrastructure;

public class FnTechTestClient : IFnTechTestClient
{
  private readonly HttpClient _httpClient;
  private readonly ApiSettings _apiSettings;

  public FnTechTestClient(IOptionsSnapshot<ApiSettings> apiSettings)
  {
    _apiSettings = apiSettings.Value;
    _httpClient = new HttpClient();
  }

  public async Task<List<GetAllCompaniesResponse>> GetAllCompaniesAsync()
  {
    return await _httpClient.GetFromJsonAsync<List<GetAllCompaniesResponse>>($"{_apiSettings.BaseUrl}/exchanges/ASX/companies?code={_apiSettings.CompaniesKey}")
        ?? [];
  }

  public async Task<List<GetAllExecutivesDto>> GetAllExecutivesAsync(string companySymbol)
  {
    return await _httpClient.GetFromJsonAsync<List<GetAllExecutivesDto>>($"{_apiSettings.BaseUrl}/companies/GSS/executives?code={_apiSettings.ApiKey}") ?? [];
  }

  public async Task<GetAverageCompensationDto> GetAverageCompensationAsync(string industryTitle)
  {
    return await _httpClient.GetFromJsonAsync<GetAverageCompensationDto>($"{_apiSettings.BaseUrl}/industries/GOLD AND SILVER ORES/benchmark?code={_apiSettings.ApiKey}") ?? new GetAverageCompensationDto();
  }
}
