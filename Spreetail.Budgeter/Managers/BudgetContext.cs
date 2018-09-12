using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Managers {
    internal class BudgetContext : DbContext {
        public BudgetContext() : base("dbBudgeter") {
            Database.SetInitializer<BudgetContext>(new DropCreateDatabaseIfModelChanges<BudgetContext>());
        }

        public DbSet<Models.Budget> Budgets { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Item> Items { get; set; }
        public DbSet<Models.ReoccuringItem> ReoccuringItems { get; set; }

    }
}