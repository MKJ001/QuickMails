namespace Common.Exceptions
{
    using System;

    public abstract class AppBaseException : Exception
    {
        public AppExceptionCodes Code { get; private set; }

        protected AppBaseException(string message, AppExceptionCodes code = AppExceptionCodes.Unknown)
            : base(message)
        {
            this.Code = code;
        }
    }
}
