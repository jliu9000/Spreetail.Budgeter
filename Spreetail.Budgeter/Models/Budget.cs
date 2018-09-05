using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Spreetail.Budgeter.Models
{
    public class Budget
    {
        [Key]
        public int BudgetID { get; set; }

        [Required]
        [MinLength(1)]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Item> Items { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double BudgetAmount;

    }
}