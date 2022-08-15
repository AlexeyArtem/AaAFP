using MySql.Data.MySqlClient;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    public abstract class BaseModel
    {
        protected static BaseDbEntities dbEntities = App.DbEntities;
        protected static IKernel ninjectKernel = App.NinjectKernel;

        public BaseModel()
        {

        }
    }
}
