// LocationSystem.Presentation/Extensions/GraphQLFieldExtensions.cs
namespace LocationSystem.Presentation.Extensions
{
    public static class GraphQLFieldExtensions
    {
        /// <summary>
        /// 为字段设置权限要求
        /// </summary>
        public static IObjectFieldDescriptor WithPermission(
            this IObjectFieldDescriptor descriptor,
            string permissionCode)
        {
            descriptor.Extend().OnBeforeCreate(d =>
           {
               d.ContextData["RequiredPermission"] = permissionCode;
           });
            return descriptor;
        }

        /// <summary>
        /// 为字段设置多个权限要求
        /// </summary>
        public static IObjectFieldDescriptor WithPermissions(
            this IObjectFieldDescriptor descriptor,
            params string[] permissionCodes)
        {
            descriptor.Extend().OnBeforeCreate(d =>
           {
               d.ContextData["RequiredPermissions"] = permissionCodes;
               d.ContextData["RequireAllPermissions"] = false; // 或 true
           });
            return descriptor;
        }
    }
}
