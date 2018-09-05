using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Spreetail.Budgeter.ViewModels.Home;
namespace Spreetail.Budgeter.Controllers {
    public class HomeController : _BaseController {

        
        public ActionResult Index(string name, string formAction) {
            var model = new HomeViewModel() {
                Name = name,
                ErrorMessage = new List<string>()
            };

            if (formAction != null) {
                if (formAction.Equals(HomeViewModel.FormActions.CreateNew, StringComparison.OrdinalIgnoreCase)) {
                    //save new budget
                    var budget = new Models.Budget() { Name = name };
                    var errors = new List<string>();
                    budget = Factory.BudgetManager.Save(budget, ref errors);
                    model.ErrorMessage = errors;

                    if (model.ErrorMessage.Count == 0) {                        
                        return RedirectToBudget(budget.BudgetID);
                    }

                } else if (formAction.Equals(HomeViewModel.FormActions.GetExisting, StringComparison.OrdinalIgnoreCase)) {
                    //redirect to existing budget
                    var budget = Factory.BudgetManager.Search(name);
                    if (budget != null) {
                        return RedirectToBudget(budget.BudgetID);
                    } else {
                        model.ErrorMessage.Add($"Budget '{name}' was not found");
                    }

                } else {
                    throw new HttpException(400, "bad request");
                }
            }

            return View(model);
        }

        private ActionResult RedirectToBudget(int budgetID) {
            return RedirectToAction("index", "budget", new { ID = budgetID });
        }


    }
}