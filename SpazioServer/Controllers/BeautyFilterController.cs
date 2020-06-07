using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class BeautyFilterController : ApiController
    {
        // GET api/<controller>
        public List<BeautyFilter> Get()
        {
            BeautyFilter af = new BeautyFilter();
            return af.getBeautyFilters();
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public BeautyFilter Post([FromBody]BeautyFilter beautyFilter)
        {

            beautyFilter.insert();
            return beautyFilter;

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