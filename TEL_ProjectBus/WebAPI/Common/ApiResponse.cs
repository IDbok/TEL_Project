namespace TEL_ProjectBus.WebAPI.Common;

public record ApiResponse<T>
{
	public string openapi { get; init; } = "3.1.0";
	//[JsonPropertyName("$schema")]
	public string schema { get; init; } = "http://json-schema.org/draft-07/schema#";
	public string version { get; init; } = "1.0.0";
	public T data { get; init; }

	public ApiResponse(T data)
	{
		this.data = data;
	}
}

