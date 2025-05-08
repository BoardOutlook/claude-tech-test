using System;
using Application.Dtos;
using Application.Interfaces;

namespace Infrastructure;

public class FnTechTestClient : IFnTechTestClient
{
  public Task<List<GetAllCompaniesResponse>> GetAllCompanies()
  {
    throw new NotImplementedException();
  }

  public Task<List<GetAllExecutivesDto>> GetAllExecutives(string companySymbol)
  {
    throw new NotImplementedException();
  }

  public Task<GetAverageCompensationDto> GetAverageCompensation(string industryTitle)
  {
    throw new NotImplementedException();
  }
}
