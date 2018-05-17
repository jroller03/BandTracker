using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker.Models;

namespace BandTracker
{
  public class Venue
  {
    private int _id;
    private string _venueName;

    public Venue(string VenueName, int id = 0)
    {
      _id = id;
      _venueName = VenueName;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetVenueName()
    {
      return _venueName;
    }
    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
         Venue newVenue = (Venue) otherVenue;
         bool idEquality = this.GetId() == newVenue.GetId();
         bool venueNameEquality = this.GetVenueName() == newVenue.GetVenueName();
         return (idEquality && venueNameEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetVenueName().GetHashCode();
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO venues (venue_name) VALUES (@thisVenueName);";

      cmd.Parameters.Add(new MySqlParameter("@thisVenueName", this._venueName));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public static List<Venue> GetAllVenues()
    {
      List<Venue> allVenues = new List<Venue> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        Venue newVenue = new Venue(venueName, id);
        allVenues.Add(newVenue);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allVenues;
    }
    public static Venue Find (int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int venueId = 0;
      string venueName = "";

      while(rdr.Read())
      {
        venueId = rdr.GetInt32(0);
        venueName = rdr.GetString(1);
      }
      Venue newVenue = new Venue(venueName, venueId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newVenue;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues WHERE id = @thisId; DELETE FROM bands_venues WHERE venue_id = @thisId;";

      MySqlParameter idParameter = new MySqlParameter();
      idParameter.ParameterName = "@thisId";
      idParameter.Value = this.GetId();
      cmd.Parameters.Add(idParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddBand(Band newBand)
    {
      MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO venues (band_id, venue_id) VALUES (@BandId, @VenueId);";

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@BandId";
        band_id.Value = newBand.GetId();
        cmd.Parameters.Add(band_id);

        MySqlParameter _id = new MySqlParameter();
        _id.ParameterName = "@VenueId";
        _id.Value = _id;
        cmd.Parameters.Add(_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public List<Band> GetBands()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT bands.* FROM venues
            JOIN bands_venues ON (venues.venue_id = venue_bands.venue_id)
            JOIN bands ON (venue_bands.band_id = bands.band_id)
            WHERE venues.venue_id = @VenueId;";

        MySqlParameter idParameter = new MySqlParameter();
        idParameter.ParameterName = "@VenueId";
        idParameter.Value = _id;
        cmd.Parameters.Add(idParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> bandIds = new List<int> {};
        while(rdr.Read())
        {
            int bandId = rdr.GetInt32(0);
            bandIds.Add(bandId);
        }
        rdr.Dispose();

        List<Band> bands = new List<Band> {};
        foreach (int bandId in bandIds)
        {
            var bandQuery = conn.CreateCommand() as MySqlCommand;
            bandQuery.CommandText = @"SELECT * FROM bands WHERE band_id = @BandId;";

            MySqlParameter bandIdParameter = new MySqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = bandId;
            bandQuery.Parameters.Add(bandIdParameter);

            var bandQueryRdr = bandQuery.ExecuteReader() as MySqlDataReader;
            while(bandQueryRdr.Read())
            {
                int BandId = bandQueryRdr.GetInt32(0);
                string bandName = bandQueryRdr.GetString(1);
                Band foundBand = new Band(bandName, bandId);
                bands.Add(foundBand);
            }
            bandQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return bands;
    }
    public void UpdateVenue(string newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET venue_name = @venueName WHERE id = @searchId";
      cmd.Parameters.Add(new MySqlParameter("@searchId", _id));
      cmd.Parameters.Add(new MySqlParameter("@venueName", newVenue));
      cmd.ExecuteNonQuery();
      _venueName = newVenue;
      conn.Close();
      if (conn !=null)
      {
          conn.Dispose();
      }
    }


  }
}
