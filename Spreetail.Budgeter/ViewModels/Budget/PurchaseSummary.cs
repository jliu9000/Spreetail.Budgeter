using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.ViewModels.Budget {
    public class PurchaseSummary {

        public int BudgetID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Dictionary<int, string> CategoryNames;

        public List<Models.Item> Items { get; set; }

        public double TotalSpent {
            get {
                if (Items?.Count > 0) {
                    return Items.Select(x => x.Cost).Sum();
                } else {
                    return 0;
                }
            }
        }



    }
}