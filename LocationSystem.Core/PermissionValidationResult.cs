namespace LocationSystem.Core
{
    /// <summary>
    /// 权限验证结果
    /// </summary>
    public class PermissionValidationResult
    {
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 验证失败的消息
        /// </summary>
        public string FailureMessage { get; set; }

        /// <summary>
        /// 验证失败的原因代码
        /// </summary>
        public PermissionValidationFailureCode FailureCode { get; set; }

        /// <summary>
        /// 用户拥有的权限列表 (用于调试)
        /// </summary>
        public List<string> UserPermissions { get; set; } = new List<string>();

        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 验证耗时 (毫秒)
        /// </summary>
        public long ValidationDurationMs { get; set; }

        /// <summary>
        /// 创建成功的验证结果
        /// </summary>
        public static PermissionValidationResult Success(bool isSuperAdmin = false)
        {
            return new PermissionValidationResult
            {
                IsAuthorized = true,
                IsSuperAdmin = isSuperAdmin,
                FailureMessage = string.Empty,
                FailureCode = PermissionValidationFailureCode.None
            };
        }

        /// <summary>
        /// 创建失败的验证结果
        /// </summary>
        public static PermissionValidationResult Failure(
            string message,
            PermissionValidationFailureCode code = PermissionValidationFailureCode.PermissionDenied)
        {
            return new PermissionValidationResult
            {
                IsAuthorized = false,
                IsSuperAdmin = false,
                FailureMessage = message,
                FailureCode = code
            };
        }
    }


}
