﻿using QueflityMVC.Domain.Common;

namespace QueflityMVC.Infrastructure.Common
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected Context _dbContext;

        public BaseRepository(Context dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual int Add(T entityToAdd)
        {
            if (entityToAdd == null)
                throw new ArgumentNullException("Entity cannot be null");
            
            _dbContext.Set<T>().Add(entityToAdd);
            _dbContext.SaveChanges();

            return entityToAdd.Id;
        }

        public virtual void Delete(int entityToDeleteId)
        {
            var entityToDelete = GetById(entityToDeleteId);

            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (!Exists(entityToDelete))
                throw new ArgumentException("Entity does not exist!");

            _dbContext.Set<T>().Remove(entityToDelete); 
            _dbContext.SaveChanges();
        }

        public virtual T Update(T entityToUpdate)
        {
            if (!Exists(entityToUpdate))
                throw new ArgumentException("Entity does not exist!");

            _dbContext.Set<T>().Update(entityToUpdate);
            _dbContext.SaveChanges();

            return GetById(entityToUpdate.Id)!;
        }

        public virtual bool Exists(T entityToCheck)
        {
            if (entityToCheck == null)
                throw new ArgumentNullException("Entity cannot be null");

            return Exists(entityToCheck.Id);
        }

        public virtual bool Exists(int entityId)
        {
            return GetById(entityId) != null;
        }

        public virtual T? GetById(int entityId)
        {
            return _dbContext.Set<T>().FirstOrDefault(ent => ent.Id == entityId);
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }
    }
}
