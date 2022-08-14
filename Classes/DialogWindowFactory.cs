using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AaAFP2
{
    class DialogWindowFactory : IDialogWindowFactory
    {
        readonly IResolutionRoot resolutionRoot;

        public DialogWindowFactory(IResolutionRoot resolutionRoot) 
        {
            this.resolutionRoot = resolutionRoot;
        }

        public Window Create(string key)
        {
            bool isResolve = resolutionRoot.CanResolve<Window>(key);
            if (isResolve)
                return resolutionRoot.Get<Window>(key);
            else
                return new NotFoundWindow();
        }
    }
}
