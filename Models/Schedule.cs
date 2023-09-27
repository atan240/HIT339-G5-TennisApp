// using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApp2.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        
        [Display(Name = "Coach UserName")]
        public string UserName { get; set; }
        public string LessonName { get; set; }
        public DateTime LessonStartDate { get; set; } = DateTime.UtcNow;
        public DateTime LessonEndDate { get; set; } = DateTime.UtcNow;
        public string Location { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public List<Coach>? Coach { get; set; }
        public List<Member>? Member { get; set; }
    }
}
