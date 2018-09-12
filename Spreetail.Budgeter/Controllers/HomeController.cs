using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spreetail.Budgeter.Model;
using Spreetail.Budgeter.Service;
using Spreetail.Budgeter.ViewModels.Home;


namespace Spreetail.Budgeter.Controllers
{
    public class HomeController : _BaseController
    {
        private IBudgetService BudgetService;
        public HomeController(IBudgetService BudgetService)
        {
            this.BudgetService = BudgetService;
        }


        public ActionResult Index(string name, string formAction)
        {
            var model = new HomeViewModel()
            {
                Name = name,
                ErrorMessage = new List<string>()
            };

            if (formAction != null)
            {
                if (formAction.Equals(HomeViewModel.FormActions.CreateNew, StringComparison.OrdinalIgnoreCase))
                {
                    //save new budget
                    var errors = new List<string>();
                    var existing = BudgetService.GetBudgetByName(name);

                    if (existing == null)
                    {  var budget = new Budget() { Name = name };
                        BudgetService.AddBudget(budget);

                        return RedirectToBudget(budget.BudgetID);
                    }
                    else
                    {
                        model.ErrorMessage.Add($"Budget {name} already exists, please try again");
                    }

                }
                else if (formAction.Equals(HomeViewModel.FormActions.GetExisting, StringComparison.OrdinalIgnoreCase))
                {
                    //redirect to existing budget
                    var budget = BudgetService.GetBudgetByName(name);
                    if (budget != null)
                    {
                        return RedirectToBudget(budget.BudgetID);
                    }
                    else
                    {
                        model.ErrorMessage.Add($"Budget '{name}' was not found");
                    }

                }
                else
                {
                    throw new HttpException(400, "bad request");
                }
            }

            return View(model);
        }

        private ActionResult RedirectToBudget(int budgetID)
        {
            return RedirectToAction("index", "budget", new { ID = budgetID });
        }


    }
}