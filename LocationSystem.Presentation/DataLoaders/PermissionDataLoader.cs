using LocationSystem.Application.GrapqLDTOs.Permissons;

namespace LocationSystem.Presentation.DataLoaders
{
    public class PermissionDataLoader : BatchDataLoader<Guid, PermissionGraphqLDto>
    {
        public PermissionDataLoader(IBatchScheduler batchScheduler, DataLoaderOptions options) : base(batchScheduler, options)
        {
        }

        protected override Task<IReadOnlyDictionary<Guid, PermissionGraphqLDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
