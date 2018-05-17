using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;
using BandTracker;
using MySql.Data.MySqlClient;


namespace BandTracker.Tests
{

   [TestClass]
   public class BandTests : IDisposable
   {
     public BandTests()
     {
         DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker;";
     }
      public void Dispose()
      {
        Band.DeleteAll();
        Venue.DeleteAll();
      }
      [TestMethod]
      public void Save_SavesBandToDatabase_BandList()
      {
        Band testBand = new Band("Nirvana");
        testBand.Save();

        List<Band> testResult = Band.GetAllBands();
        List<Band> allBands = new List<Band> {testBand};

        CollectionAssert.AreEqual(testResult, allBands);
      }
      [TestMethod]
      public void Save_DatabaseAssignsIdToObject_Id()
      {
         //Arrange
         Band testBand = new Band("Nirvana");
         testBand.Save();

         //Act
         Band savedBand = Band.GetAllBands()[0];

         int result = savedBand.GetId();
         int testId = testBand.GetId();

         //Assert
         Assert.AreEqual(testId, result);
      }
      [TestMethod]
      public void Equals_OverrideTrueForSameName_Band()
      {
         //Arrange, Act
         Band firstBand = new Band("Nirvana");
         Band secondBand = new Band("Nirvana");

         //Assert
         Assert.AreEqual(firstBand, secondBand);
      }
      [TestMethod]
      public void Find_FindsBandInDatabase_Band()
      {
         //Arrange
         Band testBand = new Band("Nirvana");
         testBand.Save();

         //Act
         Band foundBand = Band.Find(testBand.GetId());

         //Assert
         Assert.AreEqual(testBand, foundBand);
      }
      [TestMethod]
      public void GetVenues_ReturnsAllBandVenues_VenueList()
      {
        //Arrange
        Band testBand = new Band("Nirvana");
        testBand.Save();

        Venue testVenue1 = new Venue("Orpheum");
        testVenue1.Save();

        Venue testVenue2 = new Venue("Stadium");
        testVenue2.Save();

        //Act
        testBand.AddVenue(testVenue1);
        testBand.AddVenue(testVenue2);
        List<Venue> result = testBand.GetVenues();
        List<Venue> testList = new List<Venue> {testVenue1, testVenue2};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }








   }
}
