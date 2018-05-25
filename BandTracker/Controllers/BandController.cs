using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class BandController : Controller
    {
      [HttpGet("/band")]
        public ActionResult Index()
        {
            List<Band> allBands = Band.GetAllBands();
            return View(allBands);
        }
        [HttpGet("/band/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/band")]
        public ActionResult Create()
        {
            Band newBand = new Band(Request.Form["band-name"]);
            newBand.Save();
            return View("Index");
        }
        [HttpGet("/band/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Band selectedBand = Band.Find(id);
            List<Venue> venueBands = selectedBand.GetVenues();
            List<Venue> allVenues = Venue.GetAllVenues();
            model.Add("selectedBand", selectedBand);
            model.Add("venueBands", venueBands);
            model.Add("allVenues", allVenues);
            return View(model);
        }
        [HttpPost("/band/{venueId}/venue/new")]
        public ActionResult AddVenue(int bandId)
        {
            Band band = Band.Find(bandId);
            Venue venue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
            band.AddVenue(venue);
            return RedirectToAction("ViewBand",  new { id = bandId });
        }
    }
}
