using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class WeekAvailability
    {
        int id;
        string day;
        string startTime;
        string endTime;
        int fkSpaceId;
        public WeekAvailability() { }

        public WeekAvailability(int id, string day, string startTime, string endTime, int fkSpaceId)
        {
            this.id = id;
            this.day = day;
            this.startTime = startTime;
            this.endTime = endTime;
            this.fkSpaceId = fkSpaceId;
        }

        public int Id { get => id; set => id = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string EndTime { get => endTime; set => endTime = value; }
        public int FkSpaceId { get => fkSpaceId; set => fkSpaceId = value; }
        public string Day { get => day; set => day = value; }

        public List<WeekAvailability> getWeekAvailbilities()
        {
            DBServices dbs = new DBServices();
            return dbs.readWeekAvailbilities();
        }
        public List<WeekAvailability> getWeekAvailbility(int spaceId,string day)
        {
            DBServices dbs = new DBServices();
            return dbs.readWeekAvailbility(spaceId,day);
        }



        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }


    }
}