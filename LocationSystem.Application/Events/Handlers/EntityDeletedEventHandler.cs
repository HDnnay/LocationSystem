using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using LocationSystem.Application.Events;
using LocationSystem.Domain.Entities.DeletedSnapshots;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Application.Events.Handlers
{
    public class EntityDeletedEventHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EntityDeletedEventHandler> _logger;

        public EntityDeletedEventHandler(IServiceScopeFactory scopeFactory, ILogger<EntityDeletedEventHandler> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Handle(EntityDeletedEvent @event)
        {
            using var scope = _scopeFactory.CreateScope();
            var snapshotRepository = scope.ServiceProvider.GetRequiredService<IDeletedSnapshotRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                var snapshot = new DeletedSnapshot
                {
                    EntityType = @event.EntityType,
                    AssemblyQualifiedTypeName = @event.EntityType,
                    EntityId = @event.EntityId.ToString(),
                    SnapshotDataJson = @event.EntityJson,
                    DeletedAt = @event.DeletedAt,
                    DeletedBy = @event.DeletedBy,
                    DeleteReason = @event.DeleteReason
                };

                await unitOfWork.BeginTransactionAsync();
                await snapshotRepository.AddAsync(snapshot);
                await unitOfWork.CommitAsync();

                _logger.LogInformation("实体删除快照创建成功: {EntityType}, {EntityId}", @event.EntityType, @event.EntityId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "创建删除快照失败: {EntityType}, {EntityId}", @event.EntityType, @event.EntityId);
                await unitOfWork.RollbackAsync();
            }
        }
    }
}