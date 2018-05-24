using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class VenueController : Controller
    {
      [HttpGet("/venue")]
    public ActionResult Index()
    {
      List<Venue> allVenues = Venue.GetAllVenues();
      return View(allVenues);
    }

    [HttpGet("/venue/new")]
    public ActionResult CreateForm()
    {
        return View();
    }
    [HttpPost("/venue")]
    public ActionResult Create()
    {
        Venue newVenue = new Venue(Request.Form["venue-name"]);
        newVenue.Save();
        return RedirectToAction("Index");
    }

    [HttpGet("/venue/{id}")]
    public ActionResult Details(int id)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue selectedVenue = Venue.Find(id);
        List<Band> venueBands = selectedVenue.GetBands();
        List<Band> allBands = Band.GetAllBands();
        model.Add("selectedVenue", selectedVenue);
        model.Add("venueBands", venueBands);
        model.Add("allBands", allBands);
        return View(model);
    }

    [HttpGet("/venue/delete")]
    public ActionResult DeleteVenue()
    {
        List<Venue> allVenues = Venue.GetAllVenues();
        return View(allVenues);
    }

    [HttpPost("/venue/delete")]
    public ActionResult DeletePost()
    {
        int id= int.Parse(Request.Form["venue-delete"]);
        Venue selectedVenue=Venue.Find(id);
        selectedVenue.Delete();
        return RedirectToAction("Index");
    }

    [HttpGet("/venue/update")]
    public ActionResult UpdateVenue()
    {
        List<Venue> allVenues = Venue.GetAllVenues();
        return View(allVenues);
    }

    [HttpPost("/venue/update")]
    public ActionResult UpdatePost()
    {
        int id= int.Parse(Request.Form["venue-update"]);
        Venue selectedVenue=Venue.Find(id);

        string newName=Request.Form["new-venue-name"];

        selectedVenue.UpdateVenue(newName);
        return RedirectToAction("Index");
    }



    [HttpPost("/venue/{venueId}/band/new")]
    public ActionResult AddBand(int venueId)
    {
        Venue venue = Venue.Find(venueId);
        Band band = Band.Find(int.Parse(Request.Form["band-id"]));
        venue.AddBand(band);
        return RedirectToAction("Details",  new { id = venueId });
    }
  }
}
