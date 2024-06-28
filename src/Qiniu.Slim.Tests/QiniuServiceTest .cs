using Microsoft.Extensions.Options;

namespace Qiniu.Slim.Tests;

public class QiniuServiceTest(
	IQiniuService qiniuService,
	IOptions<QiniuSetting> options)
{
	private const int FILE_SIZE = 10 * 1024;
	private readonly QiniuSetting _setting = options.Value;

	[Fact]
	public async Task TestUploadDataAsync()
	{
		string key = $"{nameof(TestUploadDataAsync)}_{Random.Shared.Next()}.dat";

		PutPolicy putPolicy = new()
		{
			Scope = _setting.Bucket,
			SaveKey = key
		};

		byte[] data = new byte[FILE_SIZE];

		var result = await qiniuService.UploadDataAsync(data, putPolicy);

		Assert.Equal(200, result.Code);

		await qiniuService.DeleteAsync(_setting.Bucket, key);
	}

	[Fact]
	public async Task TestUploadStreamAsync()
	{
		string key = $"{nameof(TestUploadStreamAsync)}_{Random.Shared.Next()}.dat";

		PutPolicy putPolicy = new()
		{
			Scope = _setting.Bucket,
			SaveKey = key
		};

		byte[] data = new byte[FILE_SIZE];
		using var ms = new MemoryStream(data);

		var result = await qiniuService.UploadStreamAsync(ms, putPolicy);

		Assert.Equal(200, result.Code);

		await qiniuService.DeleteAsync(_setting.Bucket, key);
	}
}