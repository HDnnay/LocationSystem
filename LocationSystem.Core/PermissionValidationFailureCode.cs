namespace LocationSystem.Core
{
    /// <summary>
    /// 权限验证失败代码
    /// </summary>
    public enum PermissionValidationFailureCode
    {
        /// <summary>
        /// 验证成功
        /// </summary>
        None = 0,

        // ===== 认证相关错误 (1xx) =====

        /// <summary>
        /// 用户未认证
        /// </summary>
        Unauthenticated = 101,

        /// <summary>
        /// Token 过期
        /// </summary>
        TokenExpired = 102,

        /// <summary>
        /// Token 无效
        /// </summary>
        TokenInvalid = 103,

        /// <summary>
        /// 用户被禁用
        /// </summary>
        UserDisabled = 104,

        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotFound = 105,

        // ===== 权限相关错误 (2xx) =====

        /// <summary>
        /// 权限不足 (通用)
        /// </summary>
        PermissionDenied = 201,

        /// <summary>
        /// 权限代码不存在
        /// </summary>
        PermissionCodeNotFound = 202,

        /// <summary>
        /// 权限代码格式错误
        /// </summary>
        PermissionCodeInvalid = 203,

        /// <summary>
        /// 角色权限不足
        /// </summary>
        RolePermissionDenied = 204,

        /// <summary>
        /// 用户权限不足
        /// </summary>
        UserPermissionDenied = 205,

        // ===== 数据级权限错误 (3xx) =====

        /// <summary>
        /// 数据级权限验证失败
        /// </summary>
        DataLevelPermissionDenied = 301,

        /// <summary>
        /// 资源不存在
        /// </summary>
        ResourceNotFound = 302,

        /// <summary>
        /// 资源类型不匹配
        /// </summary>
        ResourceTypeMismatch = 303,

        /// <summary>
        /// 资源所有者不匹配
        /// </summary>
        ResourceOwnerMismatch = 304,

        /// <summary>
        /// 数据访问范围限制
        /// </summary>
        DataAccessScopeLimited = 305,

        // ===== 操作相关错误 (4xx) =====

        /// <summary>
        /// 操作类型不支持
        /// </summary>
        OperationNotSupported = 401,

        /// <summary>
        /// 操作被禁止
        /// </summary>
        OperationForbidden = 402,

        /// <summary>
        /// 操作频率限制
        /// </summary>
        OperationRateLimited = 403,

        /// <summary>
        /// 操作时间限制
        /// </summary>
        OperationTimeRestricted = 404,

        // ===== 系统相关错误 (5xx) =====

        /// <summary>
        /// 缓存服务不可用
        /// </summary>
        CacheServiceUnavailable = 501,

        /// <summary>
        /// 权限服务异常
        /// </summary>
        PermissionServiceError = 502,

        /// <summary>
        /// 数据库连接失败
        /// </summary>
        DatabaseConnectionFailed = 503,

        /// <summary>
        /// 配置错误
        /// </summary>
        ConfigurationError = 504,

        /// <summary>
        /// 系统内部错误
        /// </summary>
        InternalServerError = 505,

        // ===== 业务逻辑错误 (6xx) =====

        /// <summary>
        /// 租户权限限制
        /// </summary>
        TenantPermissionRestricted = 601,

        /// <summary>
        /// 组织权限限制
        /// </summary>
        OrganizationPermissionRestricted = 602,

        /// <summary>
        /// 部门权限限制
        /// </summary>
        DepartmentPermissionRestricted = 603,

        /// <summary>
        /// 项目权限限制
        /// </summary>
        ProjectPermissionRestricted = 604,

        /// <summary>
        /// 工作空间权限限制
        /// </summary>
        WorkspacePermissionRestricted = 605
    }
}