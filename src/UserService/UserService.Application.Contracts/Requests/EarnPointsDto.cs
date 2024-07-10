using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Contracts.Requests
{
    public class EarnPointsDto
    {
        public Guid UserId { get; set; }
        public int PointsEarned { get; set; }
    }
}
