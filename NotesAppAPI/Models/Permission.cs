using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppAPI.Models
{
    public class Permission
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool CanCUD { get; set; }
        public bool CanShare { get; set; }
        public bool CanAssign { get; set; }
        public bool CanProgress { get; set; }
    }
}
