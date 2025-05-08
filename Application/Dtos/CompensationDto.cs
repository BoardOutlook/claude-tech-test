namespace Application.Dtos;

public record class CompensationDto
{
  public string NameAndPosition { get; set; }
  public decimal Compensation { get; set; }
  public decimal AverageIndustryCompensation { get; set; }
}
