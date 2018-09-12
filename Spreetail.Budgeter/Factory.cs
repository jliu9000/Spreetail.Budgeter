using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spreetail.Budgeter {
    internal class Factory {

        private Managers.BudgetManager _BudgetManager;
        public Managers.BudgetManager BudgetManager {
            get {
                if (_BudgetManager == null) {
                    _BudgetManager = new Managers.BudgetManager();
                }
                return _BudgetManager;
            }
        }

        private Managers.CategoryManager _CategoryManager;
        public Managers.CategoryManager CategoryManager {
            get {
                if (_CategoryManager == null) {
                    _CategoryManager = new Managers.CategoryManager();
                }
                return _CategoryManager;
            }
        }

        private Managers.ItemManager _ItemManager;
        public Managers.ItemManager ItemManager {
            get {
                if (_ItemManager == null) {
                    _ItemManager = new Managers.ItemManager();
                }

                return _ItemManager;
            }
        }

        private Managers.ReoccuringItemManager _ReoccuringItemManager;
        public Managers.ReoccuringItemManager ReoccuringItemManager {
            get {
                if (_ReoccuringItemManager == null) {
                    _ReoccuringItemManager = new Managers.ReoccuringItemManager();
                }

                return _ReoccuringItemManager;
            }
        }


    }
}