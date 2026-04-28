namespace LocationSystem.Core.Security.Abstractions
{
    public interface IPermissionProvider
    {
        Task<List<string>> GetUserPermissionsAsync(Guid userId);
        Task<bool> IsSuperAdminAsync(Guid userId);
    }
}
