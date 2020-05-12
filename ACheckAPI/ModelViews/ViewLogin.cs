using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public ViewLogin()
        {
            Email = null;
            Password = null;
        }
    }
}
