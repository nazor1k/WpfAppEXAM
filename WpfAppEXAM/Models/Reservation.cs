using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppEXAM.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public DateTime DateFrom {  get; set; }
        public DateTime DateTo { get; set; }


        public Table Table { get; set; }
        public int TableId { get; set; }
        

        public User User { get; set; }
        public int UserId { get; set; }


    }
}
