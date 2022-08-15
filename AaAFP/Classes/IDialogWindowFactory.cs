using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AaAFP2
{
    interface IDialogWindowFactory
    {
        Window Create(string key);
    }
}
