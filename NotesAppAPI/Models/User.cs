using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesAppAPI.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}
