using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddAInfrastructure(this IServiceCollection services)
  {
    services.AddScoped<IFnTechTestClient, FnTechTestClient>();
    return services;
  }
}

