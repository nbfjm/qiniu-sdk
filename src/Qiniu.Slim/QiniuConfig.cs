namespace Qiniu.Slim;

public class QiniuConfig
{
	/// <summary>
	/// 是否采用https域名
	/// </summary>
	public bool UseHttps { get; set; }

	/// <summary>
	/// 是否采用CDN加速域名，对上传有效
	/// </summary>
	public bool UseCdnDomains { get; set; }

	/// <summary>
	/// 空间所在的区域(Zone)
	/// </summary>
	public required Zone Zone { get; set; }

	public required Mac Mac { get; set; }

	/// <summary>
	/// 获取文件上传域名
	/// </summary>
	public string UpHost
	{
		get
		{
			string scheme = UseHttps ? "https://" : "http://";
			string upHost = Zone.SrcUpHosts[0];
			if (UseCdnDomains)
			{
				upHost = Zone.CdnUpHosts[0];
			}

			return string.Concat(scheme, upHost);
		}
	}

	/// <summary>
	/// 获取资源管理域名
	/// </summary>
	public string RsHost
	{
		get
		{
			string scheme = UseHttps ? "https://" : "http://";
			return string.Concat(scheme, Zone.RsHost);
		}
	}
}