namespace TEL_ProjectBus.BLL.Exceptions;

/// <summary>
/// Базовый «транспорт» для ошибок из Consumers в HTTP-мир.
/// </summary>
public class HttpResponseException : Exception
{
	public int StatusCode { get; }
	public object? Value { get; }

	public HttpResponseException(int statusCode, string message, object? value = null)
		: base(message)
	{
		StatusCode = statusCode;
		Value = value;
	}
}
