using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanApp.Models
{
    public class PlanPlace
    {
        public int PlaceId { get; set; }

        public int PlanId { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

        public virtual Place Place { get; set; } = null!;

        public virtual PlanGroup Plan { get; set; } = null!;

        public PlanPlace() { }
    }
}
