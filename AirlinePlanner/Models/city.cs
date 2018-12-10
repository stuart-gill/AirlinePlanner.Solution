using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class City
    {
        private string _cityName;
        private int _id;
        

        public City(string name, int id = 0)
        {
            _cityName = name;
            _id = id;
        }

        public string GetName()
        {
            return _cityName;
        }

        public int GetId()
        {
            return _id;
        }

            public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
        }

            public static List<City> GetAll()
        {
            List<City> allCities = new List<City> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int CityId = rdr.GetInt32(0);
                string CityName = rdr.GetString(1);
                City newCity = new City(CityName, CityId);
                allCities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }

        public static City Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int CityId = 0;
            string CityName = "";
            while(rdr.Read())
            {
            CityId = rdr.GetInt32(0);
            CityName = rdr.GetString(1);
            }
            City newCity = new City(CityName, CityId);
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return newCity;
        }

            public List<Flight> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
                JOIN cities_flights ON (cities.id = cities_flights.departure_city_id)
                JOIN flights ON (cities_flights.flight_id = flights.id)
                WHERE cities.id = @CityId;";
            MySqlParameter departureCityId = new MySqlParameter();
            departureCityId.ParameterName = "@CityId";
            departureCityId.Value = _id;
            cmd.Parameters.Add(departureCityId);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> flights = new List<Flight>{};
            while(rdr.Read())
            {
            int flightId = rdr.GetInt32(0);
            string flightName = rdr.GetString(1);
            DateTime departTime = (DateTime) rdr.GetDateTime(2);
            string status = rdr.GetString(3);

            Flight newFlight = new Flight(flightName, departTime.ToString("MM/d/yyyy"), status, flightId);
            flights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return flights;
        }

    //     public static void DeleteFlights(int id)
    //   {
    //     MySqlConnection conn = DB.Connection();
    //     conn.Open();
    //     var cmd = conn.CreateCommand() as MySqlCommand;
    //     //   cmd.CommandText = @"DELETE FROM flights WHERE city_id = @cityId;";
    //     //   MySqlParameter cityId = new MySqlParameter();
    //     //   cityId.ParameterName = "@cityId";
    //     //   cityId.Value = id;
    //     //   cmd.Parameters.Add(cityId);
    //     conn.Close();
    //     if (conn != null)
    //     {
    //     conn.Dispose();
    //     }
    //   }

        public override bool Equals(System.Object otherCity)
    {
    if (!(otherCity is City))
        {
            return false;
        }
    else
        {
            City newCity = (City) otherCity;
            bool idEquality = this.GetId().Equals(newCity.GetId());
            bool nameEquality = this.GetName().Equals(newCity.GetName());
            return (idEquality && nameEquality);
        }
    }

      public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";
        MySqlParameter name = new MySqlParameter();
        name.ParameterName = "@name";
        name.Value = this._name;
        cmd.Parameters.Add(name);
        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId; // <-- This line is new!
        conn.Close();
        if (conn != null)
        {
        conn.Dispose();
        }
    }

  // public static void Delete()
  //   {
  //     MySqlConnection conn = DB.Connection();
  //     conn.Open();
  //     MySqlCommand cmd = new MySqlCommand("DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_flights WHERE city_id = @CityId;", conn);
  //     MySqlParameter cityIdParameter = new MySqlParameter();
  //     cityIdParameter.ParameterName = "@CityId";
  //     cityIdParameter.Value = this.GetId();
  //     cmd.Parameters.Add(cityIdParameter);
  //     cmd.ExecuteNonQuery();
  //     if (conn != null)
  //     {
  //       conn.Close();
  //     }
  //   }

  public void AddFlight(Flight newFlight)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities_flights (departure_city_id, flight_id) VALUES (@DepartureCityId, @FlightId);";
      MySqlParameter departure_city_id = new MySqlParameter();
      departure_city_id.ParameterName = "@DepartureCityId";
      departure_city_id.Value = _id;
      cmd.Parameters.Add(departure_city_id);
      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = newFlight.GetId();
      cmd.Parameters.Add(flight_id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }



    }
}
