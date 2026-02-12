namespace LocationSystem.Application.Exceptions
{
    public class RefreshTokenException : ApplicationCustomException
    {
        public RefreshTokenException(string message)
            : base(message, 401)
        {
        }
    }

    public class RefreshTokenExpiredException : RefreshTokenException
    {
        public RefreshTokenExpiredException()
            : base("刷新令牌已过期，请重新登录")
        {
        }
    }

    public class InvalidRefreshTokenException : RefreshTokenException
    {
        public InvalidRefreshTokenException()
            : base("无效的刷新令牌")
        {
        }
    }
}