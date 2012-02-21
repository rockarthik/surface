using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApuntaNotas.Business;
using ApuntaNotas.ViewModel;
using Microsoft.Practices.Unity;

namespace ApuntaNotas
{
    /// <summary>
    /// Here the DI magic come on.
    /// </summary>
    public class Bootstrapper
    {
        public IUnityContainer Container { get; set; }

        public Bootstrapper()
        {
            Container = new UnityContainer();

            ConfigureContainer();
        }

        /// <summary>
        /// We register here every service / interface / viewmodel.
        /// </summary>
        private void ConfigureContainer()
        {
            Container.RegisterInstance<INoteRepository>(new NoteRepository("notes"));
            Container.RegisterInstance<ICategoryRepository>(new CategoryRepository("categories"));
            Container.RegisterType<MainViewModel>();
            Container.RegisterType<CategoryEditorViewModel>();
        }
    }
}
