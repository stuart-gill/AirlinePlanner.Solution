using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        private string _flightName;
        private string _departTime;
        private string _status;
        private int _id;
        

        public Flight(string name, string departTime, string status, int id = 0)
        {
            _flightName = name;
            _departTime = departTime;
            _status = status;
            _id = id;
        }

        public string GetName()
        {
            return _flightName;
        }

        public string GetDepartTime()
        {
            return _departTime;
        }

        public string GetStatus()
        {
            return _status;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetDepartTime(string newDepartTime)
        {
            _departTime = newDepartTime;
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int flightId = rdr.GetInt32(0);
                string flightName = rdr.GetString(1);
                DateTime flightDepartTime = (DateTime) rdr.GetDateTime(2);
                string flightStatus = rdr.GetString(3);
                Flight newFlight = new Flight(flightName, flightDepartTime.ToString("MM/d/yyyy"), flightStatus, flightId);
                allFlights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int flightId = 0;
            string flightName = "";
            string flightDepartTime = "";
            string flightStatus= "";
          
            while(rdr.Read())
            {
                flightId = rdr.GetInt32(0);
                flightName = rdr.GetString(1);
                flightDepartTime = rdr.GetString(2);
                flightStatus = rdr.GetString(3);
            }
         
            Flight foundFlight = new Flight(flightName, flightDepartTime, flightStatus, flightId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundFlight;
        }

        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight) otherFlight;
                bool idEquality = this.GetId() == newFlight.GetId();
                bool flightNameEquality = this.GetName() == newFlight.GetName();
                bool departTimeEquality = this.GetDepartTime() == newFlight.GetDepartTime();
                bool statusEquality = this.GetStatus() == newFlight.GetStatus();
                // We no longer compare Flights' categoryIds here.
                return (idEquality && flightNameEquality && departTimeEquality && statusEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._flightName;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public void Edit(string newName, string newDepartTime, string newStatus)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET name, depart_time, status = @newName, @newDepartTime, @newStatus WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter flightName = new MySqlParameter();
            flightName.ParameterName = "@newName";
            flightName.Value = newName;
            cmd.Parameters.Add(flightName);
            MySqlParameter flightDepartTime = new MySqlParameter();
            flightDepartTime.ParameterName = "@newDepartTime";
            flightDepartTime.Value = newDepartTime;
            cmd.Parameters.Add(flightDepartTime);
            MySqlParameter flightStatus = new MySqlParameter();
            flightStatus.ParameterName = "@newStatus";
            flightStatus.Value = newStatus;
            cmd.Parameters.Add(flightStatus);
            cmd.ExecuteNonQuery();
            _status = newStatus;
            _flightName = newName;
            _departTime = newDepartTime;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCity(City departureCity, City arrivalCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (departure_city_id, arrival_city_id, flight_id) VALUES (@DepartureCityId, @ArrivalCityId, @FlightId);";
            MySqlParameter departureCityId = new MySqlParameter();
            departureCityId.ParameterName = "@DepartureCityId";
            departureCityId.Value = departureCity.GetId();
            cmd.Parameters.Add(departureCityId);
            MySqlParameter arrivalCityId = new MySqlParameter();
            arrivalCityId.ParameterName = "@ArrivalCityId";
            arrivalCityId.Value = arrivalCity.GetId();
            cmd.Parameters.Add(arrivalCityId);
            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = _id;
            cmd.Parameters.Add(flight_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }



        public List<City> GetCities()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT cities.* FROM flights 
                JOIN cities_flights ON (flights.id = cities_flights.flight_id)
                JOIN cities ON (cities_flights.departure_city_id = cities.id)
                WHERE flights.id = @FlightId;";
            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = _id;
            cmd.Parameters.Add(flightIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<City> cities = new List<City> {};
            while(rdr.Read())
            {
                int cityId = rdr.GetInt32(0);
                string cityName = rdr.GetString(1);
                City newCity = new City(cityName, cityId);
                cities.Add(newCity);
            }
            conn.Close();
            if (conn !=null)
            {
                conn.Dispose();
            }
            return cities;
        }
     

        public void Delete(int cityId, int flightId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE id = @FlightId; DELETE FROM cities_flights WHERE flight_id = @FlightId;";
            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = _id;
            cmd.Parameters.Add(flightIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }


  }
}