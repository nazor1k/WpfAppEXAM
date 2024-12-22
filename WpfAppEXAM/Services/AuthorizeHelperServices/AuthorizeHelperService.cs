using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppEXAM.Models;

namespace WpfAppEXAM.Services.AuthorizeHelperServices
{
    public class AuthorizeHelperService
    {
        public static User? User {  get; set; }
        public static Reservation? Reservation { get; set; }
    }
}
