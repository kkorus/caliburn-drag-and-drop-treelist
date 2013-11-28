using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CaliburnApp.DataAccess;
using CaliburnApp.Domain;
using CaliburnApp.Domain.Entities;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Unity.AutoRegistration;

namespace CaliburnApp.UI
{
    class Container
    {
        /// <summary>
        /// Builds this instance.
        /// http://commonservicelocator.codeplex.com/ 
        /// http://sondreb.com/blog/post/unity-as-ioc-container-for-caliburn-micro.aspx.
        /// </summary>
        /// <returns>IServiceLocator implementation.</returns>
        public IServiceLocator Build()
        {
            IUnityContainer unity = new UnityContainer();

            RegisterWorkspaceViewModels(unity);

            unity.RegisterType<IWindowManager, WindowManager>();

            EventAgrregatorRegistration(unity);

            ServicesRegistration(unity);

            // Return an type which implements the Common Service Locator interface.
            UnityServiceLocator commonLocator = new UnityServiceLocator(unity);
            return commonLocator;
        }

        /// <summary>
        /// Register repositories and data context.
        /// </summary>
        /// <param name="unity">The unity.</param>
        private static void ServicesRegistration(IUnityContainer unity)
        {
            unity.RegisterType<DbContext, DatabaseContext>();
            unity.RegisterType<IRepository<DictionaryItem>, Repository<DictionaryItem>>();
        }

        /// <summary>
        /// Events the agrregator registration.
        /// </summary>
        /// <param name="unity">The unity.</param>
        private static void EventAgrregatorRegistration(IUnityContainer unity)
        {
            EventAggregator eventAggregator = unity.Resolve<EventAggregator>();
            unity.RegisterInstance<IEventAggregator>(eventAggregator, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Bults the workspace view model registration.
        /// </summary>
        /// <param name="unity">The unity.</param>
        private static void RegisterWorkspaceViewModels(IUnityContainer unity)
        {
            unity
                .ConfigureAutoRegistration()
                .Include(If.Implements<PropertyChangedBase>, Then.Register().WithTypeName())
                .ApplyAutoRegistration();
        }
    }
}
