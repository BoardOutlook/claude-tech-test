using System;
using Application.Dtos;

namespace Application.Interfaces;

public interface IFnTechTestClient
{
  Task<List<GetAllCompaniesResponse>> GetAllCompaniesAsync();
  Task<List<GetAllExecutivesDto>> GetAllExecutivesAsync(string companySymbol);
  Task<GetAverageCompensationDto?> GetAverageCompensationAsync(string industryTitle);
}
