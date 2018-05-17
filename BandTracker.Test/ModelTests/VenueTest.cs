using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;
using BandTracker;
using MySql.Data.MySqlClient;


namespace BandTracker.Tests
{

   [TestClass]
   public class VenueTests : IDisposable
   {
     public VenueTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker;";
       }
        public void Dispose()
        {
          Venue.DeleteAll();
          Band.DeleteAll();
        }
        [TestMethod]
        public void Save_SavesVenueToDatabase_VenueList()
        {
          Venue testVenue = new Venue("Orpheum");
          testVenue.Save();

          List<Venue> testResult = Venue.GetAllVenues();
          List<Venue> allVenues = new List<Venue> {testVenue};

          CollectionAssert.AreEqual(testResult, allVenues);
        }
        [TestMethod]
        public void Save_DatabaseAssignsIdToObject_Id()
        {
           //Arrange
           Venue testVenue = new Venue("Orpheum");
           testVenue.Save();

           //Act
           Venue savedVenue = Venue.GetAllVenues()[0];

           int result = savedVenue.GetId();
           int testId = testVenue.GetId();

           //Assert
           Assert.AreEqual(testId, result);
        }
        [TestMethod]
        public void Equals_OverrideTrueForSameName_Venue()
        {
           //Arrange, Act
           Venue firstVenue = new Venue("Orpheum");
           Venue secondVenue = new Venue("Orpheum");

           //Assert
           Assert.AreEqual(firstVenue, secondVenue);
        }
        [TestMethod]
        public void Find_FindsVenueInDatabase_Venue()
        {
           //Arrange
           Venue testVenue = new Venue("Stadium");
           testVenue.Save();

           //Act
           Venue foundVenue = Venue.Find(testVenue.GetId());

           //Assert
           Assert.AreEqual(testVenue, foundVenue);
        }
        // [TestMethod]
        // public void GetBands_ReturnsAllVenueBands_BandList()
        // {
        //   //Arrange
        //   Venue testVenue = new Venue("Orpheum");
        //   testVenue.Save();
        //
        //   Band testBand1 = new Band("Nirvana");
        //   testBand1.Save();
        //
        //   Band testBand2 = new Band("Metallica");
        //   testBand2.Save();
        //
        //   //Act
        //   testVenue.AddBand(testBand1);
        //   testVenue.AddBand(testBand2);
        //   List<Band> result = testVenue.GetBands();
        //   List<Band> testList = new List<Band> {testBand1, testBand2};
        //
        //   //Assert
        //   CollectionAssert.AreEqual(testList, result);
        // }




   }
}
