using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Spreetail.Budgeter.Model;

namespace Spreetail.Budgeter.Data
{
    public class BudgetContext : DbContext {
        public BudgetContext() : base("dbBudgeter") {
            Database.SetInitializer<BudgetContext>(new DropCreateDatabaseIfModelChanges<BudgetContext>());
            var ensureDllIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ReoccuringItem> ReoccuringItems { get; set; }
    }
}