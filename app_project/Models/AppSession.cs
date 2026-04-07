using app_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_project.Models
{
    public static class AppSession
    {
        public static User CurrentUser { get; set; }
    }
}
