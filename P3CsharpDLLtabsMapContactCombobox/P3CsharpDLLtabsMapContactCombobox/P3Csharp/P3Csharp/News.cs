using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3Csharp
{
    public class Older
    {
        public string date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class News
    {
        public List<Older> older { get; set; }
    }
}
