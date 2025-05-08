using System;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Application.Common;

namespace Application.Services;

public class CompanyService : ICompanyService
{
  private readonly IFnTechTestClient _fnTechTestClient;
  private readonly ILogger<CompanyService> _logger;
  private readonly IMemoryCache _cache;
  private const string CacheKeyPrefix = "CompanyService_";
  private static TimeSpan CacheDuration;

  public CompanyService(IFnTechTestClient fnTechTestClient, ILogger<CompanyService> logger, IMemoryCache cache, IOptionsSnapshot<ApiSettings> apiSettings)
  {
    _fnTechTestClient = fnTechTestClient;
    _logger = logger;
    _cache = cache;
    CacheDuration = TimeSpan.FromMinutes(apiSettings.Value.CacheDurationInMinutes);
  }

  public async Task<IEnumerable<CompensationDto>> GetExecutivesWithCompensationAboveAverage()
  {
    try
    {
      string cacheKey = $"{CacheKeyPrefix}ExecutivesAboveAverage";
      if (_cache.TryGetValue(cacheKey, out IEnumerable<CompensationDto> cachedResult))
      {
        _logger.LogInformation("Retrieved executives with compensation above average from cache");
        return cachedResult;
      }

      var companies = await GetCompaniesAsync();
      var executives = await GetAllExecutivesAsync(companies);
      var industryAverages = await GetIndustryAveragesAsync(executives);

      var result = executives
          .Where(r => industryAverages.ContainsKey(r.IndustryTitle))
          .Where(exec => exec.Salary > industryAverages[exec.IndustryTitle])
          .Select(exec => new CompensationDto
          {
            NameAndPosition = exec.NameAndPosition,
            Compensation = exec.Salary,
            AverageIndustryCompensation = industryAverages[exec.IndustryTitle]
          })
          .ToList();

      _cache.Set(cacheKey, result, CacheDuration);

      return result;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error fetching executives with compensation above average: {Message}", ex.Message);
      throw;
    }
  }

  private async Task<List<GetAllCompaniesResponse>> GetCompaniesAsync()
  {
    string cacheKey = $"{CacheKeyPrefix}Companies";
    if (!_cache.TryGetValue(cacheKey, out List<GetAllCompaniesResponse> companies))
    {
      companies = await _fnTechTestClient.GetAllCompaniesAsync();
      _cache.Set(cacheKey, companies, CacheDuration);
    }
    return companies;
  }

  private async Task<List<GetAllExecutivesDto>> GetAllExecutivesAsync(List<GetAllCompaniesResponse> companies)
  {
    var companySymbols = companies.Select(c => c.Symbol).Distinct().ToList();
    var allExecutives = new List<GetAllExecutivesDto>();

    foreach (var symbol in companySymbols)
    {
      string cacheKey = $"{CacheKeyPrefix}Executives_{symbol}";
      if (!_cache.TryGetValue(cacheKey, out List<GetAllExecutivesDto> executives))
      {
        executives = await _fnTechTestClient.GetAllExecutivesAsync(symbol);
        _cache.Set(cacheKey, executives, CacheDuration);
      }
      allExecutives.AddRange(executives);
    }

    return allExecutives;
  }

  private async Task<Dictionary<string, decimal>> GetIndustryAveragesAsync(List<GetAllExecutivesDto> executives)
  {
    var industries = executives.Select(e => e.IndustryTitle).Distinct().ToList();
    var industryAverages = new Dictionary<string, decimal>();
    int count = 0;

    foreach (var industry in industries)
    {
      string cacheKey = $"{CacheKeyPrefix}IndustryAvg_{industry}";
      if (!_cache.TryGetValue(cacheKey, out GetAverageCompensationDto averageCompensation))
      {
        averageCompensation = await _fnTechTestClient.GetAverageCompensationAsync(industry);
        if (averageCompensation != null)
        {
          _cache.Set(cacheKey, averageCompensation, CacheDuration);
        }

        if (count == 30)
        {
          await Task.Delay(200);
          count = 0;
        }
        count++;
      }

      if (averageCompensation != null)
      {
        industryAverages[averageCompensation.IndustryTitle] = averageCompensation.AverageCompensation;
      }
    }

    return industryAverages;
  }
}
