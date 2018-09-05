using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.ViewModels.Home {
    public class HomeViewModel {

        public string Name { get; set; }
        public string FormAction { get; set; }
        public List<string> ErrorMessage { get; set; }

        public class FormActions {
            public const string CreateNew = "createnew";
            public const string GetExisting = "getexisting";
        }
    }
}