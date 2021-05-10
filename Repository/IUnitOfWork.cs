using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IUnitOfWork<TParameters> : IRepository<TParameters> where TParameters : class
    {
        int Save();
    }
}
