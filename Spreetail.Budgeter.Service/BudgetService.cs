using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spreetail.Budgeter.Data;
using Spreetail.Budgeter.Model;

namespace Spreetail.Budgeter.Service
{

    public interface IBudgetService
    {
        void AddBudget(Budget budget);
        Budget GetBudgetByName(string name);
        Budget GetBudget(int id);

        Category GetCategory(int id);
        List<Category> GetCategoriesByBudgetID(int id);
        int SaveCategory(Category category);

        List<Item> GetItemsByCatAndMonth(int catId, int month, int year);
        List<Item> GetItemsByRange(int budgetId, DateTime startDate, DateTime endDate);
        void SaveItem(Item item);
        void DeleteItem(int id);

        ReoccuringItem GetReoccuringItem(int id);
        void SaveAndGenerateReoccuring(ReoccuringItem reoccuringItem);
        void DeleteReoccuringItem(int id);
    }


    public class BudgetService : IBudgetService
    {
        private IBudgetRepository BudgetRepo;
        private ICategoryRepository CategoryRepo;
        private IItemRepository ItemRepo;
        private IReoccuringItemRepository ReoccuringItemRepo;

        public BudgetService(IBudgetRepository BudgetRepository,
                            ICategoryRepository CategoryRepository,
                            IItemRepository ItemRepository,
                            IReoccuringItemRepository ReoccuringItemRepository)
        {
            BudgetRepo = BudgetRepository;
            CategoryRepo = CategoryRepository;
            ItemRepo = ItemRepository;
            ReoccuringItemRepo = ReoccuringItemRepository;
        }

        #region Budget
        public void AddBudget(Budget budget)
        {
            BudgetRepo.Add(budget);
            BudgetRepo.SaveChanges();
        }

        public Budget GetBudget(int id)
        {
            return BudgetRepo.GetById(id);
        }

        public Budget GetBudgetByName(string name)
        {
            return BudgetRepo.GetOne(x => x.Name.ToLower() == name.ToLower());
        }



        #endregion

        #region Category

        public Category GetCategory(int id)
        {
            return CategoryRepo.GetById(id);
        }

        public List<Category> GetCategoriesByBudgetID(int id)
        {
            return CategoryRepo.GetMany(x => x.BudgetID == id).ToList();
        }

        public int SaveCategory(Category category)
        {
            if (category.CategoryID > 0)
            {
                CategoryRepo.Update(category);
            }
            else
            {
                CategoryRepo.Add(category);
            }

            CategoryRepo.SaveChanges();

            return category.CategoryID;
        }
        #endregion  

        #region Item
        public List<Item> GetItemsByCatAndMonth(int catId, int month, int year)
        {
            return ItemRepo.GetMany(x => x.CategoryID == catId).
                            Where(x => x.PurchaseDate.Year == year && x.PurchaseDate.Month == month).
                            OrderBy(x => x.PurchaseDate).
                            ToList();
        }

        public List<Item> GetItemsByRange(int budgetId, DateTime startDate, DateTime endDate)
        {
            return ItemRepo.GetMany(x => x.BudgetID == budgetId).
                            Where(x => x.PurchaseDate >= startDate && x.PurchaseDate <= endDate).
                            OrderBy(x => x.PurchaseDate).
                            ToList();
        }

        public void SaveItem(Item item)
        {
            if (item.ItemID > 0)
            {
                ItemRepo.Update(item);
            }
            else
            {
                ItemRepo.Add(item);
            }
            ItemRepo.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var item = new Item() { ItemID = id };
            ItemRepo.Delete(item);
            ItemRepo.SaveChanges();
        }

        #endregion


        #region ReoccuringItems

        public void DeleteReoccuringItem(int id)
        {
            var items = ItemRepo.GetMany(x => x.ReoccuringItemID == id);
            foreach (var item in items)
            {
                ItemRepo.Delete(item);
            }
            ItemRepo.SaveChanges();

            ReoccuringItemRepo.Delete(id);
            ReoccuringItemRepo.SaveChanges();
        }

        public void SaveAndGenerateReoccuring(ReoccuringItem reoccuringItem)
        {
            if (reoccuringItem.ReoccuringItemID > 0)
            {
                ReoccuringItemRepo.Update(reoccuringItem);
            }
            else
            {
                ReoccuringItemRepo.Add(reoccuringItem);
            }
            ReoccuringItemRepo.SaveChanges();

            var existingItems = ItemRepo.GetMany(x => x.ReoccuringItemID == reoccuringItem.ReoccuringItemID);

                foreach (var delItem in existingItems)
                {
                    ItemRepo.Delete(delItem);
                }

            //generate new items
            var currentDate = reoccuringItem.StartDate;
            while (currentDate < reoccuringItem.EndDate)
            {
                //add to db
                var item = new Item()
                {
                    ReoccuringItemID = reoccuringItem.ReoccuringItemID,
                    CategoryID = reoccuringItem.CategoryID,
                    BudgetID = reoccuringItem.BudgetID,
                    ItemName = reoccuringItem.Name,
                    Cost = reoccuringItem.ReoccuringCost,
                    PurchaseDate = currentDate
                };


                if (reoccuringItem.ReoccuringUnit == ReoccuringItem.eReoccuringUnit.Month)
                {
                    currentDate = currentDate.AddMonths(reoccuringItem.ReoccuringRate);
                }
                else
                {
                    currentDate = currentDate.AddDays(reoccuringItem.ReoccuringRate * 7);
                }
                ItemRepo.Add(item);
            }
            ItemRepo.SaveChanges();
        }

        public ReoccuringItem GetReoccuringItem(int id)
        {
            return ReoccuringItemRepo.GetById(id);
        }


        #endregion

    }
}
