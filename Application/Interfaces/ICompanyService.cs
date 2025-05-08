using System;
using Application.Dtos;

namespace Application.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompensationDto>> GetExecutivesWithCompensationAboveAverage();
}
