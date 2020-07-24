using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Search
    {
        int id;
        string field;
        string location;
        string time;
        string city;
        string street;
        string number;
        string inputDate;
        string searchDate;
        int userId;

        public Search() { }

        public Search(int id, string field, string location, string time, string city, string street, string number, string inputDate, string searchDate, int userId = 0)
        {
            this.id = id;
            this.field = field;
            this.location = location;
            this.time = time;
            this.city = city;
            this.street = street;
            this.number = number;
            this.inputDate = inputDate;
            this.searchDate = searchDate;
            this.userId = userId;
        }

        public int Id { get => id; set => id = value; }
        public string Field { get => field; set => field = value; }
        public string Location { get => location; set => location = value; }
        public string Time { get => time; set => time = value; }
        public string City { get => city; set => city = value; }
        public string Street { get => street; set => street = value; }
        public string Number { get => number; set => number = value; }
        public string InputDate { get => inputDate; set => inputDate = value; }
        public string SearchDate { get => searchDate; set => searchDate = value; }
        public int UserId { get => userId; set => userId = value; }

        public List<Search> getSearches()
        {
            DBServices dbs = new DBServices();
            return dbs.readSearches();
        }
       
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }

        public int getUserSearches(int userId)
        {
            DBServices dbs = new DBServices();
            return dbs.countUserSearches(userId);


        }
    }
}