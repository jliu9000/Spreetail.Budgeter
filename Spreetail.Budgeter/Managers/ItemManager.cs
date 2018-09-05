using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Managers {
    public class ItemManager : BaseManager {

        public List<Models.Item> GetByCategoryID(int categoryID) {
            return Get(x => x.CategoryID == categoryID);
        }

        public List<Models.Item> GetByBudgetID(int budgetID) {
            return Get(x => x.BudgetID == budgetID);
        }

        public List<Models.Item> GetByReoccuringID(int reoccuringID) {

            return Get(x => x.ReoccuringItemID == reoccuringID);
        }

        private List<Models.Item> Get(Func<Models.Item, bool> whereClause) {
            List<Models.Item> itemsByCat;

            using (var ctx = new BudgetContext()) {
                itemsByCat = ctx.Items.Where(whereClause).ToList();
            }

            return itemsByCat;
        }

        public Models.Item Save(Models.Item item, ref List<string> errors) {
            errors = new List<string>();

            using (var ctx = new BudgetContext()) {
                var budget = ctx.Budgets.FirstOrDefault(x => x.BudgetID == item.BudgetID);
                if (budget == null) {
                    errors.Add("Budget not found");
                    return item;
                }

                try {
                    item = ctx.Items.Add(item);
                    if (item.ItemID > 0) {
                        ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    ctx.SaveChanges();
                } catch (Exception ex) {
                    errors = ProcessErrors(ex);
                }
            }


            return item;
        }

        public void Delete(Models.Item item) {
            using (var ctx = new BudgetContext()) {
                try {
                    ctx.Items.Attach(item);
                    item = ctx.Items.Remove(item);
                    ctx.SaveChanges();
                } catch (Exception ex) {
                    ProcessErrors(ex);
                }
            }
        }


    }
}