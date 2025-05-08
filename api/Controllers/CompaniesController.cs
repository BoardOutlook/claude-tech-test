using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace claude_tech_test.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet("executives/compensation")]
    [ProducesResponseType<IEnumerable<CompensationDto>>(StatusCodes.Status200OK)]
    public IActionResult GetExecutivesWithCompensationAboveAverage()
    {
        var executives = _companyService.GetExecutivesWithCompensationAboveAverage();
        return Ok(executives);
    }
}

