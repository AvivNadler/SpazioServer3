using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class SpaceDataController : ApiController
    {
        // GET api/<controller>
      /*  public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        [HttpGet]
        public List<SpaceData> Get(string field, string city, string street, string number, string date)
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            foreach (Space item in dbs.readSpacesBySearch(field,city,street,number))
            {
                if(dbs.readAllAvailbilities(item.Id, date).Count > 0) 
                {
                    SpaceData s = new SpaceData();
                    s.Space = item;
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    List<string> tempAvail = new List<string>();
                    foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                    {
                        tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    }
                    s.Realavailability = tempAvail;

                    list.Add(s);

                }
                
            }
            return list;
        }

        [HttpGet]
        [Route("api/SpaceData/LastAddedSpaces")]

        public List<SpaceData> Get()
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            foreach (Space item in dbs.readLastAddedSpaces())
            {
                
                    SpaceData s = new SpaceData();
                    s.Space = item;
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    //List<string> tempAvail = new List<string>();
                    //foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, date))
                    //{
                    //    tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    //}
                    //s.Realavailability = tempAvail;

                    list.Add(s);

                

            }
            return list;
        }


        public List<SpaceData> Get(int userId)
        {
            
            Favourite f = new Favourite();
            f.getFavourites(userId);
            string today = DateTime.Today.ToString("dd/MM/yyyy");
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();

            foreach (int item in f.getFavourites(userId))
            {
                    SpaceData s = new SpaceData();
                    s.Space = dbs.readSpaceById(item);
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    List<string> tempAvail = new List<string>();
                    foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, today))
                    {
                        tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    }
                    s.Realavailability = tempAvail;

                    list.Add(s);

            }
            return list;
        }

        [HttpGet]
        public List<SpaceData> Get(string userEmail)
        {
            List<SpaceData> list = new List<SpaceData>();
            DBServices dbs = new DBServices();
            string today = DateTime.Today.ToString("dd/MM/yyyy");

            foreach (Space item in dbs.readMySpaces(userEmail))
            {
                
                    SpaceData s = new SpaceData();
                    s.Space = item;
                    //s.Realavailability = dbs.readAllAvailbilities(s.Space.Id, date);
                    s.Facility = dbs.readFacilities(s.Space.Id);
                    s.Equipment = dbs.readEquipments(s.Space.Id).ToArray();
                    s.Orders = dbs.readOrdersOfSpace(s.Space.Id);
                    s.WeekAvailabilities = dbs.readWeekAvailbilitiesById(s.Space.Id);

                    List<string> tempAvail = new List<string>();
                    foreach (string item2 in dbs.readAllAvailbilities(s.Space.Id, today))
                    {
                        tempAvail.Add(item2.Split('-')[0].Split(':')[0] + ":" + item2.Split('-')[0].Split(':')[1] + "-" + item2.Split('-')[1].Split(':')[0] + ":" + item2.Split('-')[1].Split(':')[1]);
                    }
                    s.Realavailability = tempAvail;

                    list.Add(s);

                

            }
            return list;
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public SpaceData Post([FromBody]SpaceData spaceData)
        {
            bool userType;
            Space s = spaceData.Space;
            Facility f = spaceData.Facility;
            Equipment[] e = spaceData.Equipment;
            WeekAvailability[] wa = spaceData.Availability;
            //Availability a = spaceData.Availability;

            DBServices dbs = new DBServices();


            int newSpaceId = dbs.insert(s);

            f.SpaceId = newSpaceId;

            //  countertest variabe for test how many rows affected 
            int countertest = 0;
            countertest += dbs.insert(f);

            foreach (Equipment item in e)
            {
                item.SpaceId = newSpaceId;
                countertest += dbs.insert(item);
                //dbs.insert(item); test if it can replace the row above??
            }
            foreach (WeekAvailability item in wa)
            {
                item.FkSpaceId = newSpaceId;
                countertest += dbs.insert(item);

            }

            //test if the user is already a spaceOwner
            userType = dbs.userTypeCheckandUpdate(s.UserEmail);

            //e.SpaceId = newSpaceId;
            //a.SpaceId = newSpaceId;
            //countertest += dbs.insert(a);

            return spaceData;
        }



        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}