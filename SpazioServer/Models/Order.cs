using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class Order
    {
        int orderId;
        int spaceId;
        int userId;
        string reservationDate;
        string startHour;
        string endHour;
        double price;
        string orderDate;

        public Order(int orderId, int spaceId, int userId, string reservationDate, string startHour, string endHour, string orderDate, double price)
        {
            this.orderId = orderId;
            this.spaceId = spaceId;
            this.userId = userId;
            this.reservationDate = reservationDate;
            this.startHour = startHour;
            this.endHour = endHour;
            this.orderDate = orderDate;
            this.price = price;
        }
        public Order() { }

        public int OrderId { get => orderId; set => orderId = value; }
        public int SpaceId { get => spaceId; set => spaceId = value; }
        public int UserId { get => userId; set => userId = value; }
        public string ReservationDate { get => reservationDate; set => reservationDate = value; }
        public string StartHour { get => startHour; set => startHour = value; }
        public string EndHour { get => endHour; set => endHour = value; }
        public string OrderDate { get => orderDate; set => orderDate = value; }
        public double Price { get => price; set => price = value; }

        public List<Order> getOrders()
        {
            DBServices dbs = new DBServices();
            return dbs.readOrders();
        }



        public List<Order> getOrdersByField(string field)
        {
            DBServices dbs = new DBServices();
            return dbs.readOrdersByField(field);
        }
        

        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
        public Order getOrder(int id)
        {
            DBServices dbs = new DBServices();
            return dbs.readOrder(id);
        }

/*        public Order updateOrder(int id)
        {
            DBServices dbs = new DBServices();
            return dbs.updateOrder(id);
        }*/


    }
}