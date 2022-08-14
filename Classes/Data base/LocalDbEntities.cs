using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace AaAFP2
{
    class LocalDbEntities : BaseDbEntities
    {
        private void SetID(object entity) 
        {
            DbSet dbSet = Set(entity.GetType());
            DbEntityEntry entryEntity = Entry(entity);
            int maxId = -1;

            if (dbSet != null && dbSet.Local.Contains(entity))
            {
                foreach (var item in dbSet.Local)
                {
                    if (item == entity)
                        continue;

                    DbEntityEntry entry = Entry(item);
                    DbPropertyEntry propertyID = entry.Property("ID");
                    if (propertyID != null)
                    {
                        int id = (int)propertyID.CurrentValue;
                        if (id > maxId)
                        {
                            entryEntity.Property("ID").CurrentValue = id + 1;
                            maxId = id;
                        }
                    }
                }
            }
        }

        public override void AddEntity(object entity)
        {
            DbSet dbSet = Set(entity.GetType());
            if (dbSet != null) 
            {
                dbSet.Add(entity);
            }
        }

        public override void RemoveEntity(object entity)
        {
            DbSet dbSet = Set(entity.GetType());
            if(dbSet != null && dbSet.Local.Contains(entity))
                dbSet.Local.Remove(entity);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State) 
                {
                    case EntityState.Added:
                        SetID(entry.Entity);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        entry.State = EntityState.Added;
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
            return 0;
        }
    }
}
