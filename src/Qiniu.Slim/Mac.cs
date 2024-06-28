namespace Qiniu.Slim;

/// <summary>
/// 账户访问控制(密钥)
/// </summary>
/// <param name="AccessKey">密钥-AccessKey</param>
/// <param name="SecretKey">密钥-SecretKey</param>
public record Mac(string AccessKey, string SecretKey);
