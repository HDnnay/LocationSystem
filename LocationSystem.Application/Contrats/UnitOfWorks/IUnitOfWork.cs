using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Application.Contrats.UnitOfWorks
{
    public interface IUnitOfWork
    {

        Task Commit();
        Task Rollback();
    }
}
