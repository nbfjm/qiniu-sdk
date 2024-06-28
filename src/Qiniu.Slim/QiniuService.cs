using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Qiniu.Slim;

public class QiniuService(IHttpClientFactory factory, IOptions<QiniuConfig> config) : IQiniuService
{
	const string QINIU_KEY_NAME = "key";
	const string QINIU_TOKE_NNAME = "token";
	const string QINIU_FILE_NAME = "file";

	private readonly QiniuConfig _config = config.Value;

	public async Task<HttpResult> UploadDataAsync(
		byte[] data,
		PutPolicy putPolicy,
		Dictionary<string, string>? extraParams = null)
	{
		using HttpClient client = factory.CreateClient();
		try
		{
			var content = CreateFormDataContent(_config.Mac, putPolicy, extraParams);
			content.Add(new ByteArrayContent(data), QINIU_FILE_NAME, putPolicy.SaveKey);

			var response = await client.PostAsync(_config.UpHost, content);
			var result = await ProcessResponseAsync(response);

			return result;
		}
		catch (Exception ex)
		{
			HttpResult ret = HttpResult.InvalidFile;
			ret.RefText = ex.Message;
			return ret;
		}
	}

	public async Task<HttpResult> UploadStreamAsync(
		Stream stream,
		PutPolicy putPolicy,
		Dictionary<string, string>? extraParams = null)
	{
		using HttpClient client = factory.CreateClient();
		try
		{
			var content = CreateFormDataContent(_config.Mac, putPolicy, extraParams);

			stream.Position = 0; //重置position，便于StreamContent读取流内容
			content.Add(new StreamContent(stream), QINIU_FILE_NAME, putPolicy.SaveKey);

			var response = await client.PostAsync(_config.UpHost, content);
			var result = await ProcessResponseAsync(response);

			return result;
		}
		catch (Exception ex)
		{
			HttpResult ret = HttpResult.InvalidFile;
			ret.RefText = ex.Message;
			return ret;
		}

	}

	public async Task<HttpResult> SendAsync(
		Func<QiniuConfig, HttpRequestMessage> setupRequest,
		[CallerMemberName] string? callerMemberName = default)
	{
		using HttpClient client = factory.CreateClient();
		try
		{
			HttpRequestMessage httpRequestMessage = setupRequest(_config);

			var response = await client.SendAsync(httpRequestMessage);
			var result = await ProcessResponseAsync(response);

			return result;
		}
		catch (Exception ex)
		{
			return new()
			{
				Code = (int)HttpCode.USER_UNDEF,
				RefCode = (int)HttpCode.USER_UNDEF,
				Text = $"{callerMemberName} error",
				RefText = ex.Message
			};
		}
	}

	private static MultipartFormDataContent CreateFormDataContent(
		Mac mac,
		PutPolicy putPolicy,
		Dictionary<string, string>? extraParams)
	{
		var uploadToken = Auth.CreateUploadToken(mac, putPolicy);
		var content = new MultipartFormDataContent
		{
			{ new StringContent(putPolicy.SaveKey), QINIU_KEY_NAME },
			{ new StringContent(uploadToken), QINIU_TOKE_NNAME }
		};

		if (extraParams is not null)
		{
			foreach (var (key, value) in extraParams)
			{
				if (string.IsNullOrEmpty(key)) continue;
				content.Add(new StringContent(value), key);
			}
		}

		return content;
	}

	private static async Task<HttpResult> ProcessResponseAsync(HttpResponseMessage response)
	{
		HttpResult result = new()
		{
			Code = (int)response.StatusCode,
			RefCode = (int)response.StatusCode
		};

		GetResponseHeaders(ref result, response);
		result.Text = await response.Content.ReadAsStringAsync();

		return result;
	}

	private static void GetResponseHeaders(ref HttpResult result, HttpResponseMessage? response)
	{
		if (response?.Headers is null) return;
		result.RefInfo ??= [];

		foreach (var (key, value) in response.Headers)
		{
			string values = string.Join("; ", value.Where(v => !string.IsNullOrEmpty(v)));
			if (!string.IsNullOrEmpty(values))
			{
				result.RefInfo.Add(key, values);
			}
		}
	}
}