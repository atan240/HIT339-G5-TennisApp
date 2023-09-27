using System.ComponentModel.DataAnnotations.Schema;

namespace TennisApp2.Models
{
    public class ScheduleJoin
    {
        public int Id { get; set; }

        [ForeignKey("MemberId")]
        public int MemberId { get; set; }
        public List<Member>? Member { get; set; }

        [ForeignKey("ScheduleId")]
        public int ScheduleId { get; set; }
        public List<Schedule>? Schedule { get; set; }


    }
}
