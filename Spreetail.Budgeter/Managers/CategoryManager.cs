using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Managers {
    public class CategoryManager : BaseManager {

        public List<Models.Category> GetByBudgetID(int budgetID) {
            List<Models.Category> categories;

            using (var ctx = new BudgetContext()) {
                categories = ctx.Categories.Where(x => x.BudgetID == budgetID).ToList();
            }

            return categories;
        }

        public Models.Category Get(int ID) {
            Models.Category category;

            using (var ctx = new BudgetContext()) {
                category = ctx.Categories.FirstOrDefault(x => x.CategoryID == ID);
            }

            return category;
        }

        public Models.Category Save(Models.Category category, ref List<string> errors) {
            errors = new List<string>();
            
            using (var ctx = new BudgetContext()) {
                var budget = ctx.Budgets.FirstOrDefault(x => x.BudgetID == category.BudgetID);
                if (budget == null) {
                    //not attached to a valid budget
                    errors.Add("Category is being saved to an invalid budget");
                    return category;
                }

                if (category.CategoryID > 0) {
                    //cannot update budget id from 1 budget to another;
                    var existingCategory = ctx.Categories.FirstOrDefault(x => x.CategoryID == category.CategoryID);
                    if (existingCategory == null || category.BudgetID != existingCategory.BudgetID) {
                        errors.Add("Category being operated on is invalid.  Please try again");
                        return category;
                    }
                }

                try {
                    category = ctx.Categories.Add(category);                    
                    ctx.SaveChanges();
                } catch (Exception ex) {
                    errors = ProcessErrors(ex);
                }
            }

            return category;
        }

        public void Delete(Models.Category category) {
            using (var ctx = new BudgetContext()) {
                ctx.Categories.Remove(category);
                ctx.SaveChanges();
            }
        }
    }
}