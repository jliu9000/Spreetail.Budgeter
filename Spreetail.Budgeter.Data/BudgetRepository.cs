using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spreetail.Budgeter.Model;

namespace Spreetail.Budgeter.Data
{
    public class BudgetRepository : RepositoryBase<Budget>, IBudgetRepository
    {

    }

    public interface IBudgetRepository : Interface.IRepository<Budget>
    {

    }

}
