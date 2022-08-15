using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class SettingsViewModel : DbEntityViewModel
    {
        public SettingsViewModel() 
        {
            var enterpriseInfo = DbEntities.EnterprisesInfo.Local.FirstOrDefault();
            CurrentDbEntity = enterpriseInfo;
        }
    }
}
