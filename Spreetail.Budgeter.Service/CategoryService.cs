using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spreetail.Budgeter.Data;


namespace Spreetail.Budgeter.Service
{

    public interface ICategoryService
    {

    }

    class CategoryService : ICategoryService
    {
        private ICategoryRepository repo;
        public CategoryService(ICategoryRepository CategoryRepository)
        {
            repo = CategoryRepository;
        }



    }
}
