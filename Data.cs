using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace WebApplication
{
    public class Data
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

        public DataTable GetRandomDataRecords(int noOfRecords)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "with cte as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,city,IF(lookfor=2,'friend or date', IF(lookfor=1,'date','friend')) AS lookFor,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.fitness=1,'Fitness',NULL) AS fitness,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investment=1,'investment',NULL) AS investment,IF(i.business=1,'Business',NULL) AS business,IF(i.movies=1,'Movies',NULL) AS movies,IF(i.shopping=1,'Shopping',NULL) AS shopping,IF(i.roadtrips=1,'Road trips',NULL) AS roadtrips,IF(i.politics=1,'Politics',NULL) AS politics,IF(i.chillatbar=1,'Chill at bar',NULL) AS chillatbar FROM mySite.users u inner JOIN mySite.interest i ON u.id = i.id) SELECT id,Name,gender,city,lookFor,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,fitness,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investment,business,movies,shopping,roadtrips,politics,chillatbar) AS interests FROM cte order by rand() limit @noOfRecords";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@noOfRecords", noOfRecords);
                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}