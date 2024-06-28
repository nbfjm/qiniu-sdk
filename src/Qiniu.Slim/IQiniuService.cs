using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Qiniu.Slim;

public interface IQiniuService
{
	Task<HttpResult> UploadDataAsync(
		byte[] data,
		PutPolicy putPolicy,
		Dictionary<string, string>? extraParams = null);

	Task<HttpResult> UploadStreamAsync(
		Stream stream,
		PutPolicy putPolicy,
		Dictionary<string, string>? extraParams = null);

	Task<HttpResult> SendAsync(
		Func<QiniuConfig, HttpRequestMessage> setupRequest, 
		[CallerMemberName] string? callerMemberName = default);
}
