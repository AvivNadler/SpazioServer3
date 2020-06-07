using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class SpaceVisit
    {
        int id;
        int spaceId;
        int userId;
        string visitDate;

        public SpaceVisit() { }

        public SpaceVisit(int id, int spaceId, int userId, string visitDate)
        {
            this.id = id;
            this.spaceId = spaceId;
            this.userId = userId;
            this.visitDate = visitDate;
        }

        public int Id { get => id; set => id = value; }
        public int SpaceId { get => spaceId; set => spaceId = value; }
        public int UserId { get => userId; set => userId = value; }
        public string VisitDate { get => visitDate; set => visitDate = value; }

        public List<SpaceVisit> getSpaceVisits()
        {
            DBServices dbs = new DBServices();
            return dbs.readSpaceVisits();
        }
        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
    }
}