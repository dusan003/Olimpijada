using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olimpijada.Models
{
    public class Group
    {
        public string GroupName { get; set; }
        public List<Country> Teams { get; set; }
    }
}
