using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Grade
    {

        int gradeId;
        double price;
        double capacity;
        double facility;
        double equipment;
        double rating;
        double premium;
        double order;
        double conversion;
        string modifiedDate;


        public Grade()
        {
            
        }

        public Grade(int gradeId, double price, double capacity, double facility, double equipment, double rating, double premium, double order, double conversion, string modifiedDate)
        {
            this.GradeId = gradeId;
            this.Price = price;
            this.Capacity = capacity;
            this.Facility = facility;
            this.Equipment = equipment;
            this.Rating = rating;
            this.Premium = premium;
            this.Order = order;
            this.conversion = conversion;
            this.ModifiedDate = modifiedDate;
        }

        public int GradeId { get => gradeId; set => gradeId = value; }
        public double Price { get => price; set => price = value; }
        public double Capacity { get => capacity; set => capacity = value; }
        public double Facility { get => facility; set => facility = value; }
        public double Equipment { get => equipment; set => equipment = value; }
        public double Rating { get => rating; set => rating = value; }
        public double Premium { get => premium; set => premium = value; }
        public double Order { get => order; set => order = value; }
        public double Conversion { get => conversion; set => conversion = value; }
        public string ModifiedDate { get => modifiedDate; set => modifiedDate = value; }

        public List<Grade> getGrades()
        {
            DBServices dbs = new DBServices();
            return dbs.readGrades();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
    }
}