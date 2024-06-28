using System;
using System.Security.Cryptography;
using System.Text;

namespace Qiniu.Slim;

public static class Auth
{
	const string MANAGE_TOKEN_HEADER = "POST {0}\nHost: {1}\nContent-Type: application/x-www-form-urlencoded\n\n";

	/// <summary>
	/// 生成上传凭证
	/// </summary>
	/// <param name="mac">账号(密钥)</param>
	/// <param name="putPolicy">上传策略</param>
	/// <returns>生成的上传凭证</returns>
	public static string CreateUploadToken(Mac mac, PutPolicy putPolicy)
	{
		if (putPolicy.Deadline == 0)
		{
			putPolicy.SetExpires(3600);//默认一个小时有效期
		}

		var data = Json.SerializeToUtf8Bytes(putPolicy);
		string body = Base64Encode(data);

		return $"{mac.AccessKey}:{EncodedSign(mac.SecretKey, body)}:{body}";
	}

	/// <summary>
	/// 生成 Qiniu 管理凭证
	/// </summary>
	/// <param name="mac">账号(密钥)</param>
	/// <param name="url">请求的 URL</param>
	/// <param name="body">请求的主体数据</param>
	/// <returns>生成的管理凭证</returns>
	public static string CreateManageToken(Mac mac, string url, byte[]? body = default)
	{
		Uri parsedUrl = new(url);
		string headers = string.Concat(parsedUrl.PathAndQuery, "\n");
		byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
		int bodyLength = body?.Length ?? 0;

		byte[] buffer = new byte[headerBytes.Length + bodyLength];

		Buffer.BlockCopy(headerBytes, 0, buffer, 0, headerBytes.Length);
		if (bodyLength > 0 && body != null)
		{
			Buffer.BlockCopy(body, 0, buffer, headerBytes.Length, bodyLength);
		}

		string digestBase64 = EncodedSign(mac.SecretKey, buffer);
		return $"QBox {mac.AccessKey}:{digestBase64}";
	}

	/// <summary>
	/// 生成 Qiniu 管理凭证 V2
	/// </summary>
	/// <param name="mac">账号(密钥)</param>
	/// <param name="url">请求的 URL</param>
	/// <param name="body">请求的主体数据</param>
	/// <remarks>
	/// 官网地址 <see href="https://developer.qiniu.com/kodo/1201/access-token" />
	/// </remarks>
	/// <returns></returns>
	public static string CreateManageTokenV2(Mac mac, string url, byte[]? body = default)
	{
		Uri parsedUrl = new(url);

		string headers = string.Format(MANAGE_TOKEN_HEADER, parsedUrl.PathAndQuery, parsedUrl.Host);
		byte[] headerBytes = Encoding.UTF8.GetBytes(headers);
		int bodyLength = body?.Length ?? 0;

		byte[] buffer = new byte[headerBytes.Length + bodyLength];

		Buffer.BlockCopy(headerBytes, 0, buffer, 0, headerBytes.Length);

		if (bodyLength > 0 && body != null)
		{
			Buffer.BlockCopy(body, 0, buffer, headerBytes.Length, bodyLength);
		}

		string digestBase64 = EncodedSign(mac.SecretKey, buffer);
		return $"Qiniu {mac.AccessKey}:{digestBase64}";
	}

	static string EncodedSign(string secretKey, string source)
		=> EncodedSign(secretKey, Encoding.UTF8.GetBytes(source));


	static string EncodedSign(string secretKey, byte[] source)
	{
		var key = Encoding.UTF8.GetBytes(secretKey);
		var digest = HMACSHA1.HashData(key, source);
		return Base64Encode(digest);
	}

	static string Base64Encode(byte[] data)
		=> Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_');
}
