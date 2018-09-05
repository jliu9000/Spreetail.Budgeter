using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Spreetail.Budgeter.Managers {
    public class ReoccuringItemManager : BaseManager {

        public Models.ReoccuringItem Get(int id) {
            var reoccuring = new Models.ReoccuringItem();
            using (var ctx = new BudgetContext()) {
                reoccuring = ctx.ReoccuringItems.FirstOrDefault(x => x.ReoccuringItemID == id);
            }
            return reoccuring;
        }

        public Models.ReoccuringItem Save(Models.ReoccuringItem item, ref List<string> errors) {
            errors = new List<string>();
            if (item.StartDate > item.EndDate) {
                errors.Add("Reoccuring end date must be greater than the start date");
                return item;
            }

            using (var ctx = new BudgetContext()) {
                try {
                    item = ctx.ReoccuringItems.Add(item);
                    if (item.ReoccuringItemID > 0) {
                        ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    ctx.SaveChanges();
                } catch (Exception ex) {
                    errors = ProcessErrors(ex);
                }
            }
            return item;
        }

        public void Delete(Models.ReoccuringItem item) {
            using (var ctx = new BudgetContext()) {
                ctx.ReoccuringItems.Attach(item);
                item = ctx.ReoccuringItems.Remove(item);
                ctx.SaveChanges();

            }
        }


    }
}