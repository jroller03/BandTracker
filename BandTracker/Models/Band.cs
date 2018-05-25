using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker.Models;

namespace BandTracker
{
  public class Band
  {
    private int _id;
    private string _name;

    public Band(string Name, int id = 0)
    {
      _name = Name;
      _id = id;
    }
    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
         Band newBand = (Band) otherBand;
         bool idEquality = this.GetId() == newBand.GetId();
         bool nameEquality = this.GetName() == newBand.GetName();
         return (idEquality && nameEquality);
       }
    }
    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }
    public static List<Band> GetAllBands()
    {
     List<Band> allBands = new List<Band>{};
     MySqlConnection conn = DB.Connection();
     conn.Open();
     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText= @"SELECT * FROM bands;";

     var rdr = cmd.ExecuteReader() as MySqlDataReader;
     while (rdr.Read())
     {
       int id = rdr.GetInt32(0);
       string name = rdr.GetString(1);
       Band newBand = new Band(name, id);
       allBands.Add(newBand);
     }
     conn.Close();
     if (conn != null)
     {
       conn.Dispose();
     }
     return allBands;
    }
     public void Save()
     {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

        cmd.CommandText = @"INSERT INTO bands (band_name) VALUES (@thisName);";

        cmd.Parameters.Add(new MySqlParameter("@thisName", _name));

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }
      public static Band Find (int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

        cmd.CommandText= @"SELECT * FROM bands WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int bandId = 0;
        string name = "";

        while(rdr.Read())
        {
          bandId = rdr.GetInt32(0);
          name = rdr.GetString(1);
        }
        Band newBand = new Band(name, bandId);

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return newBand;
      }
      public List<Venue> GetVenues()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT venues.* FROM bands
        JOIN bands_venues ON (band_id = bands_venues.band_id)
        JOIN venues ON (bands_venues.venue_id=venues.id)
        WHERE band_id = @BandId;";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@BandId";
        bandIdParameter.Value = _id;
        cmd.Parameters.Add(bandIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<Venue> venues = new List<Venue> {};
        while(rdr.Read())
        {
          int venueId = rdr.GetInt32(0);
          string venueName = rdr.GetString(1);
          Venue newVenue = new Venue(venueName, venueId);
          venues.Add(newVenue);
        }
        rdr.Dispose();

        conn.Close();
        if (conn != null)
        {
           conn.Dispose();
        }
        return venues;
      }
      public void AddVenue(Venue newVenue)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO bands_venues (venue_id, band_id ) VALUES (@VenueId, @BandId);";

        MySqlParameter venue_id = new MySqlParameter();
        venue_id.ParameterName = "@VenueId";
        venue_id.Value = newVenue.GetId();
        cmd.Parameters.Add(venue_id);

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@BandId";
        band_id.Value = _id;
        cmd.Parameters.Add(band_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }
      public static void DeleteAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM bands;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }




  }
}
