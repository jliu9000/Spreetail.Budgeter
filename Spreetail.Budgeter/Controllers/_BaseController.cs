using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;

namespace Spreetail.Budgeter.Controllers
{
    public abstract class _BaseController : Controller
    {

        protected List<string> ModelStateErrors()
        {
            return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToList();

        }


    }
}