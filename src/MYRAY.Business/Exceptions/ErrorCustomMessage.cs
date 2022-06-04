namespace MYRAY.Business.Exceptions;
/// <summary>
/// exception message class.
/// </summary>
public class ErrorCustomMessage
{
    /// <summary>
    /// Gets or sets custom error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets things meet error.
    /// </summary>
    public object Target { get; set; }
}