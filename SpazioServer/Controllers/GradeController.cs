using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class GradeController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Grade> Get()
        {
            Grade g = new Grade();
            return g.getGrades();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public Grade Post([FromBody]Grade grade)
        {
            grade.insert();
            return grade;
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