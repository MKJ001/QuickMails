namespace Common.Exceptions
{
    public class AppNotFoundException : AppBaseException
    {
        public AppNotFoundException(string message, AppExceptionCodes code = AppExceptionCodes.Unknown)
            : base(message, code)
        {
        }
    }
}
