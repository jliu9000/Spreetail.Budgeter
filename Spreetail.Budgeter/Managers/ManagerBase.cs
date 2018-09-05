using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Managers {
    public class BaseManager {

        protected List<string> ProcessErrors(Exception ex) {
            var errors = new List<string>();

            if (ex.GetType() == typeof(DbEntityValidationException)) {
                foreach (var error in ((DbEntityValidationException)ex).EntityValidationErrors.First().ValidationErrors) {
                    errors.Add(error.ErrorMessage);
                }
            } else {
                //TODO: log ex here

                errors.Add("Server error has occured");
            }

            return errors;
        }
    }
}