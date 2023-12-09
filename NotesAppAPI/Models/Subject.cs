using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppAPI.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
    }
}
