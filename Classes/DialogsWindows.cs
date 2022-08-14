using System;
using System.Windows;

namespace AaAFP2
{
    class DialogsWindows
    {
        private IDialogWindowFactory windowFactory;

        public DialogsWindows(IDialogWindowFactory dialogWindowFactory)
        {
            windowFactory = dialogWindowFactory;
        }

        public void ShowEditDbEntityDialog(object dbEntity)
        {
            Type type = dbEntity.GetType().BaseType == typeof(object) ? dbEntity.GetType() : dbEntity.GetType().BaseType;
            string name = type.Name;
            
            Window window = windowFactory.Create(name);
            if (window.DataContext is DbEntityViewModel viewModel)
                viewModel.SetCurrentDbEntity(dbEntity);
            window.Title = "Изменить " + window.Title;

            window.ShowDialog();
        }

        public void ShowAddDbEntityDialog(Type typeEntity) 
        {
            string name = typeEntity.Name;
            Window window = windowFactory.Create(name);
            window.Title = "Добавить " + window.Title;
            window.ShowDialog();
        }

        public void ShowDialog(string name)
        {
            Window window = windowFactory.Create(name);
            window.ShowDialog();
        }
    }
}
