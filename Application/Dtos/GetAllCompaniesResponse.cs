namespace Application.Dtos;

public record class GetAllCompaniesResponse
{
  public string Sector { get; set; }
  public string Country { get; set; }
  public string FullTimeEmployees { get; set; }
  public string Symbol { get; set; }
  public string Name { get; set; }
  public decimal? Price { get; set; }
  public string Exchange { get; set; }
  public string ExchangeShortName { get; set; }
  public string Type { get; set; }
}
