using System;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CompanyService(IFnTechTestClient fnTechTestClient, ILogger<CompanyService> logger) : ICompanyService
{
  public async Task<IEnumerable<CompensationDto>> GetExecutivesWithCompensationAboveAverage()
  {
    try
    {
      var companies = await fnTechTestClient.GetAllCompaniesAsync();
      var companySymbols = companies.Select(c => c.Symbol).Distinct().ToList();

      var executivesTasks = companySymbols.Select(symbol => fnTechTestClient.GetAllExecutivesAsync(symbol));
      var executivesLists = await Task.WhenAll(executivesTasks);
      var executives = executivesLists.SelectMany(list => list).ToList();

      var industries = executives.Select(e => e.IndustryTitle).Distinct().ToList();
      var compensationTasks = industries.Select(industry =>
          fnTechTestClient.GetAverageCompensationAsync(industry));
      var compensationResults = await Task.WhenAll(compensationTasks);

      var industryAverages = compensationResults
          .ToDictionary(r => r.IndustryTitle, r => r.AverageCompensation);

      return executives
          .Where(exec => exec.Salary > industryAverages[exec.IndustryTitle])
          .Select(exec => new CompensationDto
          {
            NameAndPosition = exec.NameAndPosition,
            Compensation = exec.Salary,
            AverageIndustryCompensation = industryAverages[exec.IndustryTitle]
          })
          .ToList();
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error fetching executives with compensation above average: {Message}", ex.Message);
      throw;
    }
  }
}
