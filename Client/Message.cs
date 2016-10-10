using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

   class Message
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public string Display
        {
            get
            {
                return Username + ":\n" + Text;
            }
        }
    }
}
