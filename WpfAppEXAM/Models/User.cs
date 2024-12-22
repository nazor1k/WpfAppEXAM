using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppEXAM.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FullName {  get; set; }
        public string PasswordHash { get; set; }

        public Role Role { get; set; }

        public List<Reservation> Reservations { get; set; }= new List<Reservation>();
    }
}
