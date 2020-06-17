using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class AdminController : ApiController
    {

        // GET api/<controller>
        //public List<Admin> Get()
        //{
        //    Admin a = new Admin();
        //    return s.getAdmins();
        //}
        // api/Admin 
        public IEnumerable<Admin> Get()
        {
            Admin a = new Admin();
            return a.getAdmins();
        }

        // GET: api/Admin/?Email = user.Email
        // GET: api/Admin/?UserName = user.UserName
        public Admin Get(string username)
        {
            try
            {
                Admin a = new Admin();
                return a.GetAdmin(username);
            }
            catch (Exception ex)
            {

                throw new Exception("Get Admin error: ", ex);
            }
        }


        //Check Admins Login details(username and password)
        [HttpGet]
        public bool Get(string username, string adminPassword)
        {
            try
            {
                Admin a = new Admin();
                bool isExsits = a.CheckLoginDetails(username, adminPassword);
                return isExsits;
            }
            catch (Exception ex)
            {
                throw new Exception("Your details are wrong, please try again.", ex);
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
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