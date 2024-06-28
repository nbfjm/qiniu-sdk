using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Qiniu.Slim.Tests;

public class Startup
{
	public static void ConfigureHost(IHostBuilder hostBuilder) =>
	   hostBuilder.ConfigureAppConfiguration(lb => lb.AddJsonFile("appsettings.json", false, true));

	public void ConfigureServices(IServiceCollection services, HostBuilderContext hostBuilderContext)
	{
		var configuration = hostBuilderContext.Configuration.GetSection(nameof(QiniuSetting));
		services.Configure<QiniuSetting>(configuration);
		services.AddQiniuService(config =>
		{
			config.Mac = new Mac(configuration["AccessKeyId"]!, configuration["AccessKeySecret"]!);
			config.Zone = Zone.ZONE_CN_South;
			config.UseHttps = true;
			config.UseCdnDomains = true;
		});
	}
}
