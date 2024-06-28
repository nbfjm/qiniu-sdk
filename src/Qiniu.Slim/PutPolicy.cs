using System;
using System.Text.Json.Serialization;

namespace Qiniu.Slim;

/// <summary>
/// 上传策略
/// </summary>
/// <param name="bucket">存储空间</param>
/// <remarks>
/// 参考文档 <see href="https://developer.qiniu.com/kodo/1206/put-policy" />
/// </remarks>
public class PutPolicy
{
	/// <summary>
	/// [必需]bucket或者bucket:key
	/// </summary>
	[JsonPropertyName("scope")]
	public required string Scope { get; set; }

	/// <summary>
	/// [可选]若为 1，表示允许用户上传以 scope 的 keyPrefix 为前缀的文件。
	/// </summary>
	[JsonPropertyName("isPrefixalScope")]
	public int? IsPrefixalScope { get; set; }

	/// <summary>
	/// [必需]上传策略失效时刻，请使用SetExpire来设置它
	/// </summary>
	[JsonPropertyName("deadline")]
	public long Deadline { get; private set; }

	/// <summary>
	/// 限定为新增语意。如果设置为非 0 值，则无论 scope 设置为什么形式，仅能以新增模式上传文件。
	/// </summary>
	[JsonPropertyName("insertOnly")]
	public int? InsertOnly { get; set; }

	/// <summary>
	/// [可选]saveKey 的优先级设置。为 true 时，saveKey不能为空，会忽略客户端指定的key，强制使用saveKey进行文件命名。
	/// 默认为 false
	/// </summary>
	[JsonPropertyName("forceSaveKey")]
	public bool? ForceSaveKey { get; set; }

	/// <summary>
	/// 保存文件的key
	/// </summary>
	[JsonPropertyName("saveKey")]
	public required string SaveKey { get; set; }

	/// <summary>
	/// [可选]终端用户
	/// </summary>
	[JsonPropertyName("endUser")]
	public string? EndUser { get; set; }

	/// <summary>
	/// [可选]返回URL
	/// </summary>
	[JsonPropertyName("returnUrl")]
	public string? ReturnUrl { get; set; }

	/// <summary>
	/// [可选]返回内容
	/// </summary>
	[JsonPropertyName("returnBody")]
	public string? ReturnBody { get; set; }

	/// <summary>
	/// [可选]回调URL
	/// </summary>
	[JsonPropertyName("callbackUrl")]
	public string? CallbackUrl { get; set; }

	/// <summary>
	/// [可选]回调内容
	/// </summary>
	[JsonPropertyName("callbackBody")]
	public string? CallbackBody { get; set; }

	/// <summary>
	/// [可选]回调内容类型
	/// </summary>
	[JsonPropertyName("callbackBodyType")]
	public string? CallbackBodyType { get; set; }

	/// <summary>
	/// [可选]回调host
	/// </summary>
	[JsonPropertyName("callbackHost")]
	public string? CallbackHost { get; set; }

	/// <summary>
	/// [可选]回调fetchkey
	/// </summary>
	[JsonPropertyName("callbackFetchKey")]
	public int? CallbackFetchKey { get; set; }

	/// <summary>
	/// [可选]上传预转持久化
	/// </summary>
	[JsonPropertyName("persistentOps")]
	public string? PersistentOps { get; set; }

	/// <summary>
	/// [可选]持久化结果通知
	/// </summary>
	[JsonPropertyName("persistentNotifyUrl")]
	public string? PersistentNotifyUrl { get; set; }

	/// <summary>
	/// [可选]私有队列
	/// </summary>
	[JsonPropertyName("persistentPipeline")]
	public string? PersistentPipeline { get; set; }

	/// <summary>
	/// [可选]上传文件大小限制：最小值，单位Byte
	/// </summary>
	[JsonPropertyName("fsizeMin")]
	public long? FsizeMin { get; set; }

	/// <summary>
	/// [可选]上传文件大小限制：最大值，单位Byte
	/// </summary>
	[JsonPropertyName("fsizeLimit")]
	public long? FsizeLimit { get; set; }

	/// <summary>
	/// [可选]上传时是否自动检测MIME
	/// </summary>
	[JsonPropertyName("detectMime")]
	public int? DetectMime { get; set; }

	/// <summary>
	/// [可选]上传文件MIME限制
	/// </summary>
	[JsonPropertyName("mimeLimit")]
	public string? MimeLimit { get; set; }

	/// <summary>
	/// [可选]文件上传后多少天后自动删除
	/// </summary>
	[JsonPropertyName("deleteAfterDays")]
	public int? DeleteAfterDays { get; set; }

	/// <summary>
	/// [可选]文件的存储类型，默认为普通存储，设置为：0 标准存储（默认），1 低频存储，2 归档存储，3 深度归档存储
	/// </summary>
	[JsonPropertyName("fileType")]
	public int? FileType { get; set; }

	/// <summary>
	/// 设置上传凭证有效期（配置Deadline属性）
	/// </summary>
	/// <param name="expireInSeconds"></param>
	public void SetExpires(int expireInSeconds)
	{
		Deadline = DateTimeOffset.Now.AddSeconds(expireInSeconds).ToUnixTimeSeconds();
	}
}
