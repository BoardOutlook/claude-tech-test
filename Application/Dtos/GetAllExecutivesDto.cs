namespace Application.Dtos;

public record GetAllExecutivesDto
{
  // public string Cik { get; set; }
  // public string Symbol { get; set; }
  // public string CompanyName { get; set; }
  public string IndustryTitle { get; set; }
  // public string AcceptedDate { get; set; }
  // public string FilingDate { get; set; }
  public string NameAndPosition { get; set; }
  // public string Year { get; set; }
  public decimal Salary { get; set; }
  // public string Bonus { get; set; }
  // public string StockAward { get; set; }
  // public string IncentivePlanCompensation { get; set; }
  // public string AllOtherCompensation { get; set; }
  // public string Total { get; set; }
  // public string Url { get; set; }
}
