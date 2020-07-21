using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using SpazioServer.Models;

namespace SpazioServer.Controllers
{
    public class UserController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [Route("api/User")]
        public List<User> Get()
        {
            User  u = new User();
            return  u.getUsers();
        }

        // GET api/<controller>/5

        public User Get(int id)
        {
            User u = new User();
            return u.getUser(id);
        }

        public User Get(string email)
        {
            User u = new User();
            return u.getUser(email);
        }

        // POST api/<controller>

        public User Post([FromBody]User user)
        {
            user.insert();
            return user;

        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("api/User/{id?}")]
        public User Put(int id, [FromBody]User user)
        {
            user.updateUser(user);
            return user;

        }

        [HttpPut]
        [Route("api/User/statusupdate/{id}")]
        public int Put(int id)
        {
            User u = new User();
            return u.updateStatus(id); 

        }

        [HttpPut]
        [Route("api/User/cancelpremium/{id}")]
        public int Put2(int id)
        {
            User u = new User();
            return u.updateCancelPremium(id);

        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            User u = new User();
            u.deleteUser(id);
        }


        [HttpGet]
        [Route("api/User/{id?}/favourites")]
        public List<int> GetFavourites(int id)
        {
            User u = new User();
            return u.getFavourites(id);
        }
        
    }
}