using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class CitiesController : Controller
    {
        [HttpGet("/cities")]
        public ActionResult Index()
        {
            List<City> allCities = City.GetAll();
            return View();
        }
        
        [HttpGet("/cities/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/cities")]
        public ActionResult Create(string cityName)
        {
            City newCity = new City(cityName);
            newCity.Save();
            List<City> allCities = City.GetAll();
            return View("Index", allCities);
        }

        [HttpGet("/cities/{id}/flights")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            City selectedCity = City.Find(id);
            List<Flight> cityFlights = selectedCity.GetFlights();
            model.Add("city", selectedCity);
            model.Add("flights", cityFlights);
            return View(model);
        }

        // [HttpGet("/cities/{cityId}/delete")]
        // public ActionResult Delete(int cityId)
        // {
        //   Dictionary<string, object> model = new Dictionary<string, object>();
        //   City selectedCity = City.Find(cityId);
        //   model.Add("city", selectedCity);
        //   model.Add("flights", cityFlights);
        //   City.DeleteFlights(cityId);
        //   City.Delete(cityId);
        //   return View();
        // }

        [HttpPost("/cities/{cityId}/flights")]
        public ActionResult Create(int cityId, string flightName, string flightDepartTime, string flightStatus)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            City foundCity = City.Find(cityId);
            Flight newFlight = new Flight(flightName, flightDepartTime, flightStatus, cityId);
            newFlight.Save();
            foundCity.AddFlight(newFlight);
            List<Flight> cityFlights = foundCity.GetFlights();
            model.Add("flights", cityFlights);
            model.Add("city", foundCity);
            return View("Show", model);
        }
        
    }
}