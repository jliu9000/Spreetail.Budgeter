using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AutoMapper;

namespace Spreetail.Budgeter {
    public class WebApiApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //automapper settings
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Models.Budget, ViewModels.Budget.BudgetViewModel>();
                cfg.CreateMap<Models.Category, ViewModels.Budget.ItemsByCategory>();
                cfg.CreateMap<Models.ReoccuringItem, Models.Item>()
                    .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.ReoccuringCost))
                        .ForMember(dest => dest.ReoccuringItemID, opt => opt.MapFrom(src => src.ReoccuringItemID));


            });
            


        }



    }
}
