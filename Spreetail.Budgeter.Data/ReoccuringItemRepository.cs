using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spreetail.Budgeter.Data.Interface;
using Spreetail.Budgeter.Model;


namespace Spreetail.Budgeter.Data
{
    public class ReoccuringItemRepository : RepositoryBase<ReoccuringItem>, IReoccuringItemRepository
    {
        public void Delete(int id)
        {
            var reoccuring = new ReoccuringItem() { ReoccuringItemID = id };
            base.Delete(reoccuring);
        }
    }


    public interface IReoccuringItemRepository : IRepository<ReoccuringItem>
    {
        void Delete(int id);
    }

}
