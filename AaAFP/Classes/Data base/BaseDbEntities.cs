using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    public class BaseDbEntities : DbEntities
    {
        public BaseDbEntities() : base() 
        {

        }

        public virtual void AddEntity(object entity)
        {
            DbSet dbSet = Set(entity.GetType());
            if (dbSet != null) 
            {
                dbSet.Add(entity);
            }
        }

        public virtual void RemoveEntity(object entity) 
        {
            DbSet dbSet = Set(entity.GetType());
            if (dbSet != null) 
            {
                dbSet.Remove(entity);
            }
        }

        public virtual void Load() 
        {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                object obj = p.GetValue(this);
                if (obj is IQueryable dbSet)
                {
                    dbSet.Load();
                }
            }
        }

        public virtual void UndoUnsavedChanges() 
        {
            var entries = ChangeTracker.Entries();
            foreach (var e in entries)
            {
                switch (e.State)
                {
                    case EntityState.Added:
                    case EntityState.Detached:
                        RemoveEntity(e.Entity);
                        break;
                    case EntityState.Deleted:
                        e.State = EntityState.Added;
                        e.State = EntityState.Unchanged;
                        break;
                    case EntityState.Modified:
                        e.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.State = EntityState.Deleted;
                    }
                    entry.Reload();
                }
                throw ex;
            }
            catch (DbEntityValidationException ex)
            {
                var entries = ex.EntityValidationErrors.Select(e => e.Entry);
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.State = EntityState.Deleted;
                    }
                    entry.Reload();
                }
                throw ex;
            }
        }
    }
}
