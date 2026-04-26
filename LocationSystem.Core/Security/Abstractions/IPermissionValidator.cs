namespace LocationSystem.Core.Security.Abstractions
{
    public interface IPermissionValidator
    {
        Task<PermissionValidationResult> ValidateAsync(PermissionValidationContext context);
    }
}
