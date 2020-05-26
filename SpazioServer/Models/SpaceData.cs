using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SpazioServer.Models;
using System.Collections.Generic;


namespace SpazioServer.Models
{
    public class SpaceData
    {
        Space space;
        Facility facility;
        Equipment[] equipment;
        WeekAvailability[] availability;
        List<string> realavailability;
        List<Order> orders;
        List<WeekAvailability> weekAvailabilities;



        public SpaceData() { }

        public SpaceData(Space space, Facility facility, Equipment[] equipment, List<string> realavailability, List<Order> orders, List<WeekAvailability> weekAvailabilities, WeekAvailability[] availability)
        {
            this.space = space;
            this.facility = facility;
            this.equipment = equipment;
            this.realavailability = realavailability;
            this.orders = orders;
            this.weekAvailabilities = weekAvailabilities;
            this.availability = availability;
        }

        public Space Space { get => space; set => space = value; }
        public Facility Facility { get => facility; set => facility = value; }
        public Equipment[] Equipment { get => equipment; set => equipment = value; }
        public List<string> Realavailability { get => realavailability; set => realavailability = value; }
        public List<Order> Orders { get => orders; set => orders = value; }
        public List<WeekAvailability> WeekAvailabilities { get => weekAvailabilities; set => weekAvailabilities = value; }
        public WeekAvailability[] Availability { get => availability; set => availability = value; }
    }
}
