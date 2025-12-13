using LocationSystem.Application.Contrats.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure.UnitOfWork
{
    public class UnitOfWorkCore : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        public UnitOfWorkCore(AppDbContext context) 
        {
            _context = context; 
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();      // 提交事务
                await _transaction.DisposeAsync();     // 释放事务资源
                _transaction = null;                   // 重置事务状态
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();    // 回滚事务
                await _transaction.DisposeAsync();     // 释放事务资源
                _transaction = null;                   // 重置事务状态
            }
        }
    }
}
