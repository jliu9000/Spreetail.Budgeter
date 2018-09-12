using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

using Spreetail.Budgeter.Model;
namespace Spreetail.Budgeter.ViewModels.Budget {
    public class BudgetViewModel {
        public int BudgetID { get; set; }

        public string Name { get; set; }

        public DateTime BudgetDate { get; set; }

        public List<ItemsByCategory> Items { get; set; }

    }

    public class ItemsByCategory {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public double BudgetAmount { get; set; }
        public List<Item> Items { get; set; }

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