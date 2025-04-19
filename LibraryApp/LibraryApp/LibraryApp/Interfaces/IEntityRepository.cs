using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Interfaces
{
    internal interface IEntityRepository<TEntity, DtoEntity> 
        where TEntity : class
        where DtoEntity : class
    {
        IQueryable<DtoEntity> GetItems();
        void AddItem(TEntity item);
        DtoEntity GetItemdById(int id);
        void RemoveItem(TEntity item);
        void UpdateItem(int id, TEntity item);
    }
}

