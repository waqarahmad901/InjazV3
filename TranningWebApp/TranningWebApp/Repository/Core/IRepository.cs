using System;
using System.Collections.Generic;

namespace TranningWebApp.Repository

{
    public interface IRepository<TEntity, in TPrimaryKey> // : IDisposable 
        where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        void Post(TEntity entity);
        void Put(TPrimaryKey id, TEntity entity);
        void Delete(TPrimaryKey id);
    }
}
