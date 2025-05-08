using System;
using Application.Dtos;

namespace Application.Interfaces;

public interface IFnTechTestClient
{
  Task<List<GetAllCompaniesResponse>> GetAllCompanies();
  Task<List<GetAllExecutivesDto>> GetAllExecutives(string companySymbol);
  Task<GetAverageCompensationDto> GetAverageCompensation(string industryTitle);
}
