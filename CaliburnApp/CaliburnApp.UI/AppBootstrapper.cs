using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CaliburnApp.DataAccess;
using CaliburnApp.UI.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace CaliburnApp.UI
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        private readonly IServiceLocator container;

        public AppBootstrapper()
        {
            this.container = new Container().Build();
            Database.SetInitializer(new DbInitializer());
            AutoMapperConfiguration.Configure();
        }

        public IServiceLocator Container
        {
            get { return this.container; }
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.Container.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrEmpty(key)
                       ? this.Container.GetInstance(service)
                       : this.Container.GetInstance(service, key);
        }
    }
}
