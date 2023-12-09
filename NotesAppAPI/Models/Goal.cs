using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppAPI.Models
{
    public class Goal
    {
        public int ID { get; set; }
        public Subject Subject { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public double Progress { get; set; }
        public DateTime? DueDate { get; set; }
        public User AssignedUser { get; set; }
    }
}
