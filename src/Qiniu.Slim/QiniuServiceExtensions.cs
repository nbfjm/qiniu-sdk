using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Qiniu.Slim;

public static class QiniuServiceExtensions
{
	public static Task<HttpResult> DeleteAsync(this IQiniuService service, string bucket, string key)
	{
		var op = Buckets.DeleteOp(bucket, key);
		return service.RsPostAsync(op);
	}

	public static Task<HttpResult> DeleteAfterDaysAsync(this IQiniuService service,
		string bucket,
		string key,
		int deleteAfterDays)
	{
		var op = Buckets.DeleteAfterDaysOp(bucket, key, deleteAfterDays);
		return service.RsPostAsync(op);
	}

	public static Task<HttpResult> BatchDeleteAsync(this IQiniuService service, string bucket, string[] keys)
	{
		var ops = keys.Select(key => $"op={Buckets.DeleteOp(bucket, key)}").ToArray();
		return service.BatchAsync(string.Join("&", ops));
	}

	public static Task<HttpResult> BatchDeleteAfterDaysAsync(this IQiniuService service,
		string bucket,
		string[] keys,
		int deleteAfterDays)
	{
		var ops = keys.Select(key => $"op={Buckets.DeleteAfterDaysOp(bucket, key, deleteAfterDays)}").ToArray();
		return service.BatchAsync(string.Join("&", ops));
	}

	public static Task<HttpResult> CopyAsync(this IQiniuService service,
		string srcBucket, string srcKey,
		string dstBucket, string dstKey,
		bool force = false)
	{
		var op = Buckets.CopyOp(srcBucket, srcKey, dstBucket, dstKey, force);
		return service.RsPostAsync(op);
	}

	public static Task<HttpResult> BatchCopyAsync(this IQiniuService service,
		string srcBucket, string[] srcKeys,
		string dstBucket, string[] dstKeys,
		bool force = false)
	{
		if (srcKeys.Length != dstKeys.Length)
			throw new ArgumentException("源Key必须与目标Key数量一致");

		var ops = srcKeys.Select((key, i) => $"op={Buckets.CopyOp(srcBucket, key, dstBucket, dstKeys[i], force)}").ToArray();
		return service.BatchAsync(string.Join("&", ops));
	}


	public static Task<HttpResult> MoveAsync(this IQiniuService service,
		string srcBucket, string srcKey,
		string dstBucket, string dstKey,
		bool force = false)
	{
		var op = Buckets.MoveOp(srcBucket, srcKey, dstBucket, dstKey, force);
		return service.RsPostAsync(op);
	}

	public static Task<HttpResult> BatchMoveAsync(this IQiniuService service,
		string srcBucket, string[] srcKeys,
		string dstBucket, string[] dstKeys,
		bool force = false)
	{
		if (srcKeys.Length != dstKeys.Length)
			throw new ArgumentException("源Key必须与目标Key数量一致");

		var ops = srcKeys.Select((key, i) => $"op={Buckets.MoveOp(srcBucket, key, dstBucket, dstKeys[i], force)}").ToArray();
		return service.BatchAsync(string.Join("&", ops));
	}

	public static Task<HttpResult> ChangeStatusAsync(this IQiniuService service, string bucket, string key, int status)
	{
		var op = Buckets.ChangeStatusOp(bucket, key, status);
		return service.RsPostAsync(op);
	}

	public static Task<HttpResult> BatchAsync(this IQiniuService service, string batchOps,
		[CallerMemberName] string? callerMemberName = default)
		=> service.SendAsync(config =>
		{
			string url = $"{config.RsHost}/batch";
			var body = Encoding.UTF8.GetBytes(batchOps);

			string token = Auth.CreateManageTokenV2(config.Mac, url, body);

			HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, url);
			httpRequestMessage.Headers.Add("Authorization", token);

			httpRequestMessage.Content = new ByteArrayContent(body);
			httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
			return httpRequestMessage;
		}, callerMemberName);

	public static Task<HttpResult> RsPostAsync(this IQiniuService service, string op,
		[CallerMemberName] string? callerMemberName = default)
		=> service.SendAsync(config =>
		{
			string url = string.Concat(config.RsHost, op);
			string token = Auth.CreateManageTokenV2(config.Mac, url);

			HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, url);
			httpRequestMessage.Headers.Add("Authorization", token);

			return httpRequestMessage;
		}, callerMemberName);

	/// <summary>
	/// 缓存刷新
	/// </summary>
	/// <param name="service"></param>
	/// <param name="mac"></param>
	/// <param name="urls"></param>
	/// <param name="dirs"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	/// <remarks>
	/// <see href="https://developer.qiniu.com/dcdn/10755/dcdn-cache-refresh-with-the-query"/>
	/// </remarks>
	public static Task<HttpResult> CdnRefreshAsync(this IQiniuService service,
		string[]? urls = default,
		string[]? dirs = default)
	{
		if (urls == null && dirs == null)
			throw new ArgumentException("urls与dirs必须设置其中一个");

		var body = Json.SerializeToUtf8Bytes(new
		{
			urls,
			dirs,
			product = "dcdn"
		});

		return service.SendAsync(config =>
		{
			string url = "https://fusion.qiniuapi.com/v2/tune/refresh";
			string token = Auth.CreateManageToken(config.Mac, url);

			HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, url);
			httpRequestMessage.Headers.Add("Authorization", token);
			httpRequestMessage.Content = new ByteArrayContent(body);
			httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return httpRequestMessage;
		});
	}
}
