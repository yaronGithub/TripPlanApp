using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanApp.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public string Title { get; set; } = null!;

        public int? PlanId { get; set; }

        public int? UserId { get; set; }

        public int? Stars { get; set; }

        public string ReviewText { get; set; } = null!;
        public DateOnly? ReviewDate { get; set; }

        public virtual PlanGroup? Plan { get; set; }

        public virtual User? User { get; set; }

        public Review() { }
    }
}
