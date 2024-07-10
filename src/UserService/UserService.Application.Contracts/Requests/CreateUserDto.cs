using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.Requests
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
