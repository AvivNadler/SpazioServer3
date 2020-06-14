using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class SportFilter
    {
        int id;
        string field;
        double rating;
        int minPrice;
        int maxPrice;
        int maxDistance;
        string startTime;
        string endTime;
        Facility facility;
        Equipment[] eq;
        bool parking;
        bool toilet;
        bool kitchen;
        bool intercom;
        bool accessible;
        bool airCondition;
        bool wiFi;
        bool trx;
        bool treadmill;
        bool stationaryBicycle;
        bool bench;
        bool dumbells;
        bool barbell;
        int userId;
        string date;
        int minCapacity;
        int maxCapacity;

        public SportFilter() { }

        public SportFilter(int id, string field, double rating, int minPrice, int maxPrice, int maxDistance, string startTime, string endTime, Facility facility, Equipment[] eq, bool parking, bool toilet, bool kitchen, bool intercom, bool accessible, bool airCondition, bool wiFi, bool trx, bool treadmill, bool stationaryBicycle, bool bench, bool dumbells, bool barbell, int userId, string date, int minCapacity = 0, int maxCapacity = 0)
        {
            this.Id = id;
            this.Field = field;
            this.Rating = rating;
            this.MinPrice = minPrice;
            this.MaxPrice = maxPrice;
            this.MaxDistance = maxDistance;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Facility = facility;
            this.Eq = eq;
            this.Parking = parking;
            this.Toilet = toilet;
            this.Kitchen = kitchen;
            this.Intercom = intercom;
            this.Accessible = accessible;
            this.AirCondition = airCondition;
            this.WiFi = wiFi;
            this.Trx = trx;
            this.Treadmill = treadmill;
            this.StationaryBicycle = stationaryBicycle;
            this.Bench = bench;
            this.Dumbells = dumbells;
            this.Barbell = barbell;
            this.UserId = userId;
            this.date = date;
            this.minCapacity = minCapacity;
            this.maxCapacity = maxCapacity;
        }

        public int Id { get => id; set => id = value; }
        public string Field { get => field; set => field = value; }
        public double Rating { get => rating; set => rating = value; }
        public int MinPrice { get => minPrice; set => minPrice = value; }
        public int MaxPrice { get => maxPrice; set => maxPrice = value; }
        public int MaxDistance { get => maxDistance; set => maxDistance = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string EndTime { get => endTime; set => endTime = value; }
        public Facility Facility { get => facility; set => facility = value; }
        public Equipment[] Eq { get => eq; set => eq = value; }
        public bool Parking { get => parking; set => parking = value; }
        public bool Toilet { get => toilet; set => toilet = value; }
        public bool Kitchen { get => kitchen; set => kitchen = value; }
        public bool Intercom { get => intercom; set => intercom = value; }
        public bool Accessible { get => accessible; set => accessible = value; }
        public bool AirCondition { get => airCondition; set => airCondition = value; }
        public bool WiFi { get => wiFi; set => wiFi = value; }
        public bool Trx { get => trx; set => trx = value; }
        public bool Treadmill { get => treadmill; set => treadmill = value; }
        public bool StationaryBicycle { get => stationaryBicycle; set => stationaryBicycle = value; }
        public bool Bench { get => bench; set => bench = value; }
        public bool Dumbells { get => dumbells; set => dumbells = value; }
        public bool Barbell { get => barbell; set => barbell = value; }
        public int UserId { get => userId; set => userId = value; }
        public string Date { get => date; set => date = value; }
        public int MinCapacity { get => minCapacity; set => minCapacity = value; }
        public int MaxCapacity { get => maxCapacity; set => maxCapacity = value; }

        public List<SportFilter> getSportFilters()
        {
            DBServices dbs = new DBServices();
            return dbs.readSportFilters();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
    }
}