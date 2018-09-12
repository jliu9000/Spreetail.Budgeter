using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spreetail.Budgeter.Data.Interface;
using Spreetail.Budgeter.Model;

namespace Spreetail.Budgeter.Data
{
    public class ItemRepository : RepositoryBase<Item>, IItemRepository
    {

    }

    public interface IItemRepository : IRepository<Item>
    {



    }
}
