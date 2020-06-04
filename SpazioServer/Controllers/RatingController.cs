using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class RatingController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Rating> Get()
        {
            Rating r = new Rating();
            return r.getRatings();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public Rating Post([FromBody]Rating rating)
        {
            rating.insert();
            return rating;
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