using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanApp.Models
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string PlacePicUrl { get; set; } = null!;
        public string PlaceName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public string PlaceDescription { get; set; } = null!;
        public double? Xcoor { get; set; }
        public double? Ycoor { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
        public virtual ICollection<PlanPlace> PlanPlaces { get; set; } = new List<PlanPlace>();

        public Place() { }
    }
}
