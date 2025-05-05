namespace TEL_ProjectBus.WebAPI.Common;

public class ApiResponse<T>
{
	public string openapi { get; set; } = "3.1.0";
	//[JsonPropertyName("$schema")]
	public string schema { get; set; } = "http://json-schema.org/draft-07/schema#";
	public string version { get; set; } = "1.0.0";
	public T data { get; set; }

	public ApiResponse(T data)
	{
		this.data = data;
	}
}

