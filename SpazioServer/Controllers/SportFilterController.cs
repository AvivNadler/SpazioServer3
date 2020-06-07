using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class SportFilterController : ApiController
    {
        // GET api/<controller>
        // GET api/<controller>
        public List<SportFilter> Get()
        {
            SportFilter af = new SportFilter();
            return af.getSportFilters();
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public SportFilter Post([FromBody]SportFilter sportFilter)
        {

            sportFilter.insert();
            return sportFilter;

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