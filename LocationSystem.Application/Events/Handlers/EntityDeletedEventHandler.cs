using LocationSystem.Application.Contrats.Repositories;
using LocationSystem.Application.Contrats.UnitOfWorks;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Application.Events.Handlers
{
    public class EntityDeletedEventHandler
    {
        private readonly ILogger<EntityDeletedEventHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeletedSnapshotRepository _repository;
        public EntityDeletedEventHandler(IDeletedSnapshotRepository repository, IUnitOfWork unitOfWork, ILogger<EntityDeletedEventHandler> logger)
        {
            _logger = logger;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(EntityDeletedEvent @event)
        {

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                // 直接创建快照，不需要反序列化实体
                var snapshot = new Domain.Entities.DeletedSnapshots.DeletedSnapshot
                {
                    EntityType = @event.EntityType,
                    AssemblyQualifiedTypeName = @event.AssemblyQualifiedTypeName,
                    EntityId = @event.EntityId.ToString(),
                    SnapshotDataJson = @event.EntityJson,
                    DeletedAt = @event.DeletedAt,
                    DeletedBy = @event.DeletedBy,
                    DeleteReason = @event.DeleteReason
                };

                await _repository.AddAsync(snapshot);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("实体删除快照创建成功: {EntityType}, {EntityId}", @event.EntityType, @event.EntityId);
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(e, "创建删除快照失败: {EntityType}, {EntityId}", @event.EntityType, @event.EntityId);
            }
        }
    }
}