using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Model
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [StringLength(50)]
        [Required]
        public string CategoryName { get; set; }
        
        [Required]
        public int BudgetID { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double BudgetAmount { get; set; }


    }
}