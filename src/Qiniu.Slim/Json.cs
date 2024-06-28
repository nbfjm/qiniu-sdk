using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Qiniu.Slim;

internal static class Json
{
	static readonly JsonSerializerOptions _ptions = new()
	{
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		PropertyNamingPolicy = null
	};

	public static byte[] SerializeToUtf8Bytes<T>(T data)
		=> JsonSerializer.SerializeToUtf8Bytes(data, _ptions);

	public static string Serialize<T>(T data)
		=> JsonSerializer.Serialize(data, _ptions);
}
