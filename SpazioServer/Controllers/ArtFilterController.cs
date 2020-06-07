using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class ArtFilterController : ApiController
    {
        // GET api/<controller>
        public List<ArtFilter> Get()
        {
            ArtFilter af = new ArtFilter();
            return af.getArtFilters();
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public ArtFilter Post([FromBody]ArtFilter artFilter)
        {

            artFilter.insert();
            return artFilter;

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