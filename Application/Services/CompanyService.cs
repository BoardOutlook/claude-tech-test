using System;
using Application.Dtos;
using Application.Interfaces;

namespace Application.Services;

public class CompanyService(IFnTechTestClient fnTechTestClient) : ICompanyService
{
  public async Task<IEnumerable<CompensationDto>> GetExecutivesWithCompensationAboveAverage()
  {
    var companies = await fnTechTestClient.GetAllCompanies();
    var companySymbolSet = new HashSet<string>();
    companies.ForEach(company => companySymbolSet.Add(company.Symbol));

    var executives = new List<GetAllExecutivesDto>();
    foreach (var companySymbol in companySymbolSet)
    {
      var companyExecutives = await fnTechTestClient.GetAllExecutives(companySymbol);
      executives.AddRange(companyExecutives);
    }

    var industrySet = new HashSet<string>();
    executives.ForEach(exec => industrySet.Add(exec.IndustryTitle));

    var industryAverageCompensation = new Dictionary<string, decimal>();
    foreach (var industry in industrySet)
    {
      var averageCompensation = await fnTechTestClient.GetAverageCompensation(industry);
      industryAverageCompensation.Add(industry, averageCompensation.AverageCompensation);
    }

    var executivesWithCompensationAboveAverage = new List<CompensationDto>();
    foreach (var exec in executives)
    {
      var averageCompensation = industryAverageCompensation[exec.IndustryTitle];
      var tenPercentAboveAverage = averageCompensation * 1.1m;
      if (exec.Salary > averageCompensation)
      {
        executivesWithCompensationAboveAverage.Add(new CompensationDto
        {
          NameAndPosition = exec.NameAndPosition,
          Compensation = exec.Salary,
          AverageIndustryCompensation = averageCompensation
        });
      }
    }

    return executivesWithCompensationAboveAverage;
  }
}
