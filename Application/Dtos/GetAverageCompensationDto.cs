namespace Application.Dtos;

public record class GetAverageCompensationDto
{
  public string IndustryTitle { get; set; }
  public int Year { get; set; }
  public decimal AverageCompensation { get; set; }
}
