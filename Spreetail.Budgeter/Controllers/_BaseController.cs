using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;

namespace Spreetail.Budgeter.Controllers {
    public abstract class _BaseController : Controller {

        private Factory _Factory;
        internal Factory Factory {
            get {
                if (_Factory == null) {
                    _Factory = new Factory();
                }
                return _Factory;
            }
        }



    }
}