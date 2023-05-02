namespace VeggiFoodAPI.Models.ViewModels
{
    public class ExceptionModel
    {
        public ExceptionModel(string message
            , string? data, string? innerException, string? stackTrace
            , string? controller, string? method)
        {
            Message = message;
            Data = data;
            InnerException = innerException;
            StackTrace = stackTrace;
            Controller = controller;
            Method = method;
        }

        public string Message { get; set; }
        public string? Data { get; set; }
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }
        public string? Controller { get; }
        public string? Method { get; }
    }
}
