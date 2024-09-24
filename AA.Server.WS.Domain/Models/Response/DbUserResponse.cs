using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.Server.WS.Domain.Models.Response
{
    public class DbUserResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsActive { get; set; }
    }
}
