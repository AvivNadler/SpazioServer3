using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SpazioServer.Models;


namespace SpazioServer.Controllers
{
    public class OrderController : ApiController
    {

        // GET api/<controller>

        public List<Order> Get()
        {
            Order o = new Order();
            return o.getOrders();
        }

        [HttpGet]
        [Route("api/Order/{field}")]
        public List<Order> GetByField(string field)
        {
            Order o = new Order();
            return o.getOrdersByField(field);
        }


        // GET api/<controller>/5


        // POST api/<controller>
        public Order Post([FromBody]Order order)
        {
            order.insert();
            return order;
        }


        [HttpGet]
        public List<OrderData> Get(int spaceid)
        {
            DBServices dbs = new DBServices();

            List<OrderData> od = dbs.readOrdersDataBySpaceId(spaceid);
            return od; ;
        }


        [Route("api/order/check")]
        public List<List<OrderData>> Get([FromBody]int[] spaces)
        {
            List<List<OrderData>> orderdata = new List<List<OrderData>>();
            DBServices dbs = new DBServices();
            string today = DateTime.Today.ToString("dd/MM/yyyy");
            
            foreach (int item in spaces)
            {
                List<OrderData> o = dbs.readOrdersDataBySpaceId(item);

                orderdata.Add(o);

            }
            return orderdata;
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