using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpazioServer.Models
{
    public class User
    {
        int id;
        string userName;
        string email;
        string password;
        string phoneNumber;
        string photo;
        bool spaceOwner;
        int visits;
        double rank;
        string registrationDate;
        string spacePublish;
        bool premium;

        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Photo { get => photo; set => photo = value; }
        public bool SpaceOwner { get => spaceOwner; set => spaceOwner = value; }
        public int Visits { get => visits; set => visits = value; }
        public double Rank { get => rank; set => rank = value; }
        public string RegistrationDate { get => registrationDate; set => registrationDate = value; }
        public string SpacePublish { get => spacePublish; set => spacePublish = value; }
        public bool Premium { get => premium; set => premium = value; }

        public User() { }

        public User(int id, string userName, string email, string password, string phoneNumber, string photo, bool spaceOwner, int visits, double rank, string registrationDate, string spacePublish, bool premium)
        {
            this.id = id;
            this.userName = userName;
            this.email = email;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.photo = photo;
            this.spaceOwner = spaceOwner;
            this.visits = visits;
            this.rank = rank;
            this.registrationDate = registrationDate;
            this.spacePublish = spacePublish;
            this.premium = premium;
        }

        public List<User> getUsers()
        {
            DBServices dbs = new DBServices();
            return dbs.readUsers();
        }
        public List<User> getUsersLastThirtyDays()
        {
            DBServices dbs = new DBServices();
            return dbs.readUsersFromLastThirtyDays();
        }
        public User getUser(int id)
        {
            DBServices dbs = new DBServices();
            return dbs.readUser(id);
        }
        public User getUser(string email)
        {
            DBServices dbs = new DBServices();
            return dbs.readUser(email);
        }

        public int updateStatus(int id)
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.updateUserStatus(id);
            return numAffected;
        }
        public int updateCancelPremium(int id)
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.updateCancelUserPremium(id);
            return numAffected;
        }

        public int insert()
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.insert(this);
            return numAffected;
        }
      

        public List<int> getFavourites(int id)
        {
            DBServices dbs = new DBServices();
            return dbs.readFavouritesSpaces(id);
        }

        public int updateUser(User user)
        {
            DBServices dbs = new DBServices();
            int numAffected = dbs.updateUserDetails(user);
            return numAffected;
        }

        public int deleteUser(int id)
        {
            DBServices dbs = new DBServices();
            int numEffected = dbs.deleteUser(id);
            return numEffected;
        }


    }
}