using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Spreetail.Budgeter.Service;
using Spreetail.Budgeter.Model;

namespace Spreetail.Budgeter.Controllers
{
    public class BudgetController : _BaseController
    {
        private readonly IBudgetService BudgetService;

        public BudgetController(IBudgetService BudgetService)
        {
            this.BudgetService = BudgetService;
        }
               

        public ActionResult Index(int ID, int? month, int? year)
        {
            var model = new ViewModels.Budget.BudgetViewModel();

            var budget = BudgetService.GetBudget(ID);

            model = AutoMapper.Mapper.Map<ViewModels.Budget.BudgetViewModel>(budget);

            if (month == null || year == null || month < 1 || month > 12 || year < 1900 || year > 9999)
            {
                model.BudgetDate = DateTime.Today;
            }
            else
            {
                model.BudgetDate = new DateTime((int)year, (int)month, 1);
            }

            var categories = BudgetService.GetCategoriesByBudgetID(ID);

            if (categories?.Count > 0)
            {
                model.Items = new List<ViewModels.Budget.ItemsByCategory>();
                foreach (var category in categories)
                {
                    var itemsModel = AutoMapper.Mapper.Map<ViewModels.Budget.ItemsByCategory>(category);
                    itemsModel.Items = BudgetService.GetItemsByCatAndMonth(category.CategoryID, model.BudgetDate.Month, model.BudgetDate.Year);

                    model.Items.Add(itemsModel);
                }
            }

            return View(model);
        }


        public ActionResult PurchaseSummary(int ID, DateTime startDate, DateTime endDate)
        {
            var model = new ViewModels.Budget.PurchaseSummary
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var budget = BudgetService.GetBudget(ID);

            model.BudgetID = ID;
            model.Name = budget.Name;

            model.Items = BudgetService.GetItemsByRange(budget.BudgetID, startDate, endDate);
            model.CategoryNames = BudgetService.GetCategoriesByBudgetID(budget.BudgetID).
                                                ToDictionary(x => x.CategoryID, x => x.CategoryName);

            return View(model);
        }



        //AJAX calls
        [HttpPost]
        public JsonResult SaveCategory(Category category)
        {
            var errors = new List<string>();
            int id = 0;

            if (ModelState.IsValid)
            {
                id = BudgetService.SaveCategory(category);   
            }
            else
            {
                errors = ModelStateErrors();
            }

            return Json(new { Errors = errors, CategoryID = id });
        }

        [HttpPost]
        public JsonResult SaveItem(Item item)
        {
            var errors = new List<string>();

            if (ModelState.IsValid)
            {
                BudgetService.SaveItem(item);
            }
            else
            {
                errors = ModelStateErrors();
            }

            return Json(new { Errors = errors });
        }

        [HttpPost]
        public void DeleteItem(int id)
        {
            BudgetService.DeleteItem(id);
        }

        [HttpPost]
        public void DeleteReoccuringItem(int id)
        {
            BudgetService.DeleteReoccuringItem(id);
        }

        [HttpPost]
        public JsonResult SaveReoccuringItem(ReoccuringItem reoccuringItem)
        {
            var errors = new List<string>();

            if (ModelState.IsValid)
            {
                BudgetService.SaveAndGenerateReoccuring(reoccuringItem);
            }
            else
            {
                errors = ModelStateErrors();
            }
            

            return Json(new { Errors = errors });
        }

        [HttpPost]
        public JsonResult GetReoccuringItem(int id)
        {
            var reoccuring = BudgetService.GetReoccuringItem(id);
            return Json(reoccuring);
        }

        [HttpPost]
        public ActionResult GetCategorySpendingHTML(int id, DateTime date)
        {
            ViewModels.Budget.ItemsByCategory model = null;

            var category = BudgetService.GetCategory(id);

            if (category != null)
            {
                model = AutoMapper.Mapper.Map<ViewModels.Budget.ItemsByCategory>(category);

                model.Items = BudgetService.GetItemsByCatAndMonth(category.CategoryID, date.Month, date.Year);

            }

            return PartialView("_CategorySpending", model);
        }

        [HttpPost]
        public JsonResult GetAllCategories(int budgetID)
        {
            var categories = BudgetService.GetCategoriesByBudgetID(budgetID);

            return Json(categories);
        }



    }
}