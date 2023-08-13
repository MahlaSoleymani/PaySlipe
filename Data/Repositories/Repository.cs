﻿using System.Data.Common;
using System.Linq.Expressions;
using Common.Utilities;
using Database.Contexts;
using Database.Contracts;
using Entities.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly ApplicationDbContext DbContext;
        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();
        public DbConnection DbConnection => DbContext.Database.GetDbConnection();

        public ApplicationDbContext AppDbContext => DbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>(); // City => Cities
        }

        #region Async Method
        public virtual ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return Entities.FindAsync(ids, cancellationToken);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entity, nameof(entity));
            await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            if (saveNow)
            {

                DbContext.IgnorePreSaveChange = ignorePreSaveChange;

                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entities, nameof(entities));
            await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            if (saveNow)
            {

                DbContext.IgnorePreSaveChange = ignorePreSaveChange;

                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Update(entity);
            if (saveNow)
            {

                DbContext.IgnorePreSaveChange = ignorePreSaveChange;

                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.UpdateRange(entities);
            if (saveNow)
            {

                DbContext.IgnorePreSaveChange = ignorePreSaveChange;

                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
            {

                DbContext.IgnorePreSaveChange = ignorePreSaveChange;

                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
            {

                DbContext.IgnorePreSaveChange = ignorePreSaveChange;

                await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        #endregion

        #region Sync Methods
        public virtual TEntity Find(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public virtual TEntity FindAsNoTracking(params object[] ids)
        {
            var res = Entities.Find(ids);
            if (res != null) Detach(res);
            return res;
        }

        public virtual IQueryable<T> RunQuery<T>(string sql, params object[] parameters) where T : class
        {
            return DbContext.Set<T>().FromSqlRaw(sql, parameters);
        }

        public virtual IQueryable<TEntity> RunQuery(string sql, params object[] parameters)
        {
            return Entities.FromSqlRaw(sql, parameters);
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

        public virtual void Add(TEntity entity, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Add(entity);
            if (saveNow)
            {
                DbContext.IgnorePreSaveChange = ignorePreSaveChange;
                DbContext.SaveChanges();
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.AddRange(entities);
            if (saveNow)
            {
                DbContext.IgnorePreSaveChange = ignorePreSaveChange;
                DbContext.SaveChanges();
            }
        }

        public virtual void Update(TEntity entity, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Update(entity);
            if (saveNow)
            {
                DbContext.IgnorePreSaveChange = ignorePreSaveChange;
                DbContext.SaveChanges();
            }

        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.UpdateRange(entities);
            if (saveNow)
            {
                DbContext.IgnorePreSaveChange = ignorePreSaveChange;
                DbContext.SaveChanges();
            }
        }

        public virtual void Delete(TEntity entity, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
            {
                DbContext.IgnorePreSaveChange = ignorePreSaveChange;
                DbContext.SaveChanges();
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = false, bool ignorePreSaveChange = false)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
            {
                DbContext.IgnorePreSaveChange = ignorePreSaveChange;
                DbContext.SaveChanges();
            }
        }
        #endregion

        #region Attach & Detach
        public virtual void Detach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            var entry = DbContext.Entry(entity);
            entry.State = EntityState.Detached;
        }

        public virtual void Attach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            if (DbContext.Entry(entity).State == EntityState.Detached)
                Entities.Attach(entity);
        }
        #endregion

        #region Explicit Loading
        public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);

            var collection = DbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class
        {
            Attach(entity);
            var collection = DbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                collection.Load();
        }

        public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
            where TProperty : class
        {
            Attach(entity);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                reference.Load();
        }
        #endregion
    }
}