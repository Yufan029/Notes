using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSettings
{
    public class Proxy
    {
        public string Address {  get; set; }
        public int Port { get; set; }
        public int[] Ids { get; set; }
    }
}
