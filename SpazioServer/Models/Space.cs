using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Space
    {
        int id;
        string name;
        string field;
        double price;
        string city;
        string street;
        string number;
        int capabillity;
        string bank;
        string branch;
        string accountNumber;
        string imageurl1;
        string imageurl2;
        string imageurl3;
        string imageurl4;
        string imageurl5;
        string userEmail;
        string description;
        string termsOfUse;
        double rank;
        string uploadtime; 
        

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Field { get => field; set => field = value; }
        public double Price { get => price; set => price = value; }
        public string City { get => city; set => city = value; }
        public string Street { get => street; set => street = value; }
        public string Number { get => number; set => number = value; }
        public int Capabillity { get => capabillity; set => capabillity = value; }
        public string Bank { get => bank; set => bank = value; }
        public string Branch { get => branch; set => branch = value; }
        public string AccountNumber { get => accountNumber; set => accountNumber = value; }
        public string Imageurl1 { get => imageurl1; set => imageurl1 = value; }
        public string Imageurl2 { get => imageurl2; set => imageurl2 = value; }
        public string Imageurl3 { get => imageurl3; set => imageurl3 = value; }
        public string Imageurl4 { get => imageurl4; set => imageurl4 = value; }
        public string Imageurl5 { get => imageurl5; set => imageurl5 = value; }
        public string UserEmail { get => userEmail; set => userEmail = value; }
        public string Description { get => description; set => description = value; }
        public string TermsOfUse { get => termsOfUse; set => termsOfUse = value; }
        public double Rank { get => rank; set => rank = value; }
        public string Uploadtime { get => uploadtime; set => uploadtime = value; }

        public Space(int id, string name, string field, float price, string city, string street, string number, int capabillity, string bank, string branch, string accountNumber, string imageurl1, string imageurl2, string imageurl3, string imageurl4, string imageurl5, string userEmail, string description, string termsOfUse, double rank, string uploadtime)
        {
            this.id = id;
            this.name = name;
            this.field = field;
            this.price = price;
            this.city = city;
            this.street = street;
            this.number = number;
            this.capabillity = capabillity;
            this.bank = bank;
            this.branch = branch;
            this.accountNumber = accountNumber;
            this.imageurl1 = imageurl1;
            this.imageurl2 = imageurl2;
            this.imageurl3 = imageurl3;
            this.imageurl4 = imageurl4;
            this.imageurl5 = imageurl5;
            this.userEmail = userEmail;
            this.description = description;
            this.termsOfUse = termsOfUse;
            this.rank = rank;
            this.uploadtime = uploadtime;
        }
        public Space() { }

        public List<Space> getSpaces()
        {
            DBServices dbs = new DBServices();
            return dbs.readSpaces();
        }
        public List<Space> getSpacesLastThirtyDays()
        {
            DBServices dbs = new DBServices();
            return dbs.readSpacesFromLastThirtyDays();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

       /* public int updateSpace(int id)
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.updateSpace(this);
            return numAffected;
        }*/

        public int deleteSpace(int id)
        {
            DBServices dbs = new DBServices();
            int numEffected = dbs.deleteSpace(id);
            return numEffected;

        }
        
            public List<Space> getSpacesBySearch(string field,string city, string street,string number)
        {
            DBServices dbs = new DBServices();
            return dbs.readSpacesBySearch(field,city,street,number);
        }


    }
}