using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class RealAvailabilityController : ApiController
    {
        //public List<RealAvailability> Get()
        //{
        //    RealAvailability a = new RealAvailability();
        //    return a.getRealAvailbilities();
        //}


        //public List<RealAvailability> GetBySpace(int spaceId)
        //{
        //    RealAvailability a = new RealAvailability();
        //    return a.getRealAvailbilities();
        //}

        // GET api/<controller>/5

        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public List<string> Get(int spaceid,string date)
        {
            RealAvailability ra = new RealAvailability();
            return ra.getAllAvailbilities(spaceid, date);
        }




        // POST api/<controller>



        public RealAvailability Post([FromBody]RealAvailability rAvailability)
        {

            rAvailability.insert();
            return rAvailability;

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [HttpPut]
        [Route("api/RealAvailability/update")]
        public void update([FromBody]RealAvailability realAvailability)
        {
            realAvailability.update(realAvailability);
        }
    }
}