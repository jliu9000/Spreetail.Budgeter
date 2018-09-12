using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spreetail.Budgeter.Model;

namespace Spreetail.Budgeter.Data
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {


    }


    public interface ICategoryRepository : Interface.IRepository<Category>
    {

    }
}
