using System;
using System.Text;

namespace Qiniu.Slim;

public static class Buckets
{
	/// <summary>
	/// 生成delete操作字符串
	/// </summary>
	/// <param name="bucket">空间名称</param>
	/// <param name="key">文件key</param>
	/// <returns>delete操作字符串</returns>
	public static string DeleteOp(string bucket, string key)
		=> $"/delete/{UrlSafeBase64Encode(bucket, key)}";

	/// <summary>
	/// 修改文件过期删除时间
	/// </summary>
	/// <param name="bucket">空间名称</param>
	/// <param name="key">文件key</param>
	/// <param name="deleteAfterDays">几天后，设置为 0 表示取消过期删除设置</param>
	/// <returns></returns>
	public static string DeleteAfterDaysOp(string bucket, string key, int deleteAfterDays)
		=> $"/deleteAfterDays/{UrlSafeBase64Encode(bucket, key)}/{deleteAfterDays}";

	/// <summary>
	/// 资源移动/重命名
	/// </summary>
	/// <param name="srcBucket">源空间</param>
	/// <param name="srcKey">源KEY</param>
	/// <param name="dstBucket">目标空间</param>
	/// <param name="dstKey">目标KEY</param>
	/// <param name="force">文件已存在时是否覆盖</param>
	/// <returns></returns>
	/// <remarks>
	/// <see href="https://developer.qiniu.com/kodo/1288/move"/>
	/// </remarks>
	public static string MoveOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force = false)
	{
		string fx = force ? "force/true" : "force/false";
		return $"/move/{UrlSafeBase64Encode(srcBucket, srcKey)}/{UrlSafeBase64Encode(dstBucket, dstKey)}/{fx}";
	}

	/// <summary>
	/// 资源复制
	/// </summary>
	/// <param name="srcBucket">源空间</param>
	/// <param name="srcKey">源KEY</param>
	/// <param name="dstBucket">目标空间</param>
	/// <param name="dstKey">目标KEY</param>
	/// <param name="force">文件已存在时是否覆盖</param>
	/// <returns></returns>
	/// <remarks>
	/// <see href="https://developer.qiniu.com/kodo/1254/copy"/>
	/// </remarks>
	public static string CopyOp(string srcBucket, string srcKey, string dstBucket, string dstKey, bool force = false)
	{
		string fx = force ? "force/true" : "force/false";
		return $"/copy/{UrlSafeBase64Encode(srcBucket, srcKey)}/{UrlSafeBase64Encode(dstBucket, dstKey)}/{fx}";
	}

	/// <summary>
	/// 修改文件状态
	/// </summary>
	/// <param name="bucket">空间名称</param>
	/// <param name="key">文件key</param>
	/// <param name="status">状态值，0表示启用，1表示禁用</param>
	/// <returns></returns>
	/// <remarks>
	/// <see href="https://developer.qiniu.com/kodo/4173/modify-the-file-status"/>
	/// </remarks>
	public static string ChangeStatusOp(string bucket, string key, int status)
		=> $"/chstatus/{UrlSafeBase64Encode(bucket, key)}/status/{status}";

	public static string UrlSafeBase64Encode(string bucket, string key)
	{
		var data = Encoding.UTF8.GetBytes($"{bucket}:{key}");
		return Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_');
	}
}
