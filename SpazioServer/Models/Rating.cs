using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Rating
    {
        int id;
        int attitude;
        int cleanliness;
        int equipmentQuality;
        int facilityQualiy;
        int authenticity;
        double totalRating;
        int fKSpaceId;
        int fKUserId;

        public Rating() { }
        public Rating(int id, int attitude, int cleanliness, int equipmentQuality, int facilityQualiy, int authenticity, double totalRating, int fKSpaceId, int fKUserId)
        {
            this.id = id;
            this.attitude = attitude;
            this.cleanliness = cleanliness;
            this.equipmentQuality = equipmentQuality;
            this.facilityQualiy = facilityQualiy;
            this.authenticity = authenticity;
            this.totalRating = totalRating;
            this.fKSpaceId = fKSpaceId;
            this.fKUserId = fKUserId;
        }

        public int Id { get => id; set => id = value; }
        public int Attitude { get => attitude; set => attitude = value; }
        public int Cleanliness { get => cleanliness; set => cleanliness = value; }
        public int EquipmentQuality { get => equipmentQuality; set => equipmentQuality = value; }
        public int FacilityQualiy { get => facilityQualiy; set => facilityQualiy = value; }
        public int Authenticity { get => authenticity; set => authenticity = value; }
        public double TotalRating { get => totalRating; set => totalRating = value; }
        public int FKSpaceId { get => fKSpaceId; set => fKSpaceId = value; }
        public int FKUserId { get => fKUserId; set => fKUserId = value; }

        public List<Rating> getRatings()
        {
            DBServices dbs = new DBServices();
            return dbs.readRatings();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
    }
}