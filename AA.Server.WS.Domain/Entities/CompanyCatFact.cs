using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Entities
{
    public class CompanyCatFact
    {
        public List<Company>? Companies { get; set; }
        public CatFact? CatFact { get; set; }
    }
}
