namespace Qiniu.Slim;

/// <summary>
/// 目前已支持的区域：华东/华东2/华北/华南/北美/新加坡/首尔
/// </summary>
public class Zone
{
    /// <summary>
    /// 资源管理
    /// </summary>
    public required string RsHost { set; get; }

    /// <summary>
    /// 源列表
    /// </summary>
    public required string RsfHost { set; get; }

    /// <summary>
    /// 数据处理
    /// </summary>
    public required string ApiHost { set; get; }

    /// <summary>
    /// 镜像刷新、资源抓取
    /// </summary>
    public required string IovipHost { set; get; }

    /// <summary>
    /// 资源上传
    /// </summary>
    public required string[] SrcUpHosts { set; get; }

    /// <summary>
    /// CDN加速
    /// </summary>
    public required string[] CdnUpHosts { set; get; }

    /// <summary>
    /// 华东
    /// </summary>
    public static readonly Zone ZONE_CN_East = new()
    {
        RsHost = "rs.qbox.me",
        RsfHost = "rsf.qbox.me",
        ApiHost = "api.qiniuapi.com",
        IovipHost = "iovip.qbox.me",
        SrcUpHosts = ["up.qiniup.com"],
        CdnUpHosts = ["upload.qiniup.com"]
    };

    /// <summary>
    /// 华东-浙江2
    /// </summary>
    public static readonly Zone ZONE_CN_East_2 = new()
    {
        RsHost = "rs-cn-east-2.qiniuapi.com",
        RsfHost = "rsf-cn-east-2.qiniuapi.com",
        ApiHost = "api-cn-east-2.qiniuapi.com",
        IovipHost = "iovip-cn-east-2.qiniuio.com",
        SrcUpHosts = ["up-cn-east-2.qiniup.com"],
        CdnUpHosts = ["upload-cn-east-2.qiniup.com"]
    };

    /// <summary>
    /// 华北
    /// </summary>
    public static readonly Zone ZONE_CN_North = new()
    {
        RsHost = "rs-z1.qbox.me",
        RsfHost = "rsf-z1.qbox.me",
        ApiHost = "api-z1.qiniuapi.com",
        IovipHost = "iovip-z1.qbox.me",
        SrcUpHosts = ["up-z1.qiniup.com"],
        CdnUpHosts = ["upload-z1.qiniup.com"]
    };

    /// <summary>
    /// 华南
    /// </summary>
    public static readonly Zone ZONE_CN_South = new()
    {
        RsHost = "rs-z2.qbox.me",
        RsfHost = "rsf-z2.qbox.me",
        ApiHost = "api-z2.qiniuapi.com",
        IovipHost = "iovip-z2.qbox.me",
        SrcUpHosts = ["up-z2.qiniup.com"],
        CdnUpHosts = ["upload-z2.qiniup.com"]
    };

    /// <summary>
    /// 北美
    /// </summary>
    public static readonly Zone ZONE_US_North = new()
    {
        RsHost = "rs-na0.qbox.me",
        RsfHost = "rsf-na0.qbox.me",
        ApiHost = "api-na0.qiniuapi.com",
        IovipHost = "iovip-na0.qbox.me",
        SrcUpHosts = ["up-na0.qiniup.com"],
        CdnUpHosts = ["upload-na0.qiniup.com"]
    };

    /// <summary>
    /// 新加坡
    /// </summary>
    public static readonly Zone ZONE_AS_Singapore = new()
    {
        RsHost = "rs-as0.qbox.me",
        RsfHost = "rsf-as0.qbox.me",
        ApiHost = "api-as0.qiniuapi.com",
        IovipHost = "iovip-as0.qbox.me",
        SrcUpHosts = ["up-as0.qiniup.com"],
        CdnUpHosts = ["upload-as0.qiniup.com"]
    };
}