namespace Application.Dtos;

public record GetAllExecutivesDto
{
  public string IndustryTitle { get; set; }
  public string NameAndPosition { get; set; }
  public decimal Salary { get; set; }
}
