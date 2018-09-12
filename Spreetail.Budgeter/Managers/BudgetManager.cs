using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Managers {
    public class BudgetManager : BaseManager {

        public Models.Budget Get(int id) {
            return Get(x => x.BudgetID == id);
        }

        public Models.Budget Search(string name) {
            return Get(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private Models.Budget Get(Func<Models.Budget, bool> getterFunc) {
            Models.Budget budget;

            using (var ctx = new BudgetContext()) {
                budget = ctx.Budgets.FirstOrDefault(getterFunc);
            }

            return budget;
        }


        public Models.Budget Save(Models.Budget budget, ref List<string> errors) {
            errors = new List<string>();

            using (var ctx = new BudgetContext()) {
                var _lock = GetLock(budget.Name);

                lock (_lock) {
                    var existingBudget = ctx.Budgets.FirstOrDefault(x => x.Name.Equals(budget.Name, StringComparison.OrdinalIgnoreCase));
                    //budget name must be unique
                    if (existingBudget != null && existingBudget.BudgetID != budget.BudgetID) {
                        errors.Add("Budget name is already take - please try again");
                        return budget;
                    }

                    try {
                        ctx.Budgets.Add(budget);                                
                        ctx.SaveChanges();
                    } catch (Exception ex) {
                        errors = ProcessErrors(ex);
                    }
                }
            }

            return budget;
        }

        private static object GetLock(string lockName) {
            return BudgetLock.GetOrAdd(lockName, new object());
        }

        private static ConcurrentDictionary<string, object> _BudgetLock;
        private static ConcurrentDictionary<string, object> BudgetLock {
            get {
                if (_BudgetLock == null) {
                    _BudgetLock = new ConcurrentDictionary<string, object>();
                }
                return _BudgetLock;
            }
            set {
                _BudgetLock = value;
            }
        }

    }
}