using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class SpaceVisitController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<SpaceVisit> Get()
        {
            SpaceVisit sv = new SpaceVisit();
            return sv.getSpaceVisits();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public SpaceVisit Post([FromBody]SpaceVisit sv)
        {
            sv.insert();
            return sv;
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