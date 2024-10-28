using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanApp.Models
{
    public class LoginInfo
    {
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [StringLength(50)]
        public string Passwd { get; set; } = null!;
    }
}
