using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

using Spreetail.Budgeter.Service;
using Spreetail.Budgeter.Data;

namespace Spreetail.Budgeter
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IBudgetService, BudgetService>();
            container.RegisterType<IBudgetRepository, BudgetRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<IReoccuringItemRepository, ReoccuringItemRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}