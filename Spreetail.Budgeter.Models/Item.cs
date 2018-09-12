using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Model
{
    public class Item
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Cost { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        public int BudgetID { get; set; }

        public int ReoccuringItemID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryID { get; set; }

    }
}