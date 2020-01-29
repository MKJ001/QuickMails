namespace Common.Exceptions
{
    public class AppBadRequestException : AppBaseException
    {
        public AppBadRequestException(string message, AppExceptionCodes code = AppExceptionCodes.Unknown)
            : base(message, code)
        {
        }
    }
}
