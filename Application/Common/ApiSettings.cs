using System;

namespace Application.Common;

public class ApiSettings
{
  public string BaseUrl { get; set; }
  public string CompaniesKey { get; set; }
  public string ApiKey { get; set; }

  public int CacheDurationInMinutes { get; set; }
}
