using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using kany.Models;

namespace kany.Controllers
{
    public class HomeController : Controller
    {
        public string username;
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr = null ;
        Random rand = new Random();

      
        public int UserID()
        {

            int id;
            con.Close();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select UserID from UserAuthentication where username = '" + Session["username"] + "'";
            dr=com.ExecuteReader();
            id = Convert.ToInt32(dr["UserID"]);
            return id;
        }


        public void ConnectionString()
        {
            con.ConnectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=IADatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
        [HttpPost]
        public ActionResult registerMember(FormCollection form)
        {
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Insert into UserAuthentication values ('" + rand.Next(100,1000) + "','"+form["Username"]+"','"+form["Password"]+"','"+0+"','"+form["Email"]+"',NULL,NULL,NULL) ";
            dr = com.ExecuteReader();
            Session["Username"] = form["Username"];
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddActor(FormCollection form)
        {
            string ActorName = form["FirstName"] + " " + form["LastName"];
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Insert into Actor values ('" + rand.Next(1000, 10000) + "','" + ActorName + "','" + form["DateOfBirth"] + "','" + form["Gender"] + "','" + form["country"] + "','"+form["City"]+"','"+form["ContactNumber"]+"',null) ";
            dr = com.ExecuteReader();
            return RedirectToAction("ManageActors");
        }
        [HttpPost]
        public ActionResult AddDirector(FormCollection form)
        {
            string DirectorName = form["FirstName"] + " " + form["LastName"];
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Insert into Director values ('" + rand.Next(10000, 11000) + "','" + DirectorName + "','" + form["ContactNumber"] + "','" + form["DateOfBirth"] + "','" + form["Gender"] + "','" + form["country"] + "',NULL,'"+form["City"]+"') ";
            dr = com.ExecuteReader();
            return RedirectToAction("ManageDirectors");
        }
        public ActionResult AddNewMovie(FormCollection form)
        {
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Insert into Movies values ('" + rand.Next(12000, 13000) + "','" + form["MovieName"] + "',NULL,'" + form["ReleaseDate"] + "','" + form["MPAA"] + "','" + form["Duration"] + "','" + form["Category"] + "','"+form["Rating"]+"','" + form["ActorID"] + "','"+form["DirectorID"]+"') ";
            dr = com.ExecuteReader();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Verify(FormCollection form)
        {
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select userID from UserAuthentication where Username ='" + form["username"] + "' and Password ='" + form["password"] + "'";
            dr = com.ExecuteReader();

            if (dr.Read())
            {

                Session["username"] = form["username"];
                username = form["username"];
                con.Close();

                return RedirectToAction("Index");
            }
            else
            {
                con.Close();
                return View("Error");
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public ActionResult deletemyaccount()
        {
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "delete from UserAuthentication where Username = '" + Session["username"].ToString() + "'";
            dr = com.ExecuteReader();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteActor(int ID)
        {
 
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "spDeleteActor";
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter paramId = new SqlParameter();
            paramId.ParameterName = "@ActorID";
            paramId.Value = ID;
            com.Parameters.Add(paramId);

            dr = com.ExecuteReader();
            
            return RedirectToAction("ManageActors");
        }
        [HttpPost]
        public ActionResult DeleteMovie(int ID)
        {

            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "spDeleteMovie";
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter paramId = new SqlParameter();
            paramId.ParameterName = "@MovieID";
            paramId.Value = ID;
            com.Parameters.Add(paramId);

            dr = com.ExecuteReader();

            return RedirectToAction("ShowMovies");
        }
        [HttpPost]
        public ActionResult DeleteDirector(int ID)
        {

            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "spDeleteDirector";
            com.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter paramId = new SqlParameter();
            paramId.ParameterName = "@DirectorID";
            paramId.Value = ID;
            com.Parameters.Add(paramId);

            dr = com.ExecuteReader();

            return RedirectToAction("ManageDirectors");
        }

        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            List<SearchModel> search = new List<SearchModel>();
            ConnectionString();
            con.Open();
            com.Connection= con;
            com.CommandText = "Select * from Movies , Actor , Director where MovieName Like '%'"+form["search"]+ "'%' or ActorName Like '%'" + form["search"] + "'%'or DirectorName Like '%'" + form["search"] + "'%'";
            dr=com.ExecuteReader();
            while (dr.Read())
            {
                var result = new SearchModel();
                result.ActorId = Convert.ToInt32(dr["ActorID"]);
                result.ActorName = dr["ActorName"].ToString();
                result.DirectorId = Convert.ToInt32(dr["DirectorID"]);
                result.DirectorName = dr["DirectorName"].ToString();
                result.MovieId = Convert.ToInt32(dr["MovieID"]);
                result.MovieName = dr["MovieName"].ToString();
                search.Add(result);
            }
            return View(search);
        }


        public ActionResult Profile()
        {
            UserModel user = new UserModel();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText =  "Select ProfilePicture from UserAuthentication where username = '"+Session["username"]+"'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                user.Image = dr["ProfilePicture"].ToString();
            }
            return View(user);
        }

        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Actor()
        {
            return View();
        }
        public ActionResult Director()
        {
            return View();
        }

        public ActionResult AddActor()
        {

            return View();
        }
        public ActionResult Movie()
        {
            return View();
        }
        public ActionResult AddDirector()
        {

            return View();
        }
        public ActionResult AddMovie()
        {

            return View();
        }
        public ActionResult AccountSettings()
        {
            UserModel user = new UserModel();

            return View(user);
        }
        public ActionResult ManageActors()
        {
            List<ActorsModel> actorsList = new List<ActorsModel>();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * from Actor";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                var Actor = new ActorsModel();
                Actor.ActorId = Convert.ToInt32(dr["ActorID"]);
                Actor.ActorName = dr["ActorName"].ToString();
                Actor.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                Actor.Gender = dr["Gender"].ToString();
                Actor.Nationality = dr["Nationality"].ToString();
                Actor.City = dr["City"].ToString();
                Actor.ContactNumber = dr["ContactNumber"].ToString();
                actorsList.Add(Actor);

            }
            return View(actorsList);
        }
        public ActionResult ManageDirectors()
        {
            List<DirectorsModel> DirectorList = new List<DirectorsModel>();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * from Director";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                var Director = new DirectorsModel();
                Director.DirectorId = Convert.ToInt32(dr["DirectorID"]);
                Director.DirectorName = dr["DirectorName"].ToString();
                Director.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                Director.Gender = dr["Gender"].ToString();
                Director.Nationality = dr["Nationality"].ToString();
                Director.City = dr["City"].ToString();
                Director.ContactNumber = dr["ContactNumber"].ToString();
                DirectorList.Add(Director);

            }
            return View(DirectorList);
        }
        [HttpPost]
        public ActionResult ChangeProfilePicture(UserModel user, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/images/profiles/"), pic);

                file.SaveAs(path);

                user.Image = pic;
            }
            ConnectionString();
            con.Open();
            com.Connection= con;
            com.CommandText = "Update UserAuthentication Set ProfilePicture = '" + user.Image + "' Where Username = '"+Session["username"]+"'";
            dr = com.ExecuteReader();
            return View("Index");
        }
        
        public ActionResult ActorDetails(int id)
        {
            var Actor = new ActorsModel();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * From Actor where ActorID = '" + id + "' ";
            dr = com.ExecuteReader();

            while (dr.Read())
            {
                
                Actor.ActorId = Convert.ToInt32(dr["ActorID"]);
                Actor.ActorName = dr["ActorName"].ToString();
                Actor.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                Actor.Gender = dr["Gender"].ToString();
                Actor.Nationality = dr["Nationality"].ToString();
                Actor.City = dr["City"].ToString();
                Actor.ContactNumber = dr["ContactNumber"].ToString();
                

            }
            return View(Actor);
        }
        public ActionResult DirectorDetails(int id)
        {
            var Director = new DirectorsModel();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * From Director where DirectorID = '" + id + "' ";
            dr = com.ExecuteReader();

            while (dr.Read())
            {

                Director.DirectorId = Convert.ToInt32(dr["DirectorID"]);
                Director.DirectorName = dr["DirectorName"].ToString();
                Director.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                Director.Gender = dr["Gender"].ToString();
                Director.Nationality = dr["Nationality"].ToString();
                Director.City = dr["City"].ToString();
                Director.ContactNumber = dr["ContactNumber"].ToString();


            }
            return View(Director);
        }
        public ActionResult ShowMovies()
        {
            List<MovieModel> movies = new List<MovieModel>();
            ConnectionString();
            con.Open();
            com.Connection= con;
            com.CommandText = "Select * from Movies";
            dr=com.ExecuteReader();
            while (dr.Read())
            {
                var movie = new MovieModel();
                movie.MovieID = Convert.ToInt32(dr["MovieID"]);
                movie.MovieName = dr["MovieName"].ToString();
                movie.Image = dr["MovieImage"].ToString();
                movie.DateOfRelease = Convert.ToDateTime(dr["DateOfRelease"]);
                movie.MPAA = dr["MPAA"].ToString();
                movie.Duration = Convert.ToInt32(dr["Duration"]);
                movie.Category = dr["Category"].ToString();
                movie.Rating = Convert.ToInt32(dr["Rating"]);
                movie.ActorID = Convert.ToInt32(dr["ActorID"]);
                movie.DirectorID = Convert.ToInt32(dr["DirectorID"]);
                movies.Add(movie);
            }
            return View(movies);
        }
        public ActionResult AddComment(int id)
        {
            var movie = new MovieModel();
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * from Movies where MovieID= '"+id+"'";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                movie.MovieID = Convert.ToInt32(dr["MovieID"]);
                movie.MovieName = dr["MovieName"].ToString();
                movie.Image = dr["MovieImage"].ToString();
                movie.DateOfRelease = Convert.ToDateTime(dr["DateOfRelease"]);
                movie.MPAA = dr["MPAA"].ToString();
                movie.Duration = Convert.ToInt32(dr["Duration"]);
                movie.Category = dr["Category"].ToString();
                movie.Rating = Convert.ToInt32(dr["Rating"]);
                movie.ActorID = Convert.ToInt32(dr["ActorID"]);
                movie.DirectorID = Convert.ToInt32(dr["DirectorID"]);
            }
            return View(movie);
        }

        [HttpPost]
        public ActionResult AddNewComment(int id, FormCollection form)
        {
            
            
            ConnectionString();
            con.Open();
            com.Connection= con;
            com.CommandText = "Insert into Comments values ('" + rand.Next(15000, 20000) + "','"+UserID()+"','"+id+"','"+form["Comment"] +"')";
            dr = com.ExecuteReader();
            return RedirectToAction("Index");
        }
    }
}