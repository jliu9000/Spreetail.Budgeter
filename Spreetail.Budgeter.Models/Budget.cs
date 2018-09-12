using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spreetail.Budgeter.Model
{
    public class Budget
    {
        [Key]
        public int BudgetID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MinLength(1)]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double BudgetAmount { get; set; }

    }
}