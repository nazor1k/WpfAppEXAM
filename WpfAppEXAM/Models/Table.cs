using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppEXAM.Models
{
    public class Table
    {
        public int Id { get; set; }

        public int Seats {  get; set; }

        public string Description { get; set; }

        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        
    }
}
