using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanApp.Models
{
    public class PlanGroup
    {
        public int PlanId { get; set; }
        public string GroupName { get; set; } = null!;
        public int? UserId { get; set; }
        public bool IsPublished { get; set; }
        public string GroupDescription { get; set; } = null!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
        public virtual ICollection<PlanPlace> PlanPlaces { get; set; } = new List<PlanPlace>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual User? User { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public virtual ICollection<User> UsersNavigation { get; set; } = new List<User>();

        public PlanGroup() { }
    }
}
