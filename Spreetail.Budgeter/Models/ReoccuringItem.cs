using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Spreetail.Budgeter.Models
{
    public class ReoccuringItem
    {
        [Key]
        public int ReoccuringItemID { get; set; }
        [Required]
        public int BudgetID { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
        public DateTime StartDate { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
        public DateTime EndDate { get; set; }

        [Required]
        public int ReoccuringRate { get; set; }

        [Required]
        public eReoccuringUnit ReoccuringUnit { get; set; }

        [Range(0, double.MaxValue)]
        public double ReoccuringCost { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public enum eReoccuringUnit {
            Week = 1,
            Month =2
        }

    }
}