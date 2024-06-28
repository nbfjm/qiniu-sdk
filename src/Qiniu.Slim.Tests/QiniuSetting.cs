namespace Qiniu.Slim.Tests;

public class QiniuSetting
{
	/// <summary>
	/// AccessKeyId
	/// </summary>
	public required string AccessKeyId { get; set; }

	/// <summary>
	/// AccessKeySecret
	/// </summary>
	public required string AccessKeySecret { get; set; }

	/// <summary>
	/// 存储空间名称
	/// </summary>
	public required string Bucket { get; set; }

	/// <summary>
	/// 空间域名
	/// </summary>
	public required string Domain { get; set; }
}
