using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class SearchController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Search> Get()
        {
            Search s = new Search();
            return s.getSearches();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public Search Post([FromBody]Search search)
        {
            search.insert();
            return search;
        }


        [HttpGet]
        [Route("api/Search/{userid}")]
        public Dictionary<string, int> GetByUser(int userId)
        {
            Dictionary<string,int> Counter = new Dictionary<string, int>();
            Search s = new Search();
            Counter.Add("Counter", s.getUserSearches(userId));
            return Counter;
            
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