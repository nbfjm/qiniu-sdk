using Microsoft.Extensions.DependencyInjection;
using System;

namespace Qiniu.Slim;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQiniuService(this IServiceCollection services,
        Action<QiniuConfig> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddHttpClient();
        services.AddSingleton<IQiniuService, QiniuService>();
        return services;
    }
}
