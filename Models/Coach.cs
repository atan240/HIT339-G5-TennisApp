// using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApp2.Models
{
    public class Coach
    {
        public int Id { get; set; }
        [Display(Name = "Coach Username")]
        public string UserName { get; set; }
        [Display(Name = "Image URL")]
        public string? Image { get; set; }
        public string? Biography { get; set; }
        public string? Expertise { get; set; }
        public string? Accreditations { get; set; }
        public bool? IsDeleted { get; set; } = false;
        
        public List<Schedule>? Schedule { get; set; }
    }
}
