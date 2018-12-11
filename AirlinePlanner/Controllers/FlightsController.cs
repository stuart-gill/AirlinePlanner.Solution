using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class FlightsController : Controller
    {
        [HttpGet("cities/{cityId}/flights/new")]
        public ActionResult New(int cityId)
        {
            City city = City.Find(cityId);
            return View();
        }
        
         [HttpGet("/cities/{cityId}/flights/{flightId}")]
        public ActionResult Show(int cityId, int flightId)
        {
            Flight flight = Flight.Find(flightId);
            Dictionary<string, object> model = new Dictionary<string, object>();
            City city = City.Find(cityId);
            model.Add("flight", flight);
            model.Add("city", city);
            return View(model);
        }

        // [HttpGet("/cities/{id}/delete")]
        // public ActionResult Delete(int id)
        // {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     City city = City.Find(id);
        //     List<Flight> cityFlights = city.GetFlights();

        //     foreach(Flight flight in cityFlights)
        //     {
        //     flight.Delete(city.GetId(), flight.GetId());
        //     }

        //     City.Delete(id);

        //     model.Add("city", city);
        //     model.Add("flights", cityFlights);
        //     return View("Delete", model);

        // }

        //  [HttpPost("/cities/{cityId}/flights/{flightId}")]
        // public ActionResult Update(int cityId, int flightId, string newName, string newDepartTime, string newStatus)
        // {
        //     Flight flight = Flight.Find(flightId);
        //     flight.Edit(newName, newDepartTime, newStatus);
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     City city = City.Find(cityId);
        //     model.Add("city", city);
        //     model.Add("flight", flight);
        //     return View("Show", model);
        // }
    }
}