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
     public CourseTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker;";
       }
        public void Dispose()
        {
          Course.DeleteAll();
          Student.DeleteAll();
        }




   }
}
