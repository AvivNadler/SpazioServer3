using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using SpazioServer.Models;
using System.Globalization;
using System.Reflection;


public class DBServices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBServices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    ////---------------------------------------------------------------------------------
    //// Create the SqlCommand
    ////---------------------------------------------------------------------------------
    public SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }



    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the USER functions ******
    //--------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------------
    // This method inserts a order to the Orders table 
    //--------------------------------------------------------------------------------------------------
    public int insert(User user)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(user);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(User user)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}','{4}','{5}',{6},{7},'{8}','{9}','{10}')", user.Email, user.Password, user.UserName,
            user.PhoneNumber, user.Photo, user.SpaceOwner, user.Visits, user.Rank, time.ToString(format),user.SpacePublish,user.Premium);
        String prefix = "INSERT INTO Users_2020" + "(Email,Password,UserName,PhoneNumber,Photo,SpaceOwner,visits,rank,RegisterationDate,SpacePublish,Premium) ";
        command = prefix + sb.ToString();

        return command;
    }
    //--------------------------------------------------------------------------------------------------
    // This method reads specific user by id parameter 
    //--------------------------------------------------------------------------------------------------
    public User readUser(int id)
    {
        User u = new User();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 Where Id=" + id.ToString();
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row


                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = dr["RegisterationDate"].ToString();
                u.SpacePublish = dr["SpacePublish"].ToString(); 
                u.Premium = Convert.ToBoolean(dr["Premium"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return u;
    }
    public User readUser(string email)
    {
        User u = new User();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 Where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row


                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = dr["RegisterationDate"].ToString();
                u.SpacePublish = dr["SpacePublish"].ToString();
                u.Premium = Convert.ToBoolean(dr["Premium"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return u;
    }
    //--------------------------------------------------------------------------------------------------
    // This method reads all the users from the users table 
    //--------------------------------------------------------------------------------------------------
    public bool userTypeCheckandUpdate(string email)
    {
        bool userType = false;
        User u = new User();
        int numEffected;
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 Where Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        if (!u.SpaceOwner)
        {
            userType = false;
            numEffected = updateUserType(email);
            if (numEffected == 1)
            {
                userType = true;
            }
        }
        return userType;
    }
    private int updateUserType(string email)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("SET SpaceOwner = {0} WHERE Email = '{1}'", 1, email);
        String prefix = "UPDATE Users_2020 ";
        command = prefix + sb.ToString();

        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(command, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }
    
    public int updateUserDetails(User user)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("SET Email='{0}',Password='{1}',PhoneNumber='{2}',Photo='{3}' WHERE Id={4}", user.Email, user.Password, user.PhoneNumber, user.Photo, user.Id.ToString());
        String prefix = "UPDATE Users_2020 ";
        command = prefix + sb.ToString();

        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(command, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }
    public int updateUserSpacePubish(string userEmail)
    {
        SqlConnection con;
        SqlCommand cmd;
        string format = "yyyy-MM-dd HH:mm:ss";
        string format2 = "yyyy-MM-dd";
        DateTime publishDate = DateTime.Now;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("SET SpacePublish='{0} WHERE Email='{1}' AND SpacePublish='0001-01-01'",  publishDate.ToString(format2), userEmail);
        String prefix = "UPDATE Users_2020 ";
        command = prefix + sb.ToString();

        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(command, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //set user premium column to true
    public int updateUserStatus(int id)
    {
        SqlConnection con;
        SqlCommand cmd;
        

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Set Premium = 'true' Where Id={0}",  id.ToString());
        String prefix = "UPDATE Users_2020 ";
        command = prefix + sb.ToString();

        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(command, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }


    //set user premium column to false

    public int updateCancelUserPremium(int id)
    {
        SqlConnection con;
        SqlCommand cmd;


        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Set Premium = 'false' Where Id={0}", id.ToString());
        String prefix = "UPDATE Users_2020 ";
        command = prefix + sb.ToString();

        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(command, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    public List<User> readUsers()
    {
        List<User> Users = new List<User>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                User u = new User();

                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = dr["RegisterationDate"].ToString();
                u.SpacePublish = dr["SpacePublish"].ToString();
                u.Premium = Convert.ToBoolean(dr["Premium"]);

                Users.Add(u);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Users;
    }

    //--------------------------------------------------------------------------------------------------
    // This method reads all favourites spaces id of specific user by id parameter 
    //--------------------------------------------------------------------------------------------------
    public List<int> readFavouritesSpaces(int id)
    {
        List<int> spacesId = new List<int>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT SpaceId FROM Favourites_2020 Where UserId=" + id;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row


                spacesId.Add(Convert.ToInt32(dr["SpaceId"]));



            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return spacesId;
    }

    //--------------------------------------------------------------------
    // Build the Delete command method String for Users table
    //--------------------------------------------------------------------
    private String BuildDeleteUserCommand(int id)
    {
        String command;
        command = "delete from Users_2020 where Id = " + id.ToString();
        return command;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Delete specific user by id parameter 
    //--------------------------------------------------------------------------------------------------
    public int deleteUser(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteUserCommand(id);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method reads all the new users from the lat 30 days, from the users table 
    //--------------------------------------------------------------------------------------------------
    public List<User> readUsersFromLastThirtyDays()
    {
        List<User> Users = new List<User>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Users_2020 WHERE datediff(DAY,RegistrationDate,getdate())<=30";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                User u = new User();

                u.Id = Convert.ToInt32(dr["Id"]);
                u.Email = (string)dr["Email"];
                u.Password = (string)dr["Password"];
                u.UserName = (string)dr["UserName"];
                u.PhoneNumber = (string)dr["PhoneNumber"];
                u.Photo = (string)dr["Photo"];
                u.SpaceOwner = Convert.ToBoolean(dr["SpaceOwner"]);
                u.Visits = Convert.ToInt32(dr["visits"]);
                u.Rank = Convert.ToDouble(dr["rank"]);
                u.RegistrationDate = dr["RegistrationDate"].ToString();
                u.SpacePublish = dr["SpacePublish"].ToString();
                u.Premium = Convert.ToBoolean(dr["Premium"]);

                Users.Add(u);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Users;
    }


    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the SPACE functions ******
    //--------------------------------------------------------------------------------------------------
    public List<Space> readSpaces()
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Spaces_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];

                if (dr["Latitude"] == DBNull.Value)
                { //in case there is not location
                    s.Latitude = 0;
                }
                else
                {
                    s.Latitude = Convert.ToDouble(dr["Latitude"]);
                }
                if (dr["Longitude"] == DBNull.Value)
                { //in case there is not location
                    s.Longitude = 0;
                }
                else
                {
                    s.Longitude = Convert.ToDouble(dr["Longitude"]);
                }


                if (dr["Rank"] == DBNull.Value)
                { //in case there is not ratings to space yet
                    s.Rank = 3.499;
                }
                else
                {
                    s.Rank = Convert.ToDouble(dr["Rank"]);
                }
                s.Uploadtime = dr["UploadDate"].ToString();

                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }
    public List<Space> readLastAddedSpaces()
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = @"SELECT TOP 5 Spaces_2020.SpaceId, SpaceName, Field, Price, City, Street, Number, Capabillity,
Bank, Branch, AccountNumber, Image1, Image2, Image3, Image4, Image5, FKEmail, Description, TermsOfUse, UploadDate,
AVG(Ratings_2020.TotalRating) as Rank,COUNT(Distinct Ratings_2020.Id) as RankCount ,COUNT(Distinct SpaceVisits_2020.Id) as Visits, Latitude, Longitude 
FROM Spaces_2020 left JOIN Ratings_2020 ON Ratings_2020.FKSpaceId = Spaces_2020.SpaceId
left JOIN SpaceVisits_2020 ON SpaceVisits_2020.SpaceId = Spaces_2020.SpaceId 
group by Spaces_2020.SpaceId,SpaceName,Field,Price,City,Street,Number,Capabillity,Bank,Branch,AccountNumber,Image1,Image2,Image3,Image4,Image5
,FKEmail,Description,TermsOfUse,UploadDate, Latitude, Longitude  
Order by UploadDate Desc;";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                if (dr["Latitude"] == DBNull.Value)
                { //in case there is not location
                    s.Latitude = 0;
                }
                else
                {
                    s.Latitude = Convert.ToDouble(dr["Latitude"]);
                }
                if (dr["Longitude"] == DBNull.Value)
                { //in case there is not location
                    s.Longitude = 0;
                }
                else
                {
                    s.Longitude = Convert.ToDouble(dr["Longitude"]);
                }
                if (dr["Rank"] == DBNull.Value)
                { //in case there is not ratings to space yet
                    s.Rank = 3.499;
                }
                else
                {
                    s.Rank = Convert.ToDouble(dr["Rank"]);
                }
                s.Uploadtime = dr["UploadDate"].ToString();

                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }
    public List<Space> readMySpaces(string userEmail)
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            //string selectSTR2 = "SELECT SpaceId, SpaceName, Field, Price, City, Street, Number, Capabillity, Bank, Branch, AccountNumber, Image1,
            //Image2, Image3, Image4, Image5, FKEmail, Description, TermsOfUse, UploadDate, AVG(Ratings_2020.TotalRating) as Rank FROM Spaces_2020
            //    left JOIN Ratings_2020 ON Ratings_2020.FKSpaceId = Spaces_2020.SpaceId Where FKEmail = '" + userEmail + "' 
            //   group by SpaceId,SpaceName,Field,Price,City,Street,Number,Capabillity,Bank,Branch,AccountNumber,Image1,Image2,Image3,Image4,
            // Image5,FKEmail,Description,TermsOfUse,UploadDate Order by UploadDate Desc";

            string selectSTR = @"SELECT Spaces_2020.SpaceId, SpaceName, Field, Price, City, Street, Number, Capabillity, Bank, Branch, AccountNumber,
Image1, Image2, Image3, Image4, Image5, FKEmail, Description, TermsOfUse, UploadDate, AVG(Ratings_2020.TotalRating) as Rank
,COUNT(Distinct Ratings_2020.Id) as RankCount ,COUNT(Distinct SpaceVisits_2020.Id) as Visits, Latitude, Longitude 
FROM Spaces_2020 left JOIN Ratings_2020 ON Ratings_2020.FKSpaceId = Spaces_2020.SpaceId left JOIN SpaceVisits_2020 ON SpaceVisits_2020.SpaceId = Spaces_2020.SpaceId 
Where FKEmail = '" + userEmail +
@"' group by Spaces_2020.SpaceId,SpaceName,Field,Price,City,Street,Number,Capabillity,Bank,Branch,AccountNumber,Image1,Image2,Image3,Image4,
Image5,FKEmail,Description,TermsOfUse,UploadDate, Latitude, Longitude 
Order by UploadDate Desc;";

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                if (dr["Latitude"] == DBNull.Value)
                { //in case there is not location
                    s.Latitude = 0;
                }
                else
                {
                    s.Latitude = Convert.ToDouble(dr["Latitude"]);
                }
                if (dr["Longitude"] == DBNull.Value)
                { //in case there is not location
                    s.Longitude = 0;
                }
                else
                {
                    s.Longitude = Convert.ToDouble(dr["Longitude"]);
                }
                if (dr["Rank"] == DBNull.Value)
                { //in case there is not ratings to space yet
                    s.Rank = 3.499;
                }
                else
                {
                    s.Rank = Convert.ToDouble(dr["Rank"]);
                }
                s.Uploadtime = dr["UploadDate"].ToString();

                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }

    private string isEmpty(string field)
    {
        string temp = field;
        if (temp == "" || temp == null)
            temp = "_%";
        return temp;
    }

    public Space readSpaceById(int id)
    {

        Space s = new Space();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string
            string command;
            sb.AppendFormat("WHERE Spaces_2020.SpaceId={0} group by Spaces_2020.SpaceId,SpaceName,Field,Price,City,Street," +
                "Number,Capabillity,Bank,Branch,AccountNumber,Image1,Image2,Image3,Image4,Image5,FKEmail,Description," +
                "TermsOfUse,UploadDate, Latitude, Longitude  Order by UploadDate Desc", id.ToString());
            String prefix = @"SELECT Spaces_2020.SpaceId, SpaceName, Field, Price, City, Street, Number, Capabillity,
Bank, Branch, AccountNumber, Image1, Image2, Image3, Image4, Image5, FKEmail, Description, TermsOfUse, UploadDate,
AVG(Ratings_2020.TotalRating) as Rank,COUNT(Distinct Ratings_2020.Id) as RankCount ,COUNT(Distinct SpaceVisits_2020.Id) as Visits, Latitude, Longitude 
FROM Spaces_2020 left JOIN Ratings_2020 ON Ratings_2020.FKSpaceId = Spaces_2020.SpaceId left JOIN SpaceVisits_2020 ON SpaceVisits_2020.SpaceId = Spaces_2020.SpaceId ";

            command = prefix + sb.ToString();

            string selectSTR = command;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                if (dr["Latitude"] == DBNull.Value)
                { //in case there is not location
                    s.Latitude = 0;
                }
                else
                {
                    s.Latitude = Convert.ToDouble(dr["Latitude"]);
                }
                if (dr["Longitude"] == DBNull.Value)
                { //in case there is not location
                    s.Longitude = 0;
                }
                else
                {
                    s.Longitude = Convert.ToDouble(dr["Longitude"]);
                }
                if (dr["Rank"] == DBNull.Value)
                { //in case there is not ratings to space yet
                    s.Rank = 3.499;
                }
                else
                {
                    s.Rank = Convert.ToDouble(dr["Rank"]);
                }
                s.Uploadtime = dr["UploadDate"].ToString();

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return s;
    }

    public List<Space> readSpacesBySearch(string field, string city, string street, string number)
    {

        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string
            string command;
            sb.AppendFormat("WHERE Field like '{0}' and City like '{1}' and  Street like '{2}'  and  Number like '{3}'  group by Spaces_2020.SpaceId,SpaceName,Field" +
                ",Price,City,Street,Number,Capabillity,Bank,Branch,AccountNumber,Image1,Image2,Image3,Image4,Image5,FKEmail,Description,TermsOfUse,UploadDate," +
                "Latitude,Longitude Order by Rank DESC, SpaceName ASC", isEmpty(field), isEmpty(city), isEmpty(street), isEmpty(number));
            String prefix = "SELECT Spaces_2020.SpaceId, SpaceName, Field, Price, City, Street, Number, Capabillity, Bank, Branch, AccountNumber, Image1," +
                " Image2, Image3, Image4, Image5, FKEmail, Description, TermsOfUse, UploadDate, AVG(Ratings_2020.TotalRating) as Rank" +
                ",COUNT(Distinct Ratings_2020.Id) as RankCount ,COUNT(Distinct SpaceVisits_2020.Id) as Visits,Latitude,Longitude" +
                " FROM Spaces_2020 left JOIN Ratings_2020 ON Ratings_2020.FKSpaceId = Spaces_2020.SpaceId " +
                "left JOIN SpaceVisits_2020 ON SpaceVisits_2020.SpaceId = Spaces_2020.SpaceId ";
            command = prefix + sb.ToString();

            string selectSTR = command;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];

                if (dr["Latitude"] == DBNull.Value)
                { //in case there is not location
                    s.Latitude = 0;
                }
                else
                {
                    s.Latitude = Convert.ToDouble(dr["Latitude"]);
                }
                if (dr["Longitude"] == DBNull.Value)
                { //in case there is not location
                    s.Longitude = 0;
                }
                else
                {
                    s.Longitude = Convert.ToDouble(dr["Longitude"]);
                }


                if (dr["Rank"] == DBNull.Value) { //in case there is not ratings to space yet
                    s.Rank = 3.499;
                }
                else { 
                s.Rank = Convert.ToDouble(dr["Rank"]);
                }
                s.Uploadtime = dr["UploadDate"].ToString();

                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }

    public List<Space> readSpacesFromLastThirtyDays()
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Spaces_2020 Where datediff(DAY,UploadDate,getdate())<=30 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                s.Rank = Convert.ToDouble(dr["Rank"]);
                s.Uploadtime = dr["UploadDate"].ToString();



                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }

    public int insert(Space space)
    {
        SqlConnection con;
        SqlCommand cmd;

        //DBServices dbs = new DBServices();

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(space);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        // the Id is made automatically in the sql so this data reader and the query above will help us get this new id
        int newSpaceInsertedId = -1;
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        while (dr.Read())
        {   // Read till the end of the data into a row

            newSpaceInsertedId = Convert.ToInt32(dr["SpaceId"]); // get the new space id
        }
        //f.SpaceId = newSpaceInsertedId;
        //e.SpaceId = newSpaceInsertedId;
        //a.SpaceId = newSpaceInsertedId;
        //dbs.insert(f);
        //dbs.insert(e);
        //dbs.insert(a);
        try
        {
            //int numEffected = cmd.ExecuteNonQuery();
            //return numEffected;
            return newSpaceInsertedId;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Space space)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}', '{1}' , {2}, '{3}','{4}', '{5}' , {6}, '{7}','{8}', '{9}' ,'{10}', '{11}'" +
            ",'{12}', '{13}' ,'{14}', '{15}', '{16}', '{17}', {18},'{19}',{20},{21})", space.Name, space.Field, space.Price,
            space.City, space.Street, space.Number, space.Capabillity, space.Bank, space.Branch, space.Imageurl1, space.Imageurl2,
            space.Imageurl3, space.Imageurl4, space.Imageurl5, space.AccountNumber, space.UserEmail, space.Description, space.TermsOfUse,
            space.Rank, time.ToString(format), space.Latitude, space.Longitude);
        String prefix = "INSERT INTO Spaces_2020" + "([SpaceName] ,[Field] ,[Price],[City],[Street] ,[Number],[Capabillity]" +
            " ,[Bank] ,[Branch]  ,[Image1]  ,[Image2],[Image3],[Image4],[Image5],[AccountNumber],[FKEmail],[Description]," +
            "[TermsOfUse],[Rank], [UploadDate],[Latitude], [Longitude]) OUTPUT Inserted.SpaceId ";
        command = prefix + sb.ToString();

        return command;
    }

    private String BuildDeleteSpaceCommand(int id)
    {
        String command;
        command = "delete from Spaces_2020 where SpaceId=" + id.ToString();
        return command;
    }

    public int deleteSpace(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteSpaceCommand(id);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Favourite functions ******
    //--------------------------------------------------------------------------------------------------
    public List<Favourite> readFavourites()
    {
        List<Favourite> favourites = new List<Favourite>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Favourites_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Favourite f = new Favourite();

                f.UserId = Convert.ToInt32(dr["UserId"]);
                f.SpaceId = Convert.ToInt32(dr["SpaceId"]);




                favourites.Add(f);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return favourites;
    }

    public int insert(Favourite favourite)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(favourite);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Favourite favourite)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1})", favourite.UserId, favourite.SpaceId);
        String prefix = "INSERT INTO Favourites_2020" + "(UserId,SpaceId)";
        command = prefix + sb.ToString();

        return command;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Delete specific favourite by id parameter 
    //--------------------------------------------------------------------------------------------------
    public int removeFavourite(int spaceId, int userId)
    {

        String command;
        command = "delete from Favourites_2020 where SpaceId = " + spaceId.ToString() + "AND UserId=" + userId.ToString();

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = command;      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Equipment functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(Equipment eq)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(eq);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Equipment eq)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}')", eq.Name, eq.SpaceId);
        String prefix = "INSERT INTO Equipment_2020" + "(EquipmentName,FKSpaceId) ";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Equipment> readEquipments()
    {
        List<Equipment> Equipments = new List<Equipment>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Equipment_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Equipment eq = new Equipment();

                eq.Id = Convert.ToInt32(dr["EquipmentId"]);
                eq.Name = (string)dr["EquipmentName"];
                eq.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);
                Equipments.Add(eq);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Equipments;
    }
    public List<Equipment> readEquipments(int spaceId)
    {
        List<Equipment> Equipments = new List<Equipment>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Equipment_2020 Where FKSpaceId=" + spaceId.ToString();
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Equipment eq = new Equipment();

                eq.Id = Convert.ToInt32(dr["EquipmentId"]);
                eq.Name = (string)dr["EquipmentName"];
                eq.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);
                Equipments.Add(eq);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Equipments;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Facility functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(Facility f)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(f);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }
    public int update(Facility f)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildUpdateCommand(f);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }
    private String BuildUpdateCommand(Facility f)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("SET Parking='{0}', Toilet='{1}', Kitchen='{2}', Intercom='{3}' ,Accessible='{4}'," +
            " AirCondition='{5}', WiFi='{6}' WHERE FKSpaceId={7}", f.Parking, f.Toilet, f.Kitchen, f.Intercom, f.Accessible, f.AirCondition, f.Wifi, f.SpaceId.ToString());
        String prefix = "UPDATE Facilities_2020 " ;
        command = prefix + sb.ToString();

        return command;
    }

    private String BuildInsertCommand(Facility f)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}', '{5}','{6}', '{7}')", f.Parking, f.Toilet, f.Kitchen, f.Intercom, f.Accessible, f.AirCondition, f.Wifi, f.SpaceId);
        String prefix = "INSERT INTO Facilities_2020" + "(Parking, Toilet, Kitchen, Intercom ,Accessible, AirCondition, WiFi, FKSpaceId)";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Facility> readFacilities()
    {
        List<Facility> Facilities = new List<Facility>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Facilities_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Facility f = new Facility();

                f.Id = Convert.ToInt32(dr["FacilityId"]);
                f.Parking = Convert.ToBoolean(dr["Parking"]);
                f.Toilet = Convert.ToBoolean(dr["Toilet"]);
                f.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                f.Intercom = Convert.ToBoolean(dr["Intercom"]);
                f.Accessible = Convert.ToBoolean(dr["Accessible"]);
                f.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                f.Wifi = Convert.ToBoolean(dr["WiFi"]);
                f.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);


                Facilities.Add(f);

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Facilities;
    }
    public Facility readFacilities(int spaceId)
    {
        SqlConnection con = null;
        Facility f = new Facility();

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Facilities_2020 where FKSpaceId=" + spaceId.ToString();
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                f.Id = Convert.ToInt32(dr["FacilityId"]);
                f.Parking = Convert.ToBoolean(dr["Parking"]);
                f.Toilet = Convert.ToBoolean(dr["Toilet"]);
                f.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                f.Intercom = Convert.ToBoolean(dr["Intercom"]);
                f.Accessible = Convert.ToBoolean(dr["Accessible"]);
                f.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                f.Wifi = Convert.ToBoolean(dr["WiFi"]);
                f.SpaceId = Convert.ToInt32(dr["FKSpaceId"]);

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return f;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Availability functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(Availability a)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(a);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Availability a)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}','{2}', '{3}','{4}', '{5}','{6}', '{7}')", a.Sunday, a.Monday, a.Tuesday, a.Wednesday, a.Thursday, a.Friday, a.Saturday, a.SpaceId);
        String prefix = "INSERT INTO Availability_2020" + "(Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday, FK_SpaceId)";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Availability> readAvailabilities()
    {
        List<Availability> Availabilities = new List<Availability>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Availability_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Availability a = new Availability();

                a.Id = Convert.ToInt32(dr["AvailabilityId"]);
                a.Sunday = (string)dr["Sunday"];
                a.Monday = (string)dr["Monday"];
                a.Tuesday = (string)dr["Tuesday"];
                a.Wednesday = (string)dr["Wednesday"];
                a.Thursday = (string)dr["Thursday"];
                a.Friday = (string)dr["Friday"];
                a.Saturday = (string)dr["Saturday"];
                a.SpaceId = Convert.ToInt32(dr["FK_SpaceId"]);

                Availabilities.Add(a);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Availabilities;
    }

    public Availability readAvailability(int id)
    {
        Availability a = new Availability();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Availability_2020 Where FK_SpaceId='" + id + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                a.Id = Convert.ToInt32(dr["AvailabilityId"]);
                a.Sunday = (string)dr["Sunday"];
                a.Monday = (string)dr["Monday"];
                a.Tuesday = (string)dr["Tuesday"];
                a.Wednesday = (string)dr["Wednesday"];
                a.Thursday = (string)dr["Thursday"];
                a.Friday = (string)dr["Friday"];
                a.Saturday = (string)dr["Saturday"];
                a.SpaceId = Convert.ToInt32(dr["FK_SpaceId"]);

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return a;
    }



    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Fields by Equipment functions ******
    //--------------------------------------------------------------------------------------------------
    public int insert(FieldEq fe)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(fe);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(FieldEq fe)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}','{2}')", fe.Id, fe.Field, fe.Name);
        String prefix = "INSERT INTO FieldEquipment_2020" + "(EquipmentId,Field,EquipmentName)";
        command = prefix + sb.ToString();

        return command;
    }

    public List<FieldEq> readFieldsEq()
    {
        List<FieldEq> FieldsEq = new List<FieldEq>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM FieldEquipment_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                FieldEq fe = new FieldEq();

                fe.Id = Convert.ToInt32(dr["EquipmentId"]);
                fe.Field = (string)dr["Field"];
                fe.Name = (string)dr["EquipmentName"];



                FieldsEq.Add(fe);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return FieldsEq;
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Admin functions ******
    //--------------------------------------------------------------------------------------------------

    public List<Admin> readAdmins()
    {
        List<Admin> admins = new List<Admin>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Admins_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Admin a = new Admin();

                a.Id = Convert.ToInt32(dr["ID"]);
                a.UserName = (string)dr["UserName"];
                a.Password = (string)dr["Password"];
                a.FirstName = (string)dr["FirstName"];
                a.LastName = (string)dr["LastName"];

                admins.Add(a);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return admins;
    }

    public Admin GetAdmin(string username)
    {

        SqlConnection con = null;
        Admin a = new Admin();
        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM Admins_2020 WHERE UserName= '" + username + "'";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                a.UserName = dr["UserName"].ToString();
                a.FirstName = dr["FirstName"].ToString();
                a.LastName = dr["LastName"].ToString();
            }

            return a;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public bool CheckAdmin(string username, string password)
    {
        bool userAdminInDB = false;
        SqlConnection con = null;
        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file
            string query = "select * from Admins_2020 where UserName ='" + username + "'";
            SqlCommand cmd = new SqlCommand(query, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                if (dr["Password"].ToString() == password)
                {
                    userAdminInDB = true;
                }
            }

            return userAdminInDB;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
    }

    //--------------------------------------------------------------------------------------------------
    //                    ****** This section include the Order functions ******
    //--------------------------------------------------------------------------------------------------
    //--------------------------------------------------------------------
    // Build the Insert command method String for Orders table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(Order order)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;
        // string timeInString = DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss");


        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1},'{2}','{3}','{4}','{5}', {6})", order.SpaceId.ToString(), order.UserId.ToString(),
            order.ReservationDate, order.StartHour, order.EndHour, time.ToString(format), order.Price);
        String prefix = "INSERT INTO Orders_2020 " + "(SpaceId, UserId, ReservationDate, StartHour, EndHour, OrderDate, Price) ";
        command = prefix + sb.ToString();

        return command;
    }
    //--------------------------------------------------------------------------------------------------
    // This method inserts a order to the Orders table 
    //--------------------------------------------------------------------------------------------------
    public int insert(Order order)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand2(order);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // This method reads all the orders from the Orders table 
    //--------------------------------------------------------------------------------------------------
    public List<Order> readOrders()
    {
        List<Order> orders = new List<Order>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Orders_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Order o = new Order();

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                string temp = dr["ReservationDate"].ToString();
                o.ReservationDate = temp.Split(' ')[0];
                o.StartHour = (string)dr["StartHour"];
                o.EndHour = (string)dr["EndHour"];
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = dr["OrderDate"].ToString();


                orders.Add(o);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return orders;
    }
    public List<Order> readOrdersByField(string field)
    {
        List<Order> orders = new List<Order>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = @"select [dbo].[Orders_2020].* 
from [dbo].[Orders_2020] 
inner join [dbo].[Spaces_2020] on 
[dbo].[Orders_2020].SpaceId = [dbo].[Spaces_2020].SpaceId
Where [dbo].[Spaces_2020].Field='" + field + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Order o = new Order();

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                string temp = dr["ReservationDate"].ToString();
                o.ReservationDate = temp.Split(' ')[0];
                o.StartHour = (string)dr["StartHour"];
                o.EndHour = (string)dr["EndHour"];
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = dr["OrderDate"].ToString();


                orders.Add(o);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return orders;
    }

    public List<Order> readOrdersOfSpace(int spaceId)
    {
        List<Order> orders = new List<Order>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Orders_2020 where SpaceId=" + spaceId.ToString();
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Order o = new Order();

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                string temp = dr["ReservationDate"].ToString();
                o.ReservationDate = temp.Split(' ')[0];
                o.StartHour = dr["StartHour"].ToString();
                o.EndHour = dr["EndHour"].ToString();
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = dr["OrderDate"].ToString();


                orders.Add(o);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return orders;
    }

    public Order readOrder(int id)
    {
        Order o = new Order();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Orders_2020 Where OrderId='" + id + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                string temp = dr["ReservationDate"].ToString();
                o.ReservationDate = temp.Split(' ')[0];
                o.StartHour = (string)dr["StartHour"];
                o.EndHour = (string)dr["EndHour"];
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = dr["OrderDate"].ToString();

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return o;
    }
    public List<OrderData> readOrdersDataBySpaceId(int spaceid)
    {
        List<OrderData> orders = new List<OrderData>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM Orders_2020 Where SpaceId=" + spaceid.ToString() ;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Order o = new Order();
                OrderData od = new OrderData();

                o.OrderId = Convert.ToInt32(dr["OrderId"]);
                o.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                o.UserId = Convert.ToInt32(dr["UserId"]);
                string temp = dr["ReservationDate"].ToString();
                o.ReservationDate = temp.Split(' ')[0];
                o.StartHour = (string)dr["StartHour"];
                o.EndHour = (string)dr["EndHour"];
                o.Price = Convert.ToDouble(dr["Price"]);
                o.OrderDate = dr["OrderDate"].ToString();

                od.Order = o;

                od.User = this.readUser(o.UserId);

                

                orders.Add(od);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return orders;
    }



    private String BuildInsertCommand2(Order order)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        string format = "yyyy-MM-dd HH:mm:ss";
        string format2 = "yyyy-MM-dd";
        DateTime time = DateTime.Now;
        DateTime reservation = new DateTime(Convert.ToInt32(order.ReservationDate.Split('/')[2]), Convert.ToInt32(order.ReservationDate.Split('/')[1]), Convert.ToInt32(order.ReservationDate.Split('/')[0]));
        // string timeInString = DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss");


        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1},'{2}','{3}','{4}','{5}', {6})", order.SpaceId.ToString(), order.UserId.ToString(), reservation.ToString(format2), order.StartHour, order.EndHour, time.ToString(format), order.Price);
        String prefix = "INSERT INTO Orders_2020 " + "(SpaceId, UserId, ReservationDate, StartHour, EndHour, OrderDate, Price) ";
        command = prefix + sb.ToString();



        //to get the number of week (week starts on monday)
        CultureInfo myCI = new CultureInfo("en-US");
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;


        RealAvailability ra = new RealAvailability(0,reservation.Year,cal.GetWeekOfYear(reservation,dfi.CalendarWeekRule,dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate,reservation,"00","00",order.SpaceId);
        RealAvailability ra2 = new RealAvailability(0, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, "00", "00", order.SpaceId);

        Availability a = readAvailability(order.SpaceId);

        List<RealAvailability> availabilityList = readRealAvailbility(order.SpaceId, reservation.ToString(format2));


        List<string> reala = new List<string>();

        List<string> reala2 = new List<string>();

        //RealAvailability newra = ra;
        //RealAvailability newra2 = ra2;

        List<WeekAvailability> list = readWeekAvailbility(order.SpaceId, reservation.DayOfWeek.ToString());

        
        
            int index = 0;
            bool isBetween = false;

        foreach (WeekAvailability item in list)
        {
            DateTime startdateweek = new DateTime(reservation.Year, reservation.Month, reservation.Day, Int32.Parse(item.StartTime.Split(':')[0]), Int32.Parse(item.StartTime.Split(':')[1]), 0);
            DateTime enddateweek = new DateTime(reservation.Year, reservation.Month, reservation.Day, Int32.Parse(item.EndTime.Split(':')[0]), Int32.Parse(item.EndTime.Split(':')[1]), 0);
            isBetween = false;
            foreach (RealAvailability item2 in availabilityList)
            {
                DateTime startdatereal = new DateTime(reservation.Year, reservation.Month, reservation.Day, Int32.Parse(item2.StartTime.Split(':')[0]), Int32.Parse(item2.StartTime.Split(':')[1]), 0);
                DateTime enddatereal = new DateTime(reservation.Year, reservation.Month, reservation.Day, Int32.Parse(item2.EndTime.Split(':')[0]), Int32.Parse(item2.EndTime.Split(':')[1]), 0);
                if (startdatereal >= startdateweek && enddatereal <= enddateweek)
                {
                    isBetween = true;
                }
            }
            if (isBetween == false && compareTimes(item.StartTime, item.EndTime, order).Count != 0)
            {
                reala = compareTimes(item.StartTime, item.EndTime, order);

                if (reala.Count == 1 )
                {
                    RealAvailability newra = new RealAvailability(0, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, reala[0].Split('-')[0], reala[0].Split('-')[1], order.SpaceId);
                    
                        insert(newra);
                }
                else if (reala.Count == 2)
                {
                    //newra insert all the details expect start ite and endtime
                    //newra2.StartTime = reala[1].Split('-')[0];
                    //newra2.EndTime = reala[1].Split('-')[1];
                    RealAvailability newra = new RealAvailability(0, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, reala[0].Split('-')[0], reala[0].Split('-')[1], order.SpaceId);

                    insert(newra);
                    RealAvailability newra2 = new RealAvailability(0, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, reala[1].Split('-')[0], reala[1].Split('-')[1], order.SpaceId);

                    insert(newra2);
                }
            }
        }
            


            foreach (RealAvailability item3 in availabilityList)
            {
                 

                if (compareTimes(item3.StartTime, item3.EndTime, order).Count != 0 )
                {
                    reala2 = compareTimes(item3.StartTime, item3.EndTime, order);
                    index = item3.Id;

                    if (reala2.Count == 1)
                    {

                        RealAvailability newra = new RealAvailability(index, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, reala2[0].Split('-')[0], reala2[0].Split('-')[1], order.SpaceId);
                        if (newra.StartTime == newra.EndTime )
                        {
                            DeleteAvailability(newra);
                        }
                        else
                        {
                            UpdateAvailability(newra);
                        }

                    }
                    // or insert and update the times of the change ra ??
                    else if (reala2.Count == 2)
                    {
                        //newra insert all the details expect start ite and endtime
                        //newra2.StartTime = reala[1].Split('-')[0];
                        //newra2.EndTime = reala[1].Split('-')[1];
                        RealAvailability newra = new RealAvailability(index, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, reala2[0].Split('-')[0], reala2[0].Split('-')[1], order.SpaceId);

                        UpdateAvailability(newra);

                        RealAvailability newra2 = new RealAvailability(0, reservation.Year, cal.GetWeekOfYear(reservation, dfi.CalendarWeekRule, dfi.FirstDayOfWeek), reservation.DayOfWeek.ToString(), order.ReservationDate, reservation, reala2[1].Split('-')[0], reala2[1].Split('-')[1], order.SpaceId);

                        insert(newra2);

                    }
                }

            }
            //newra insert all the details expect start ite and endtime
            //newra.StartTime = reala[0].Split('-')[0];
            //newra.EndTime = reala[0].Split('-')[1];
          
        

        


        //Availability a = readAvailability(order.SpaceId,order.ReservationDate.ToString("dddd"));



        // DateTime dt = new DateTime(Int32.Parse(order.ReservationDate.Split('/')[2]), Int32.Parse(order.ReservationDate.Split('/')[1]), Int32.Parse(order.ReservationDate.Split('/')[0]));
        //string wday = dt.ToString("dddd");
        // Availability a = readAvailability(order.SpaceId);

        // start hour = a.wday.Split('-')[0]  
        // end hour = a.wday.Split('-')[1] 
        // checking if space is exsist in real availabilty with the same day of wday
        // if its exsist calculate the avialable hours
        // else check if the hours is between one of the real avialabilitis
        //DateTime.Now.ToString("HH:mm")	
        //DateTime dateTime = DateTime.ParseExact(time, "HH:mm:ss",
        //                                         CultureInfo.InvariantCulture);
       


        return command;
    }


    private int UpdateAvailability(RealAvailability r)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        String command;

        StringBuilder sb = new StringBuilder();

        // use a string builder to create the dynamic string
        sb.AppendFormat("SET [Year]={1}, [WeekNumber]={2}, [Week_Day]='{3}', [StartTime]='{4}', [EndTime]='{5}', [FkSpaceId]={6} WHERE Id={0} ", r.Id.ToString(), r.Year.ToString(), r.WeekNumber.ToString(), r.Day, r.StartTime, r.EndTime, r.FkSpaceId.ToString());
        String prefix = "UPDATE RealAvailability_2020 ";
        command = prefix + sb.ToString();


        String cStr = command;      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    private int DeleteAvailability(RealAvailability r)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        String command;

        StringBuilder sb = new StringBuilder();

        // use a string builder to create the dynamic string
        sb.AppendFormat("WHERE Id={0} ", r.Id.ToString());
        String prefix = "DELETE FROM RealAvailability_2020 ";
        command = prefix + sb.ToString();


        String cStr = command;      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    private List<string> compareTimes(string rStart, string rEnd, Order o)
    {
        //date send from server in format of dd/mm/yyyy
        DateTime resdate = new DateTime(Convert.ToInt32(o.ReservationDate.Split('/')[2]), Convert.ToInt32(o.ReservationDate.Split('/')[1]), Convert.ToInt32(o.ReservationDate.Split('/')[0]));

        DateTime orderStart = new DateTime(resdate.Year, resdate.Month, resdate.Day, Int32.Parse(o.StartHour.Split(':')[0]), Int32.Parse(o.StartHour.Split(':')[1]), 00);
        DateTime orderEnd = new DateTime(resdate.Year, resdate.Month, resdate.Day, Int32.Parse(o.EndHour.Split(':')[0]), Int32.Parse(o.EndHour.Split(':')[1]), 00);
        DateTime rangeStart = new DateTime(resdate.Year, resdate.Month, resdate.Day, Int32.Parse(rStart.Split(':')[0]), Int32.Parse(rStart.Split(':')[1]), 00);
        DateTime rangeEnd = new DateTime(resdate.Year, resdate.Month, resdate.Day, Int32.Parse(rEnd.Split(':')[0]), Int32.Parse(rEnd.Split(':')[1]), 00);

        //TimeSpan orderStart = new TimeSpan(Int32.Parse(o.StartHour.Split(':')[0]), Int32.Parse(o.StartHour.Split(':')[1]), 00);
        //TimeSpan orderEnd = new TimeSpan(Int32.Parse(o.EndHour.Split(':')[0]), Int32.Parse(o.EndHour.Split(':')[1]), 00);
        //TimeSpan rangeStart = new TimeSpan(Int32.Parse(rStart.Split(':')[0]), Int32.Parse(rStart.Split(':')[1]), 00);
        //TimeSpan rangeEnd = new TimeSpan(Int32.Parse(rEnd.Split(':')[0]), Int32.Parse(rEnd.Split(':')[1]), 00);
        RealAvailability ra = new RealAvailability();

        List<string> avaList = new List<string>();
        string format = "HH:mm:ss";
        // another option maybe better List<RealAvailability> avaList = new List<RealAvailability>();

        if (rangeStart.CompareTo(orderStart) == 0 && rangeEnd.CompareTo(orderEnd) == 0)
        {
            ra.StartTime = rangeStart.ToString(format);
            ra.EndTime = rangeStart.ToString(format);

            avaList.Add(ra.StartTime.ToString() + "-" + ra.EndTime.ToString());
        }
        else if (rangeStart.CompareTo(orderStart) == 0 && rangeEnd.CompareTo(orderEnd) == 1)
        {
            ra.StartTime = orderEnd.ToString(format);
            ra.EndTime = rangeEnd.ToString(format);
            avaList.Add(ra.StartTime.ToString() + "-" + ra.EndTime.ToString());

        }
        else if (rangeStart.CompareTo(orderStart) == -1 && rangeEnd.CompareTo(orderEnd) == 0)
        {
            ra.StartTime = rangeStart.ToString(format);
            ra.EndTime = orderStart.ToString(format);
            avaList.Add(ra.StartTime.ToString() + "-" + ra.EndTime.ToString());

        }
        else if(rangeStart.CompareTo(orderStart) == -1 && rangeEnd.CompareTo(orderEnd) == 1)
        {
            ra.StartTime = rangeStart.ToString(format);
            ra.EndTime = orderStart.ToString(format);
            avaList.Add(ra.StartTime.ToString() + "-" + ra.EndTime.ToString());
            RealAvailability ra2 = new RealAvailability();
            ra2.StartTime = orderEnd.ToString(format);
            ra2.EndTime = rangeEnd.ToString(format);

            avaList.Add(ra2.StartTime.ToString() + "-" + ra2.EndTime.ToString());
        }

        return avaList;
    }

    public List<Space> readAllSpaces()
    {
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = @"SELECT Spaces_2020.SpaceId, SpaceName, Field, Price, City, Street, Number, Capabillity, Bank,
Branch, AccountNumber, Image1, Image2, Image3, Image4, Image5, FKEmail, Description, TermsOfUse, UploadDate,
AVG(Ratings_2020.TotalRating) as Rank,COUNT(Distinct Ratings_2020.Id) as RankCount ,COUNT(Distinct SpaceVisits_2020.Id) as Visits, Latitude, Longitude 
FROM Spaces_2020 left JOIN Ratings_2020 ON Ratings_2020.FKSpaceId = Spaces_2020.SpaceId 
left JOIN SpaceVisits_2020 ON SpaceVisits_2020.SpaceId = Spaces_2020.SpaceId group by Spaces_2020.SpaceId,SpaceName,Field,Price,City,Street,Number,
Capabillity,Bank,Branch,AccountNumber,Image1,Image2,Image3,Image4,Image5,FKEmail,Description,TermsOfUse,UploadDate, Latitude, Longitude 
Order by UploadDate Desc;";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Space s = new Space();

                s.Id = Convert.ToInt32(dr["SpaceId"]);
                s.Name = (string)dr["SpaceName"];
                s.Field = (string)dr["Field"];
                s.Price = Convert.ToDouble(dr["Price"]);
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.Capabillity = Convert.ToInt32(dr["Capabillity"]);
                s.Bank = (string)dr["Bank"];
                s.Branch = (string)dr["Branch"];
                s.AccountNumber = (string)dr["AccountNumber"];
                s.Imageurl1 = (string)dr["Image1"];
                s.Imageurl2 = (string)dr["Image2"];
                s.Imageurl3 = (string)dr["Image3"];
                s.Imageurl4 = (string)dr["Image4"];
                s.Imageurl5 = (string)dr["Image5"];
                s.UserEmail = (string)dr["FKEmail"];
                s.Description = (string)dr["Description"];
                s.TermsOfUse = (string)dr["TermsOfUse"];
                if (dr["Latitude"] == DBNull.Value)
                { //in case there is not location
                    s.Latitude = 0;
                }
                else
                {
                    s.Latitude = Convert.ToDouble(dr["Latitude"]);
                }
                if (dr["Longitude"] == DBNull.Value)
                { //in case there is not location
                    s.Longitude = 0;
                }
                else
                {
                    s.Longitude = Convert.ToDouble(dr["Longitude"]);
                }
                if (dr["Rank"] == DBNull.Value)
                { //in case there is not ratings to space yet
                    s.Rank = 3.499;
                }
                else
                {
                    s.Rank = Convert.ToDouble(dr["Rank"]);
                }
                s.Uploadtime = dr["UploadDate"].ToString();

                Spaces.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Spaces;
    }

    public List<string> readAllAvailbilities(int spaceId, string date)
    {
        List<string> allav = new List<string>();
        List<string> allav2 = new List<string>();

        SqlConnection con = null;
        DateTime date2 = new DateTime();
        string format2 = "yyyy-MM-dd";
        string searchDate;

        if (date!=null && date!="")
        {
            date2 = new DateTime(Convert.ToInt32(date.Split('/')[2]), Convert.ToInt32(date.Split('/')[1]), Convert.ToInt32(date.Split('/')[0]));
            searchDate = date2.ToString(format2);
        }
        else
        {
            DateTime now = DateTime.Now;
            searchDate = now.ToString(format2);
        }

       
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
             
            string selectSTR = "SELECT * FROM RealAvailability_2020 WHERE FkSpaceId=" + spaceId.ToString() + " AND AvailabilityDate='" + searchDate + "' order by StartTime";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                RealAvailability reAv = new RealAvailability();

                reAv.Id = Convert.ToInt32(dr["Id"]);
                reAv.Year = Convert.ToInt32(dr["Year"]);
                reAv.WeekNumber = Convert.ToInt32(dr["WeekNumber"]);
                reAv.Day = (string)dr["Week_Day"];
                reAv.StartTime = dr["StartTime"].ToString(); 
                reAv.EndTime = dr["EndTime"].ToString();
                reAv.FkSpaceId = Convert.ToInt32(dr["FkSpaceId"]);


                string temp = dr["AvailabilityDate"].ToString();
                string tempdate = temp.Split(' ')[0];
                string day = tempdate.Split('/')[0];
                string month = tempdate.Split('/')[1];
                string year = tempdate.Split('/')[2];
                reAv.Date = temp;
                //reAv.Date2 = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));


                allav.Add(reAv.StartTime + "-" + reAv.EndTime);
                if (reAv.StartTime != reAv.EndTime)
                    allav2.Add(reAv.StartTime + "-" + reAv.EndTime);
            }
          /*  if(allav.Count == 0)
            {

                WeekAvailability w = readWeekAvailbility2(spaceId, date2.DayOfWeek.ToString()); //to get they day eg. Sunday
                allav.Add(w.StartTime + "-" + w.EndTime);
            }*/
            List<WeekAvailability> list = readWeekAvailbility(spaceId, date2.DayOfWeek.ToString());
            bool isBetween = false;
            foreach (WeekAvailability item in list)
            {
                isBetween = false;
                for(int i=0 ; i<allav.Count ; i++)
                {
                    DateTime startdatereal = new DateTime(date2.Year, date2.Month, date2.Day, Int32.Parse(allav[i].Split(':')[0]), Int32.Parse(allav[i].Split(':')[1]), 0);
                    DateTime enddatereal = new DateTime(date2.Year, date2.Month, date2.Day, Int32.Parse(allav[i].Split('-')[1].Split(':')[0]), Int32.Parse(allav[i].Split('-')[1].Split(':')[1]), 0);
                    DateTime startdateweek = new DateTime(date2.Year, date2.Month, date2.Day, Int32.Parse(item.StartTime.Split(':')[0]), Int32.Parse(item.StartTime.Split(':')[1]), 0);
                    DateTime enddateweek = new DateTime(date2.Year, date2.Month, date2.Day, Int32.Parse(item.EndTime.Split(':')[0]), Int32.Parse(item.EndTime.Split(':')[1]), 0);

                    if (startdatereal>=startdateweek && enddatereal<=enddateweek)
                    {
                        isBetween = true;
                    }
                }
                if(isBetween == false)
                {
                    allav2.Add(item.StartTime + "-" + item.EndTime);
                }
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return allav2;
    }
    public List<RealAvailability> readRealAvailbility(int spaceId, string date)
    {
        List<RealAvailability> ra = new List<RealAvailability>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM RealAvailability_2020 WHERE FkSpaceId=" + spaceId.ToString() + " AND AvailabilityDate='" + date + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                RealAvailability reAv = new RealAvailability();

                reAv.Id = Convert.ToInt32(dr["Id"]);
                reAv.Year = Convert.ToInt32(dr["Year"]);
                reAv.WeekNumber = Convert.ToInt32(dr["WeekNumber"]);
                reAv.Day = (string)dr["Week_Day"];
                reAv.StartTime = dr["StartTime"].ToString();
                reAv.EndTime = dr["EndTime"].ToString();
                reAv.FkSpaceId = Convert.ToInt32(dr["FkSpaceId"]);

                string temp = dr["AvailabilityDate"].ToString();
                string temp2 = temp.Split(' ')[0];
                string day = temp2.Split('/')[0];
                string month = temp2.Split('/')[1];
                string year = temp2.Split('/')[2];
                reAv.Date = temp;
                //reAv.Date2 = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));

                ra.Add(reAv);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return ra;
    }

    private String BuildInsertCommand(RealAvailability r)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        string format = "yyyy-MM-dd";
        //DateTime time = DateTime.Now;
        // string timeInString = DateTime.Now.ToString("YYYY-MM-DD hh:mm:ss");


        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1},'{2}','{3}','{4}',{5}, '{6}')", r.Year.ToString(), r.WeekNumber.ToString(), r.Day, r.StartTime, r.EndTime, r.FkSpaceId.ToString(), r.Date2.ToString(format));
        String prefix = "INSERT INTO RealAvailability_2020 " + "([Year], [WeekNumber], [Week_Day], [StartTime], [EndTime], [FkSpaceId], [AvailabilityDate]) ";
        command = prefix + sb.ToString();

        return command;
    }
    //--------------------------------------------------------------------------------------------------
    // This method inserts a order to the Orders table 
    //--------------------------------------------------------------------------------------------------
    public int insert(RealAvailability r)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(r);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public Country getCountryByName(string name)
    {
        //List<Country> countriesList = new List<Country>();
        Country c = new Country();
        SqlConnection con = null;

        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM Countries_2020 WHERE cname='" + name + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end



            //while (dr.Read())
            //{   // Read till the end of the data into a row
            if (dr.Read())
            {
                c.Id = Convert.ToInt32(dr["id"]);
                c.Name = (string)dr["cname"];
                c.Lang = (string)dr["lang"];
                c.Continent = (string)dr["continent"];
                return c;
            }
            else
            {
                return null;
            }




            //}


        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public List<Country> getAllCountries()
    {
        List<Country> countriesList = new List<Country>();
        SqlConnection con = null;

        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM Countries_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Country c = new Country();

                c.Id = Convert.ToInt32(dr["id"]);
                c.Name = (string)dr["cname"];
                c.Lang = (string)dr["lang"];
                c.Continent = (string)dr["continent"];
                countriesList.Add(c);
            }

            return countriesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public List<Country> getByContinent(string name)
    {
        List<Country> countriesList = new List<Country>();
        SqlConnection con = null;

        try
        {
            con = connect("database"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM Countries_2020 WHERE continent='" + name + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Country c = new Country();

                c.Id = Convert.ToInt32(dr["id"]);
                c.Name = (string)dr["cname"];
                c.Lang = (string)dr["lang"];
                c.Continent = (string)dr["continent"];
                countriesList.Add(c);
            }

            return countriesList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }
    public DBServices readCountries()
    {
        SqlConnection con = null;
        try
        {
            con = connect("database");
            da = new SqlDataAdapter("select * from Countries_2020", con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dt = ds.Tables[0];
        }

        catch (Exception ex)
        {
            // write errors to log file
            // try to handle the error
            throw ex;
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

        return this;

    }

    public void update()
    {
        da.Update(dt);
    }
    //--------------------------------------------------------------------------------------------------
    // This method inserts a order to the WeekAvailability table 
    //--------------------------------------------------------------------------------------------------
    public List<WeekAvailability> readWeekAvailbilitiesById(int id)
    {
        List<WeekAvailability> list = new List<WeekAvailability>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "select * from[dbo].[WeekAvailablity_2020] Where [FkSpaceId] =" + id.ToString() + " Order by[Day], [StartTime]";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                WeekAvailability wa = new WeekAvailability();

                wa.Id = Convert.ToInt32(dr["Id"]);
                wa.Day = (string)dr["Day"];
                wa.StartTime = dr["StartTime"].ToString();
                wa.EndTime = dr["EndTime"].ToString();
                wa.FkSpaceId = Convert.ToInt32(dr["FkSpaceId"]);

                list.Add(wa);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }
    public List<WeekAvailability> readWeekAvailbilities()
    {
        List<WeekAvailability> list = new List<WeekAvailability>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            string selectSTR = "SELECT * FROM WeekAvailablity_2020";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                WeekAvailability wa = new WeekAvailability();

                wa.Id = Convert.ToInt32(dr["Id"]);
                wa.Day = (string)dr["Day"];
                wa.StartTime = dr["StartTime"].ToString();
                wa.EndTime = dr["EndTime"].ToString();
                wa.FkSpaceId = Convert.ToInt32(dr["FkSpaceId"]);

                list.Add(wa);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }


    //list of Availbilitie of Specific day like sunday 16:00-18:00 and 22:00-23:00  
    public List<WeekAvailability> readWeekAvailbility(int spaceId, string day)
    {
        List<WeekAvailability> list = new List<WeekAvailability>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            sb.AppendFormat("WHERE FkSpaceId={0} AND Day='{1}'", spaceId.ToString(), day);


            string selectSTR = "SELECT * FROM WeekAvailablity_2020 " + sb;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                WeekAvailability wa = new WeekAvailability();

                wa.Id = Convert.ToInt32(dr["Id"]);
                wa.Day = (string)dr["Day"];
                wa.StartTime = dr["StartTime"].ToString();
                wa.EndTime = dr["EndTime"].ToString();
                wa.FkSpaceId = Convert.ToInt32(dr["FkSpaceId"]);
                list.Add(wa);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    // read one availabiliy per day of specific day (there should be antother function for list of availabilities per day)
    public WeekAvailability readWeekAvailbility2(int spaceId, string day)
    {
        WeekAvailability wa = new WeekAvailability();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            sb.AppendFormat("WHERE FkSpaceId={0} AND Day='{1}'", spaceId.ToString(), day);


            string selectSTR = "SELECT * FROM WeekAvailablity_2020 " + sb;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                wa.Id = Convert.ToInt32(dr["Id"]);
                wa.Day = (string)dr["Day"];
                wa.StartTime = dr["StartTime"].ToString();
                wa.EndTime = dr["EndTime"].ToString();
                wa.FkSpaceId = Convert.ToInt32(dr["FkSpaceId"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return wa;
    }
    private String BuildInsertCommand(WeekAvailability wa)
    {
        String command;

        StringBuilder sb = new StringBuilder();

        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}', '{1}', '{2}', {3})", wa.Day, wa.StartTime, wa.EndTime, wa.FkSpaceId.ToString());
        String prefix = "INSERT INTO WeekAvailablity_2020 " + "([Day], [StartTime], [EndTime], [FkSpaceId]) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int insert(WeekAvailability wa)
    {

        SqlConnection con = null ;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(wa);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public DBServices readRealAvailabilities()
    {
        SqlConnection con = null;
        try
        {
            con = connect("database");
            da = new SqlDataAdapter("select * from [RealAvailability_2020]", con);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dt = ds.Tables[0];
        }

        catch (Exception ex)
        {
            // write errors to log file
            // try to handle the error
            throw ex;
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

        return this;

    }

    public List<Search> readSearches()
    {
        List<Search> list = new List<Search>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM Searches_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Search s = new Search();

                s.Id = Convert.ToInt32(dr["SearchId"]);
                s.Field = (string)dr["Field"];
                s.Location = (string)dr["Location"];
                s.Time = (string)dr["Time"];
                s.City = (string)dr["City"];
                s.Street = (string)dr["Street"];
                s.Number = (string)dr["Number"];
                s.InputDate = dr["InputDate"].ToString().Split(' ')[0];
                s.SearchDate = dr["SearchDate"].ToString();
                s.UserId = Convert.ToInt32(dr["FkUserId"]);


                list.Add(s);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }
    public int insert(Search s)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(s);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(Search s)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        string format = "yyyy-MM-dd HH:mm:ss";
        string format2 = "yyyy-MM-dd";
        DateTime now = DateTime.Now;
        DateTime inputdate = new DateTime(Convert.ToInt32(s.InputDate.Split('/')[2]), Convert.ToInt32(s.InputDate.Split('/')[1]), Convert.ToInt32(s.InputDate.Split('/')[0]));


        sb.AppendFormat("Values('{0}', '{1}' ,'{2}', '{3}','{4}','{5}','{6}','{7}',{8})", s.Field, s.Location, s.Time, s.City, s.Street, s.Number, inputdate.ToString(format2), now.ToString(format), s.UserId.ToString());
        String prefix = "INSERT INTO Searches_2020" + "(Field,Location,Time,City,Street,Number,InputDate,SearchDate,FkUserId) ";
        command = prefix + sb.ToString();

        return command;
    }

    public int insert(Rating r)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(r);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(Rating r)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //string format = "yyyy-MM-dd HH:mm:ss";
        //double totalrating = (r.Attitude + r.Cleanliness + r.EquipmentQuality + r.FacilityQualiy + r.Authenticity) / 5.0;

        sb.AppendFormat("Values({0}, {1},{2}, {3},{4},{5},{6},{7})", r.Attitude.ToString(), r.Cleanliness.ToString(), r.EquipmentQuality.ToString(),
            r.FacilityQualiy.ToString(), r.Authenticity.ToString(), r.TotalRating.ToString(), r.FKSpaceId.ToString(), r.FKUserId.ToString());
        String prefix = "INSERT INTO Ratings_2020" + "(Attitude, Cleanliness, EquipmentQuality, FacilityQualiy, Authenticity, TotalRating,FKSpaceId,FKUserId) ";
        command = prefix + sb.ToString();

        return command;
    }

    public List<Rating> readRatings()
    {
        List<Rating> list = new List<Rating>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM Ratings_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Rating r = new Rating();

                r.Id = Convert.ToInt32(dr["Id"]);
                r.Attitude = Convert.ToInt32(dr["Attitude"]);
                r.Cleanliness = Convert.ToInt32(dr["Cleanliness"]);
                r.EquipmentQuality = Convert.ToInt32(dr["EquipmentQuality"]);
                r.FacilityQualiy = Convert.ToInt32(dr["FacilityQualiy"]);
                r.Authenticity = Convert.ToInt32(dr["Authenticity"]);
                r.TotalRating = Convert.ToDouble(dr["TotalRating"]);
                r.FKSpaceId = Convert.ToInt32(dr["FKSpaceId"]);
                r.FKUserId = Convert.ToInt32(dr["FKUserId"]); 
                list.Add(r);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    public List<ArtFilter> readArtFilters()
    {
        List<ArtFilter> list = new List<ArtFilter>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM ArtFilters_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                ArtFilter af = new ArtFilter();

                af.Id = Convert.ToInt32(dr["Id"]);
                af.Field = (string)dr["Field"];
                af.Rating = Convert.ToDouble(dr["Rating"]);
                af.MinPrice = Convert.ToInt32(dr["minPrice"]); 
                af.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                af.MaxDistance = Convert.ToInt32(dr["MaxDistance"]) ;
                af.StartTime = dr["StartTime"].ToString() ;
                af.EndTime = dr["EndTime"].ToString(); 
                af.Parking = Convert.ToBoolean(dr["Parking"]); 
                af.Toilet= Convert.ToBoolean(dr["Toilet"]); 
                af.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                af.Intercom = Convert.ToBoolean(dr["Intercom"]);
                af.Accessible = Convert.ToBoolean(dr["Accessible"]) ;
                af.AirCondition = Convert.ToBoolean(dr["AirCondition"]) ;
                af.WiFi = Convert.ToBoolean(dr["WiFi"]); 
                af.Canvas = Convert.ToBoolean(dr["Canvas"]); 
                af.GreenScreen = Convert.ToBoolean(dr["GreenScreen"]); 
                af.PottersWheel = Convert.ToBoolean(dr["PottersWheel"]) ;
                af.Guitar = Convert.ToBoolean(dr["Guitar"]);
                af.Drum = Convert.ToBoolean(dr["Drum"]);
                af.Speaker = Convert.ToBoolean(dr["Speaker"]); 
                af.UserId = Convert.ToInt32(dr["FkUserId"]); 
                af.Date = dr["FilterDate"].ToString();
                af.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                af.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);

                list.Add(af);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    public int insert(ArtFilter af)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(af);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(ArtFilter af)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //string format = "yyyy-MM-dd HH:mm:ss";
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}',{1}, {2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}',{20},'{21}',{22},{23})", af.Field,af.Rating.ToString(),af.MinPrice.ToString(), af.MaxPrice.ToString(),af.MaxDistance.ToString(),af.StartTime, af.EndTime,af.Parking,af.Toilet,af.Kitchen,af.Intercom,af.Accessible,af.AirCondition,af.WiFi,af.Canvas,af.GreenScreen,af.PottersWheel,af.Guitar,af.Drum,af.Speaker,af.UserId.ToString(),time.ToString(format),af.MinCapacity.ToString(),af.MaxCapacity.ToString());
        String prefix = "INSERT INTO ArtFilters_2020 " + "([Field],[Rating],[minPrice],[maxPrice],[MaxDistance],[StartTime],[EndTime],[Parking]  ,[Toilet] ,[Kitchen] ,[Intercom],[Accessible]  ,[AirCondition] ,[WiFi] ,[Canvas] ,[GreenScreen],[PottersWheel],[Guitar],[Drum] ,[Speaker] ,[FkUserId],[FilterDate],[minCapacity],[maxCapacity]) ";
        command = prefix + sb.ToString();

        return command;
    }
    public List<SportFilter> readSportFilters()
    {
        List<SportFilter> list = new List<SportFilter>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM SportFilters_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                SportFilter sf = new SportFilter();

                sf.Id = Convert.ToInt32(dr["Id"]);
                sf.Field = (string)dr["Field"];
                sf.Rating = Convert.ToDouble(dr["Rating"]);
                sf.MinPrice = Convert.ToInt32(dr["minPrice"]);
                sf.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                sf.MaxDistance = Convert.ToInt32(dr["MaxDistance"]);
                sf.StartTime = dr["StartTime"].ToString();
                sf.EndTime = dr["EndTime"].ToString();
                sf.Parking = Convert.ToBoolean(dr["Parking"]);
                sf.Toilet = Convert.ToBoolean(dr["Toilet"]);
                sf.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                sf.Intercom = Convert.ToBoolean(dr["Intercom"]);
                sf.Accessible = Convert.ToBoolean(dr["Accessible"]);
                sf.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                sf.WiFi = Convert.ToBoolean(dr["WiFi"]);
                sf.Trx = Convert.ToBoolean(dr["TRX"]);
                sf.Treadmill = Convert.ToBoolean(dr["Treadmill"]);
                sf.StationaryBicycle = Convert.ToBoolean(dr["StationaryBicycle"]);
                sf.Bench = Convert.ToBoolean(dr["Bench"]);
                sf.Dumbells = Convert.ToBoolean(dr["Dumbells"]);
                sf.Barbell = Convert.ToBoolean(dr["Barbell"]);
                sf.UserId = Convert.ToInt32(dr["FkUserId"]);
                sf.Date = dr["FilterDate"].ToString();
                sf.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                sf.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);

                list.Add(sf);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    public int insert(SportFilter sf)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(sf);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(SportFilter sf)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //string format = "yyyy-MM-dd HH:mm:ss";
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}',{1}, {2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'," +
            "'{13}','{14}','{15}','{16}','{17}','{18}','{19}',{20},'{21}',{22},{23})", sf.Field, sf.Rating.ToString(), sf.MinPrice.ToString(),
            sf.MaxPrice.ToString(), sf.MaxDistance.ToString(), sf.StartTime, sf.EndTime, sf.Parking, sf.Toilet, sf.Kitchen, sf.Intercom,
            sf.Accessible, sf.AirCondition, sf.WiFi, sf.Trx, sf.Treadmill, sf.StationaryBicycle, sf.Bench, sf.Dumbells, sf.Barbell,
            sf.UserId.ToString(), time.ToString(format), sf.MinCapacity.ToString(), sf.MaxCapacity.ToString());
        String prefix = "INSERT INTO SportFilters_2020 " + "([Field],[Rating],[minPrice],[maxPrice],[MaxDistance],[StartTime],[EndTime]," +
            "[Parking],[Toilet],[Kitchen],[Intercom],[Accessible],[AirCondition],[WiFi],[TRX],[Treadmill],[StationaryBicycle],[Bench]," +
            "[Dumbells] ,[Barbell] ,[FkUserId],[FilterDate],[minCapacity],[maxCapacity]) ";
        command = prefix + sb.ToString();

        return command;
    }
    public List<BeautyFilter> readBeautyFilters()
    {
        List<BeautyFilter> list = new List<BeautyFilter>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM BeautyFilters_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                BeautyFilter bf = new BeautyFilter();

                bf.Id = Convert.ToInt32(dr["Id"]);
                bf.Field = (string)dr["Field"];
                bf.Rating = Convert.ToDouble(dr["Rating"]);
                bf.MinPrice = Convert.ToInt32(dr["minPrice"]);
                bf.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                bf.MaxDistance = Convert.ToInt32(dr["MaxDistance"]);
                bf.StartTime = dr["StartTime"].ToString();
                bf.EndTime = dr["EndTime"].ToString();
                bf.Parking = Convert.ToBoolean(dr["Parking"]);
                bf.Toilet = Convert.ToBoolean(dr["Toilet"]);
                bf.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                bf.Intercom = Convert.ToBoolean(dr["Intercom"]);
                bf.Accessible = Convert.ToBoolean(dr["Accessible"]);
                bf.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                bf.WiFi = Convert.ToBoolean(dr["WiFi"]);
                bf.Dryers = Convert.ToBoolean(dr["Dryers"]);
                bf.NailPolishRacks = Convert.ToBoolean(dr["NailPolishRacks"]);
                bf.ReceptionAreaSeatingandDecor = Convert.ToBoolean(dr["ReceptionAreaSeatingandDecor"]);
                bf.LaserHairRemoval = Convert.ToBoolean(dr["LaserHairRemoval"]);
                bf.PedicureManicure = Convert.ToBoolean(dr["PedicureManicure"]);
                bf.HairColoringKit = Convert.ToBoolean(dr["HairColoringKit"]);
                bf.UserId = Convert.ToInt32(dr["FkUserId"]);
                bf.Date = dr["FilterDate"].ToString();
                bf.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                bf.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);

                list.Add(bf);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    public int insert(BeautyFilter bf)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(bf);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(BeautyFilter bf)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //string format = "yyyy-MM-dd HH:mm:ss";
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values('{0}',{1}, {2},{3},{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}',{20},'{21}',{22},{23})", bf.Field, bf.Rating.ToString(), bf.MinPrice.ToString(), bf.MaxPrice.ToString(), bf.MaxDistance.ToString(), bf.StartTime, bf.EndTime, bf.Parking, bf.Toilet, bf.Kitchen, bf.Intercom, bf.Accessible, bf.AirCondition, bf.WiFi, bf.Dryers, bf.NailPolishRacks, bf.ReceptionAreaSeatingandDecor, bf.LaserHairRemoval, bf.PedicureManicure, bf.HairColoringKit, bf.UserId.ToString(), time.ToString(format), bf.MinCapacity.ToString(), bf.MaxCapacity.ToString());
        String prefix = "INSERT INTO BeautyFilters_2020 " + "([Field],[Rating],[minPrice],[maxPrice],[MaxDistance],[StartTime],[EndTime],[Parking]  ,[Toilet] ,[Kitchen] ,[Intercom],[Accessible]  ,[AirCondition] ,[WiFi] ,[Dryers] ,[NailPolishRacks],[ReceptionAreaSeatingandDecor],[LaserHairRemoval],[PedicureManicure] ,[HairColoringKit] ,[FkUserId],[FilterDate],[minCapacity],[maxCapacity]) ";
        command = prefix + sb.ToString();

        return command;
    }
    public List<SpaceVisit> readSpaceVisits()
    {
        List<SpaceVisit> list = new List<SpaceVisit>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM SpaceVisits_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                SpaceVisit sv = new SpaceVisit();

                sv.Id = Convert.ToInt32(dr["Id"]);
                sv.SpaceId = Convert.ToInt32(dr["SpaceId"]);
                sv.UserId = Convert.ToInt32(dr["UserId"]);
                sv.VisitDate = dr["VisitDate"].ToString();

                list.Add(sv);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    public int insert(SpaceVisit sv)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(sv);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------
    private String BuildInsertCommand(SpaceVisit sv)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //string format = "yyyy-MM-dd HH:mm:ss";
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        sb.AppendFormat("Values({0},{1},'{2}')", sv.SpaceId.ToString(),sv.UserId.ToString(),time.ToString(format));
        String prefix = "INSERT INTO [SpaceVisits_2020] " + "(SpaceId,UserId,VisitDate) ";
        command = prefix + sb.ToString();

        return command;
    }

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------


    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------


    public Dictionary<string, int> readArtFiltersData()
    {
        //List<ArtFilter> list = new List<ArtFilter>();
        Dictionary<string, int> Data = new Dictionary<string, int>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = @"select COUNT(Id) as Counter,
ISNULL(AVG(minPrice), 0) as minPriceAvg,
ISNULL(AVG(maxPrice), 0) as maxPriceAvg,
ISNULL(AVG(Rating), 0) as RatingAvg,
ISNULL(AVG(MaxDistance), 0) as MaxDistanceAvg,
ISNULL(cast(cast(avg(cast(CAST(StartTime as datetime) as float)) as datetime) as time(0)), '') as AvgStartTime,
ISNULL(cast(cast(avg(cast(CAST(EndTime as datetime) as float)) as datetime) as time(0)), '') as AvgEndTime,
ISNULL(AVG((DATEDIFF(MINUTE, 0, StartTime))), 0) as AvgStartTimeMinutes,
ISNULL(AVG((DATEDIFF(MINUTE, 0, EndTime))), 0) as AvgEndTimeMinutes,
count(nullif([Parking], 'false')) as ParkingCounter,
count(nullif([Toilet], 'false')) as ToiletCounter,
count(nullif([Kitchen], 'false')) as KitchenCounter,
count(nullif([Intercom], 'false')) as IntercomCounter,
count(nullif([Accessible], 'false')) as AccessibleCounter,
count(nullif([AirCondition], 'false')) as AirConditionCounter,
count(nullif([WiFi], 'false')) as WiFiCounter,
count(nullif([Canvas], 'false')) as CanvasCounter,
count(nullif([GreenScreen], 'false')) as GreenScreenCounter,
count(nullif([PottersWheel], 'false')) as PottersWheelCounter,
count(nullif([Guitar], 'false')) as GuitarCounter,
count(nullif([Drum], 'false')) as DrumCounter,
count(nullif([Speaker], 'false')) as SpeakerCounter,
count(nullif([PottersWheel], 'false')) as PottersWheelCounter,
ISNULL(AVG(minCapacity), 0) as minCapacityAvg,
ISNULL(AVG(maxCapacity), 0) as maxCapacityAvg

from ArtFilters_2020
where DATEDIFF(D, [FilterDate], GETDATE())  < 14  ";

            //where DATEDIFF(D, [FilterDate], GETDATE())  < 14 "; #TODO 


            // Add some elements to the dictionary. There are no
            // duplicate keys, but some of the values are duplicates.

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                //ArtFilter af = new ArtFilter();

                //af.Id = 0;
                //af.Field = (string)dr["Field"];
                //af.Rating = Convert.ToInt32(dr["Rating"]);
                //af.MinPrice = Convert.ToInt32(dr["minPrice"]);
                //af.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                //af.MaxDistance = Convert.ToInt32(dr["MaxDistance"]);
                //af.StartTime = dr["StartTime"].ToString();
                //af.EndTime = dr["EndTime"].ToString();
                //af.Parking = Convert.ToBoolean(dr["Parking"]);
                //af.Toilet = Convert.ToBoolean(dr["Toilet"]);
                //af.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                //af.Intercom = Convert.ToBoolean(dr["Intercom"]);
                //af.Accessible = Convert.ToBoolean(dr["Accessible"]);
                //af.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                //af.WiFi = Convert.ToBoolean(dr["WiFi"]);
                //af.Canvas = Convert.ToBoolean(dr["Canvas"]);
                //af.GreenScreen = Convert.ToBoolean(dr["GreenScreen"]);
                //af.PottersWheel = Convert.ToBoolean(dr["PottersWheel"]);
                //af.Guitar = Convert.ToBoolean(dr["Guitar"]);
                //af.Drum = Convert.ToBoolean(dr["Drum"]);
                //af.Speaker = Convert.ToBoolean(dr["Speaker"]);
                //af.UserId = Convert.ToInt32(dr["FkUserId"]);
                //af.Date = dr["FilterDate"].ToString();
                //af.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                //af.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);

                Data.Add("Counter", Convert.ToInt32(dr["Counter"]));
                Data.Add("minCapacityAvg", Convert.ToInt32(dr["minCapacityAvg"]));
                Data.Add("maxCapacityAvg", Convert.ToInt32(dr["maxCapacityAvg"]));
                Data.Add("minPriceAvg", Convert.ToInt32(dr["minPriceAvg"]));
                Data.Add("maxPriceAvg", Convert.ToInt32(dr["maxPriceAvg"]));
                Data.Add("MaxDistanceAvg", Convert.ToInt32(dr["MaxDistanceAvg"]));
                Data.Add("AvgStartTimeMinutes", Convert.ToInt32(dr["AvgStartTimeMinutes"]));
                Data.Add("AvgEndTimeMinutes", Convert.ToInt32(dr["AvgEndTimeMinutes"]));
                Data.Add("ToiletCounter", Convert.ToInt32(dr["ToiletCounter"]));
                Data.Add("ParkingCounter", Convert.ToInt32(dr["ParkingCounter"]));
                Data.Add("KitchenCounter", Convert.ToInt32(dr["KitchenCounter"]));
                Data.Add("IntercomCounter", Convert.ToInt32(dr["IntercomCounter"]));
                Data.Add("AccessibleCounter", Convert.ToInt32(dr["AccessibleCounter"]));
                Data.Add("AirConditionCounter", Convert.ToInt32(dr["AirConditionCounter"]));
                Data.Add("WiFiCounter", Convert.ToInt32(dr["WiFiCounter"]));
                Data.Add("CanvasCounter", Convert.ToInt32(dr["CanvasCounter"]));
                Data.Add("GreenScreenCounter", Convert.ToInt32(dr["GreenScreenCounter"]));
                Data.Add("PottersWheelCounter", Convert.ToInt32(dr["PottersWheelCounter"]));
                Data.Add("GuitarCounter", Convert.ToInt32(dr["GuitarCounter"]));
                Data.Add("DrumCounter", Convert.ToInt32(dr["DrumCounter"]));
                Data.Add("SpeakerCounter", Convert.ToInt32(dr["SpeakerCounter"]));


                //list.Add(af);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Data;
    }
    public Dictionary<string, int> readBeautyFiltersData()
    {
        //List<ArtFilter> list = new List<ArtFilter>();
        Dictionary<string, int> Data = new Dictionary<string, int>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = @"select COUNT(Id) as Counter,
AVG(minPrice) as minPriceAvg,
AVG(maxPrice) as maxPriceAvg,
AVG(Rating) as RatingAvg,
AVG(MaxDistance) as MaxDistanceAvg,
cast(cast(avg(cast(CAST(StartTime as datetime) as float)) as datetime) as time(0)) AvgStartTime,
cast(cast(avg(cast(CAST(EndTime as datetime) as float)) as datetime) as time(0)) AvgEndTime,
AVG((DATEDIFF(MINUTE, 0, StartTime))) as AvgStartTimeMinutes,
AVG((DATEDIFF(MINUTE, 0, EndTime))) as AvgEndTimeMinutes,
count(nullif([Parking], 'false')) as ParkingCounter,
count(nullif([Toilet], 'false')) as ToiletCounter,
count(nullif([Kitchen], 'false')) as KitchenCounter,
count(nullif([Intercom], 'false')) as IntercomCounter,
count(nullif([Accessible], 'false')) as AccessibleCounter,
count(nullif([AirCondition], 'false')) as AirConditionCounter,
count(nullif([WiFi], 'false')) as WiFiCounter,
count(nullif([Dryers], 'false')) as DryersCounter,
count(nullif([NailPolishRacks], 'false')) as NailPolishRacksCounter,
count(nullif([ReceptionAreaSeatingandDecor], 'false')) as ReceptionAreaSeatingandDecorCounter,
count(nullif([LaserHairRemoval], 'false')) as LaserHairRemovalCounter,
count(nullif([PedicureManicure], 'false')) as PedicureManicureCounter,
count(nullif([HairColoringKit], 'false')) as HairColoringKitCounter,
ISNULL(AVG(minCapacity), 0) as minCapacityAvg,
ISNULL(AVG(maxCapacity), 0) as maxCapacityAvg

from BeautyFilters_2020
where DATEDIFF(D, [FilterDate], GETDATE())  < 14 ";

            //where DATEDIFF(D, [FilterDate], GETDATE())  < 14 "; #TODO 

            // Add some elements to the dictionary. There are no
            // duplicate keys, but some of the values are duplicates.

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                //ArtFilter af = new ArtFilter();

                //af.Id = 0;
                //af.Field = (string)dr["Field"];
                //af.Rating = Convert.ToInt32(dr["Rating"]);
                //af.MinPrice = Convert.ToInt32(dr["minPrice"]);
                //af.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                //af.MaxDistance = Convert.ToInt32(dr["MaxDistance"]);
                //af.StartTime = dr["StartTime"].ToString();
                //af.EndTime = dr["EndTime"].ToString();
                //af.Parking = Convert.ToBoolean(dr["Parking"]);
                //af.Toilet = Convert.ToBoolean(dr["Toilet"]);
                //af.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                //af.Intercom = Convert.ToBoolean(dr["Intercom"]);
                //af.Accessible = Convert.ToBoolean(dr["Accessible"]);
                //af.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                //af.WiFi = Convert.ToBoolean(dr["WiFi"]);
                //af.Canvas = Convert.ToBoolean(dr["Canvas"]);
                //af.GreenScreen = Convert.ToBoolean(dr["GreenScreen"]);
                //af.PottersWheel = Convert.ToBoolean(dr["PottersWheel"]);
                //af.Guitar = Convert.ToBoolean(dr["Guitar"]);
                //af.Drum = Convert.ToBoolean(dr["Drum"]);
                //af.Speaker = Convert.ToBoolean(dr["Speaker"]);
                //af.UserId = Convert.ToInt32(dr["FkUserId"]);
                //af.Date = dr["FilterDate"].ToString();
                //af.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                //af.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);

                Data.Add("Counter", Convert.ToInt32(dr["Counter"]));
                Data.Add("minCapacityAvg", Convert.ToInt32(dr["minCapacityAvg"]));
                Data.Add("maxCapacityAvg", Convert.ToInt32(dr["maxCapacityAvg"]));
                Data.Add("minPriceAvg", Convert.ToInt32(dr["minPriceAvg"]));
                Data.Add("maxPriceAvg", Convert.ToInt32(dr["maxPriceAvg"]));
                Data.Add("MaxDistanceAvg", Convert.ToInt32(dr["MaxDistanceAvg"]));
                Data.Add("AvgStartTimeMinutes", Convert.ToInt32(dr["AvgStartTimeMinutes"]));
                Data.Add("AvgEndTimeMinutes", Convert.ToInt32(dr["AvgEndTimeMinutes"]));
                Data.Add("ToiletCounter", Convert.ToInt32(dr["ToiletCounter"]));
                Data.Add("ParkingCounter", Convert.ToInt32(dr["ParkingCounter"]));
                Data.Add("KitchenCounter", Convert.ToInt32(dr["KitchenCounter"]));
                Data.Add("IntercomCounter", Convert.ToInt32(dr["IntercomCounter"]));
                Data.Add("AccessibleCounter", Convert.ToInt32(dr["AccessibleCounter"]));
                Data.Add("AirConditionCounter", Convert.ToInt32(dr["AirConditionCounter"]));
                Data.Add("WiFiCounter", Convert.ToInt32(dr["WiFiCounter"]));
                Data.Add("DryersCounter", Convert.ToInt32(dr["DryersCounter"]));
                Data.Add("NailPolishRacksCounter", Convert.ToInt32(dr["NailPolishRacksCounter"]));
                Data.Add("ReceptionAreaSeatingandDecorCounter", Convert.ToInt32(dr["ReceptionAreaSeatingandDecorCounter"]));
                Data.Add("LaserHairRemovalCounter", Convert.ToInt32(dr["LaserHairRemovalCounter"]));
                Data.Add("PedicureManicureCounter", Convert.ToInt32(dr["PedicureManicureCounter"]));
                Data.Add("HairColoringKitCounter", Convert.ToInt32(dr["HairColoringKitCounter"]));


                //list.Add(af);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Data;
    }
    public Dictionary<string, int> readSportFiltersData()
    {
        //List<ArtFilter> list = new List<ArtFilter>();
        Dictionary<string, int> Data = new Dictionary<string, int>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = @"select COUNT(Id) as Counter,
ISNULL(AVG(minPrice), 0) as minPriceAvg,
ISNULL(AVG(maxPrice), 0) as maxPriceAvg,
ISNULL(AVG(Rating), 0) as RatingAvg,
ISNULL(AVG(MaxDistance), 0) as MaxDistanceAvg,
ISNULL(cast(cast(avg(cast(CAST(StartTime as datetime) as float)) as datetime) as time(0)), '') as AvgStartTime,
ISNULL(cast(cast(avg(cast(CAST(EndTime as datetime) as float)) as datetime) as time(0)), '') as AvgEndTime,
ISNULL(AVG((DATEDIFF(MINUTE, 0, StartTime))), 0) as AvgStartTimeMinutes,
ISNULL(AVG((DATEDIFF(MINUTE, 0, EndTime))), 0) as AvgEndTimeMinutes,
count(nullif([Parking], 'false')) as ParkingCounter,
count(nullif([Toilet], 'false')) as ToiletCounter,
count(nullif([Kitchen], 'false')) as KitchenCounter,
count(nullif([Intercom], 'false')) as IntercomCounter,
count(nullif([Accessible], 'false')) as AccessibleCounter,
count(nullif([AirCondition], 'false')) as AirConditionCounter,
count(nullif([WiFi], 'false')) as WiFiCounter,
count(nullif([TRX], 'false')) as TRXCounter,
count(nullif([Treadmill], 'false')) as TreadmillCounter,
count(nullif([StationaryBicycle], 'false')) as StationaryBicycleCounter,
count(nullif([Bench], 'false')) as BenchCounter,
count(nullif([Dumbells], 'false')) as DumbellsCounter,
count(nullif([Barbell], 'false')) as BarbellCounter,

ISNULL(AVG(minCapacity), 0) as minCapacityAvg,
ISNULL(AVG(maxCapacity), 0) as maxCapacityAvg

from SportFilters_2020
where DATEDIFF(D, [FilterDate], GETDATE())  < 14 
";

            //where DATEDIFF(D, [FilterDate], GETDATE())  < 14 "; #TODO 

            // Add some elements to the dictionary. There are no
            // duplicate keys, but some of the values are duplicates.

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                //ArtFilter af = new ArtFilter();

                //af.Id = 0;
                //af.Field = (string)dr["Field"];
                //af.Rating = Convert.ToInt32(dr["Rating"]);
                //af.MinPrice = Convert.ToInt32(dr["minPrice"]);
                //af.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                //af.MaxDistance = Convert.ToInt32(dr["MaxDistance"]);
                //af.StartTime = dr["StartTime"].ToString();
                //af.EndTime = dr["EndTime"].ToString();
                //af.Parking = Convert.ToBoolean(dr["Parking"]);
                //af.Toilet = Convert.ToBoolean(dr["Toilet"]);
                //af.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                //af.Intercom = Convert.ToBoolean(dr["Intercom"]);
                //af.Accessible = Convert.ToBoolean(dr["Accessible"]);
                //af.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                //af.WiFi = Convert.ToBoolean(dr["WiFi"]);
                //af.Canvas = Convert.ToBoolean(dr["Canvas"]);
                //af.GreenScreen = Convert.ToBoolean(dr["GreenScreen"]);
                //af.PottersWheel = Convert.ToBoolean(dr["PottersWheel"]);
                //af.Guitar = Convert.ToBoolean(dr["Guitar"]);
                //af.Drum = Convert.ToBoolean(dr["Drum"]);
                //af.Speaker = Convert.ToBoolean(dr["Speaker"]);
                //af.UserId = Convert.ToInt32(dr["FkUserId"]);
                //af.Date = dr["FilterDate"].ToString();
                //af.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                //af.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);

                Data.Add("Counter", Convert.ToInt32(dr["Counter"]));
                Data.Add("minCapacityAvg", Convert.ToInt32(dr["minCapacityAvg"]));
                Data.Add("maxCapacityAvg", Convert.ToInt32(dr["maxCapacityAvg"]));
                Data.Add("minPriceAvg", Convert.ToInt32(dr["minPriceAvg"]));
                Data.Add("maxPriceAvg", Convert.ToInt32(dr["maxPriceAvg"]));
                Data.Add("MaxDistanceAvg", Convert.ToInt32(dr["MaxDistanceAvg"]));
                Data.Add("AvgStartTimeMinutes", Convert.ToInt32(dr["AvgStartTimeMinutes"]));
                Data.Add("AvgEndTimeMinutes", Convert.ToInt32(dr["AvgEndTimeMinutes"]));
                Data.Add("ToiletCounter", Convert.ToInt32(dr["ToiletCounter"]));
                Data.Add("ParkingCounter", Convert.ToInt32(dr["ParkingCounter"]));
                Data.Add("KitchenCounter", Convert.ToInt32(dr["KitchenCounter"]));
                Data.Add("IntercomCounter", Convert.ToInt32(dr["IntercomCounter"]));
                Data.Add("AccessibleCounter", Convert.ToInt32(dr["AccessibleCounter"]));
                Data.Add("AirConditionCounter", Convert.ToInt32(dr["AirConditionCounter"]));
                Data.Add("WiFiCounter", Convert.ToInt32(dr["WiFiCounter"]));
                Data.Add("TRXCounter", Convert.ToInt32(dr["TRXCounter"]));
                Data.Add("TreadmillCounter", Convert.ToInt32(dr["TreadmillCounter"]));
                Data.Add("StationaryBicycleCounter", Convert.ToInt32(dr["StationaryBicycleCounter"]));
                Data.Add("BenchCounter", Convert.ToInt32(dr["BenchCounter"]));
                Data.Add("DumbellsCounter", Convert.ToInt32(dr["DumbellsCounter"]));
                Data.Add("BarbellCounter", Convert.ToInt32(dr["BarbellCounter"]));


                //list.Add(af);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Data;
    }

    

    //--------------------------------------------------------------------
    // Build the Insert command method String for Users table
    //--------------------------------------------------------------------

    public int getOrdersAmountById(int spaceid)
    {
        int amount = 0;
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = @"select COUNT(OrderId) as OrdersAmount
from Orders_2020
Where spaceid=" + spaceid;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row


                amount = Convert.ToInt32(dr["OrdersAmount"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return amount;

    }



    //Grades Section//

    public Dictionary<string, int> getGrades()
    {

        //List<ArtFilter> list = new List<ArtFilter>();
        Dictionary<string, int> Grades = new Dictionary<string, int>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string    Grades_2020
            string selectSTR = @"select top 1 *
from Grades_2020
order by ModifiedDate desc;";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                Grades.Add("Price", Convert.ToInt32(dr["Price"]));
                Grades.Add("Capacity", Convert.ToInt32(dr["Capacity"]));
                Grades.Add("Facility", Convert.ToInt32(dr["Facility"]));
                Grades.Add("Equipment", Convert.ToInt32(dr["Equipment"]));
                Grades.Add("Rating", Convert.ToInt32(dr["Rating"]));
                Grades.Add("Premium", Convert.ToInt32(dr["Premium"]));
                Grades.Add("Order", Convert.ToInt32(dr["Order"]));
                Grades.Add("Conversion", Convert.ToInt32(dr["Conversion"]));


            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return Grades;

    }

    public List<Grade> readGrades()
    {
        List<Grade> list = new List<Grade>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = "SELECT * FROM Grades_2020 ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                Grade g = new Grade();

                g.GradeId = Convert.ToInt32(dr["GradeId"]);
                g.Price = Convert.ToDouble(dr["Price"]);
                g.Capacity = Convert.ToDouble(dr["Capacity"]);
                g.Facility = Convert.ToDouble(dr["Facility"]);
                g.Equipment = Convert.ToDouble(dr["Equipment"]);
                g.Rating = Convert.ToDouble(dr["Rating"]);
                g.Premium = Convert.ToDouble(dr["Premium"]);
                g.Order = Convert.ToDouble(dr["Order"]);
                g.Conversion = Convert.ToDouble(dr["Conversion"]);
                g.ModifiedDate = dr["ModifiedDate"].ToString();


                list.Add(g);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return list;
    }

    public int insert(Grade Grade)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildInsertCommand(Grade);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildInsertCommand(Grade Grade)
    {
        String command;
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1},{2}, {3},{4}, {5},{6}, {7},'{8}')", Grade.Price, Grade.Capacity, Grade.Facility, Grade.Equipment, Grade.Rating,
            Grade.Premium, Grade.Order, Grade.Conversion, time.ToString(format));
        String prefix = "INSERT INTO Grades_2020";
        command = prefix + sb.ToString();

        return command;
    }

    public int update(Grade Grade)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        String cStr = BuildUpdateCommand(Grade);
        // cmd = CreatCommmand(cStr, con);
        cmd = CreateCommand(cStr, con);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
                con.Close();
        }
    }

    private String BuildUpdateCommand(Grade Grade)
    {
        String command;
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat(@"SET [Price] = {0}
      ,[Capacity] =  {1}
      ,[Facility] ={2}
      ,[Equipment] = {3}
      ,[Rating] = {4}
      ,[Premium] = {5}
      ,[Order] = {6}
      ,[Conversion] = {7}
      ,[ModifiedDate] = '{8}'
 WHERE [GradeId]={9} ", Grade.Price, Grade.Capacity, Grade.Facility, Grade.Equipment, Grade.Rating,
            Grade.Premium, Grade.Order, Grade.Conversion, time.ToString(format), 1);
        String prefix = "UPDATE [dbo].[Grades_2020] ";
        command = prefix + sb.ToString();

        return command;
    }




    public int readSpaceVisits(int spaceId)
    {
        List<SpaceVisit> list = new List<SpaceVisit>();
        SqlConnection con = null;
        int visits = 0;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = @"select COUNT(Id) as Visits
from SpaceVisits_2020
where SpaceId=" + spaceId;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row

                visits = Convert.ToInt32(dr["Visits"]);

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return visits;
    }
    public bool isUserPremium(string userEmail)
    {
        bool b = false;
        List<Space> Spaces = new List<Space>();
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            string selectSTR = @"select Premium 
from Users_2020
where Email='" + userEmail + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                b = Convert.ToBoolean(dr["Premium"]);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return b;
    }

    public double readFiltersDataRating(string field)
    {
        //List<ArtFilter> list = new List<ArtFilter>();
        double rating = 0;
        SqlConnection con = null;
        try
        {
            con = connect("database");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {

            StringBuilder sb = new StringBuilder();

            // use a string builder to create the dynamic string
            string selectSTR = @"select ISNULL(AVG(Rating),0) as RatingAvg 
from " + field + @"Filters_2020
where DATEDIFF(D, [FilterDate], GETDATE())  < 14 ";

            //where DATEDIFF(D, [FilterDate], GETDATE())  < 3 "; #TODO 


            // Add some elements to the dictionary. There are no
            // duplicate keys, but some of the values are duplicates.

            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {   // Read till the end of the data into a row
                //ArtFilter af = new ArtFilter();

                //af.Id = 0;
                //af.Field = (string)dr["Field"];

                //af.MinPrice = Convert.ToInt32(dr["minPrice"]);
                //af.MaxPrice = Convert.ToInt32(dr["maxPrice"]);
                //af.MaxDistance = Convert.ToInt32(dr["MaxDistance"]);
                //af.StartTime = dr["StartTime"].ToString();
                //af.EndTime = dr["EndTime"].ToString();
                //af.Parking = Convert.ToBoolean(dr["Parking"]);
                //af.Toilet = Convert.ToBoolean(dr["Toilet"]);
                //af.Kitchen = Convert.ToBoolean(dr["Kitchen"]);
                //af.Intercom = Convert.ToBoolean(dr["Intercom"]);
                //af.Accessible = Convert.ToBoolean(dr["Accessible"]);
                //af.AirCondition = Convert.ToBoolean(dr["AirCondition"]);
                //af.WiFi = Convert.ToBoolean(dr["WiFi"]);
                //af.Canvas = Convert.ToBoolean(dr["Canvas"]);
                //af.GreenScreen = Convert.ToBoolean(dr["GreenScreen"]);
                //af.PottersWheel = Convert.ToBoolean(dr["PottersWheel"]);
                //af.Guitar = Convert.ToBoolean(dr["Guitar"]);
                //af.Drum = Convert.ToBoolean(dr["Drum"]);
                //af.Speaker = Convert.ToBoolean(dr["Speaker"]);
                //af.UserId = Convert.ToInt32(dr["FkUserId"]);
                //af.Date = dr["FilterDate"].ToString();
                //af.MinCapacity = Convert.ToInt32(dr["minCapacity"]);
                //af.MaxCapacity = Convert.ToInt32(dr["maxCapacity"]);
                rating = Convert.ToDouble(dr["RatingAvg"]);



                //list.Add(af);
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }
        return rating;
    }

    public int updateSpace(Space space)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        String command;

        StringBuilder sb = new StringBuilder();
      
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime time = DateTime.Now;

        // use a string builder to create the dynamic string
        sb.AppendFormat(@"SET [SpaceName]='{0}' ,[Field]='{1}' ,[Price]={2},[City]='{3}',[Street]='{4}' " +
            ",[Number]='{5}',[Capabillity]={6} ,[Bank]='{7}' ,[Branch]='{8}'  ,[Image1]='{9}'  ,[Image2]='{10}'," +
            "[Image3]='{11}',[Image4]='{12}',[Image5]='{13}',[AccountNumber]='{14}',[FKEmail]='{15}'," +
            "[Description]='{16}',[TermsOfUse]='{17}',[Rank]={18}, [UploadDate]='{19}',[Latitude]={20}, [Longitude]={21}" +
            " Where [SpaceId]={22} ", space.Name, space.Field, space.Price, space.City, space.Street, space.Number, space.Capabillity,
            space.Bank, space.Branch, space.Imageurl1, space.Imageurl2, space.Imageurl3, space.Imageurl4, space.Imageurl5, space.AccountNumber,
            space.UserEmail, space.Description, space.TermsOfUse, space.Rank, time.ToString(format), space.Latitude, space.Longitude, space.Id);

        String prefix = "UPDATE Spaces_2020 ";
        command = prefix + sb.ToString();


        String cStr = command;      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }


    }

    private String BuildDeleteEquipmentCommand(int spaceId)
    {
        String command;
        command = "delete from Equipment_2020 where FKSpaceId=" + spaceId.ToString();
        return command;
    }

    public int deleteEquipment(int spaceId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteEquipmentCommand(spaceId);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private String BuildDeleteWeekAvailabilitiesCommand(int spaceId)
    {
        String command;
        command = "delete from WeekAvailablity_2020 where FkSpaceId=" + spaceId.ToString();
        return command;
    }

    public int deleteWeekAvailabilities(int spaceId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("database"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildDeleteWeekAvailabilitiesCommand(spaceId);      // helper method to build the insert string

        cmd = CreateCommand(cStr, con);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
}


