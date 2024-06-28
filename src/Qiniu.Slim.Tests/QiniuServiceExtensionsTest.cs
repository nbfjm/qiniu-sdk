using Microsoft.Extensions.Options;

namespace Qiniu.Slim.Tests;

public class QiniuServiceExtensionsTest(
	IQiniuService qiniuService,
	IOptions<QiniuSetting> options)
{
	private const int FILE_SIZE = 10 * 1024;
	private readonly QiniuSetting _setting = options.Value;

	[Fact]
	public async Task TestDeleteAsync()
	{
		string key = $"{nameof(TestDeleteAsync)}_{Random.Shared.Next()}.dat";

		PutPolicy putPolicy = new()
		{
			Scope = _setting.Bucket,
			SaveKey = key
		};

		byte[] data = new byte[FILE_SIZE];
		await qiniuService.UploadDataAsync(data, putPolicy);

		var result = await qiniuService.DeleteAsync(_setting.Bucket, key);

		Assert.Equal(200, result.Code);
	}

	[Fact]
	public async Task TestDeleteAfterDaysAsync()
	{
		string key = $"{nameof(TestDeleteAfterDaysAsync)}_{Random.Shared.Next()}.dat";

		PutPolicy putPolicy = new()
		{
			Scope = _setting.Bucket,
			SaveKey = key
		};

		byte[] data = new byte[FILE_SIZE];
		using var ms = new MemoryStream(data);

		await qiniuService.UploadStreamAsync(ms, putPolicy);

		var result = await qiniuService.DeleteAfterDaysAsync(_setting.Bucket, key, 3);
		Assert.Equal(200, result.Code);

		await qiniuService.DeleteAsync(_setting.Bucket, key);
	}

	[Fact]
	public async Task TestCopyAsync()
	{
		string key = $"{nameof(TestCopyAsync)}_{Random.Shared.Next()}.dat";

		PutPolicy putPolicy = new()
		{
			Scope = _setting.Bucket,
			SaveKey = key
		};

		byte[] data = new byte[FILE_SIZE];
		using var ms = new MemoryStream(data);

		await qiniuService.UploadStreamAsync(ms, putPolicy);

		string dstKey = $"{nameof(TestCopyAsync)}_copy.dat";
		var result = await qiniuService.CopyAsync(_setting.Bucket, key, _setting.Bucket, dstKey);
		Assert.Equal(200, result.Code);

		await qiniuService.DeleteAsync(_setting.Bucket, key);
		await qiniuService.DeleteAsync(_setting.Bucket, dstKey);
	}

	[Fact]
	public async Task TestCdnRefreshAsync()
	{
		string key = $"{nameof(TestCdnRefreshAsync)}_{Random.Shared.Next()}.dat";

		PutPolicy putPolicy = new()
		{
			Scope = _setting.Bucket,
			SaveKey = key
		};

		byte[] data = new byte[FILE_SIZE];
		using var ms = new MemoryStream(data);

		await qiniuService.UploadStreamAsync(ms, putPolicy);

		var result = await qiniuService.CdnRefreshAsync(urls: [$"{_setting.Domain}/{key}"]);
		Assert.Equal(200, result.Code);

		await qiniuService.DeleteAsync(_setting.Bucket, key);
	}
}