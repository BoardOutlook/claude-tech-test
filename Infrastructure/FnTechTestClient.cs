using System;
using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class FnTechTestClient(IOptionsSnapshot<ApiSettings> apiSettings) : IFnTechTestClient
{
  public Task<List<GetAllCompaniesResponse>> GetAllCompaniesAsync()
  {
    throw new NotImplementedException();
  }

  public Task<List<GetAllExecutivesDto>> GetAllExecutivesAsync(string companySymbol)
  {
    throw new NotImplementedException();
  }

  public Task<GetAverageCompensationDto> GetAverageCompensationAsync(string industryTitle)
  {
    throw new NotImplementedException();
  }
}
