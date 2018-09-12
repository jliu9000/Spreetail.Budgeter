using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.ViewModels.Budget {
    public class BudgetViewModel {
        public int BudgetID { get; set; }
        public string Name { get; set; }

        public DateTime BudgetDate { get; set; }

        public List<ItemsByCategory> Items { get; set; }

    }

    public class ItemsByCategory {
        public int CategoryID;
        public string CategoryName;
        public double BudgetAmount;
        public List<Models.Item> Items;

        public double ItemCostTotal {
            get {
                return Items.Select(x => x.Cost).Sum();
            }
        }
        public double BudgetRemaining {
            get {
                return BudgetAmount - ItemCostTotal;
            }
        }

        

    }
}