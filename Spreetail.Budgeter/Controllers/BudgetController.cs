using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Spreetail.Budgeter.Controllers {
    public class BudgetController : _BaseController {


        public ActionResult Index(int ID, int? month, int? year) {
            var model = new ViewModels.Budget.BudgetViewModel();

            var budget = Factory.BudgetManager.Get(ID);
            model = AutoMapper.Mapper.Map<ViewModels.Budget.BudgetViewModel>(budget);

            if (month == null || year == null || month < 1 || month > 12 || year < 1900 || year > 9999) {
                model.BudgetDate = DateTime.Today;
            } else {
                model.BudgetDate = new DateTime((int)year, (int)month, 1);
            }

            var categories = Factory.CategoryManager.GetByBudgetID(ID);

            if (categories?.Count > 0) {
                model.Items = new List<ViewModels.Budget.ItemsByCategory>();
                foreach (var category in categories) {
                    var itemsModel = AutoMapper.Mapper.Map<ViewModels.Budget.ItemsByCategory>(category);

                    itemsModel.Items = Factory.ItemManager.GetByCategoryID(category.CategoryID);
                        itemsModel.Items = itemsModel.Items.
                                                      Where(x => x.PurchaseDate.Year == model.BudgetDate.Year && x.PurchaseDate.Month == model.BudgetDate.Month).
                                                      OrderBy(x => x.PurchaseDate).
                                                      ToList();
                    
                    model.Items.Add(itemsModel);
                }
            }

            //model.UncategorizedItems = new List<ViewModels.Budget.ItemsByCategory>();

            return View(model);
        }
                

        public ActionResult PurchaseSummary(int ID, DateTime startDate, DateTime endDate) {
            var model = new ViewModels.Budget.PurchaseSummary();

            model.StartDate = startDate;
            model.EndDate = endDate;

            var budget = Factory.BudgetManager.Get(ID);
            model.BudgetID = ID;
            model.Name = budget.Name;

            model.Items = Factory.ItemManager.GetByBudgetID(ID).Where(x => x.PurchaseDate >= startDate && x.PurchaseDate <= endDate).
                                                                OrderBy(x=> x.PurchaseDate).
                                                                ToList();
            model.CategoryNames = Factory.CategoryManager.GetByBudgetID(ID).ToDictionary(x => x.CategoryID, x => x.CategoryName);

            return View(model);
        }



        //WebAPI calls
        [HttpPost]
        public JsonResult SaveCategory(Models.Category category) {
            var errors = new List<string>();
            category = Factory.CategoryManager.Save(category, ref errors);
            return Json(new { Errors = errors, CategoryID = category.CategoryID });
        }

        [HttpPost]
        public JsonResult SaveItem(Models.Item item) {

            var errors = new List<string>();
            item = Factory.ItemManager.Save(item, ref errors);

            return Json(new { Errors = errors });
        }

        [HttpPost]
        public JsonResult DeleteItem(int itemID, int reoccuringID) {
            if (reoccuringID > 0) {
                var reoccuringItems = Factory.ItemManager.GetByReoccuringID(reoccuringID);
                foreach (var item in reoccuringItems) {
                    Factory.ItemManager.Delete(item);
                }

                var reoccuring = new Models.ReoccuringItem() { ReoccuringItemID = reoccuringID };
                Factory.ReoccuringItemManager.Delete(reoccuring);
            } else {
                var item = new Models.Item() { ItemID = itemID };
                Factory.ItemManager.Delete(item);
            }

            return Json(new { Message = "ok" });
        }

        [HttpPost]
        public JsonResult SaveReoccuringItem(Models.ReoccuringItem reoccuringItem) {

            var errors = new List<string>();
            reoccuringItem = Factory.ReoccuringItemManager.Save(reoccuringItem, ref errors);

            if (errors.Count == 0) {
                //delete any existing records and regenerate
                var existingItems = Factory.ItemManager.GetByReoccuringID(reoccuringItem.ReoccuringItemID);
                if (existingItems.Count > 0) {
                    foreach (var delItem in existingItems) {
                        Factory.ItemManager.Delete(delItem);
                    }
                }

                //generate new items
                var currentDate = reoccuringItem.StartDate;
                while (currentDate < reoccuringItem.EndDate) {
                    //add to db
                    var item = AutoMapper.Mapper.Map<Models.Item>(reoccuringItem);
                    item.PurchaseDate = currentDate;

                    item = Factory.ItemManager.Save(item, ref errors);
                    if (reoccuringItem.ReoccuringUnit == Models.ReoccuringItem.eReoccuringUnit.Month) {
                        currentDate = currentDate.AddMonths(reoccuringItem.ReoccuringRate);
                    } else {
                        currentDate = currentDate.AddDays(reoccuringItem.ReoccuringRate * 7);
                    }
                }
            }

            return Json(new { Errors = errors });
        }

        [HttpPost]
        public JsonResult GetReoccuringItem(int id) {
            var reoccuring = Factory.ReoccuringItemManager.Get(id);
            return Json(reoccuring);
        }


        public ActionResult GetCategorySpendingHTML(int id, DateTime date) {
            ViewModels.Budget.ItemsByCategory model = null;

            var category = Factory.CategoryManager.Get(id);

            if (category != null) {
                model = AutoMapper.Mapper.Map<ViewModels.Budget.ItemsByCategory>(category);

                model.Items = Factory.ItemManager.GetByCategoryID(category.CategoryID).
                                                  Where(x=> x.PurchaseDate.Month == date.Month && x.PurchaseDate.Year == date.Year).
                                                  OrderBy(x=> x.PurchaseDate).
                                                  ToList();                
            }

            return PartialView("_CategorySpending", model);

        }

        [HttpPost]
        public JsonResult GetAllCategories(int budgetID) {
            var categories = Factory.CategoryManager.GetByBudgetID(budgetID);

            return Json(categories);
        }



    }
}