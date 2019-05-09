using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace FoodGuide.Models
{
  public class Cuisine
  {
    public int Id {get; set;}
    public string Name {get; set;}
    public bool VegFriendly {get; set;}
    //public string ImageURL {get; set;}

    public Cuisine (string name, bool vegFriendly, /*string imageURL,*/ int id = 0) {
      Name = name;
      VegFriendly = vegFriendly;
      //ImageURL = imageURL;
      Id = id;
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> allCuisines = new List<Cuisine> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisine;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        bool vegFriendly = rdr.GetBoolean(2);

        Cuisine newCuisine = new Cuisine(name, vegFriendly, id);
        allCuisines.Add(newCuisine);
      }
      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
      return allCuisines;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cuisine";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if (!(otherCuisine is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;
        bool idEquality = (this.Id == newCuisine.Id);
        bool nameEquality = (this.Name == newCuisine.Name);
        bool vegFriendlyEquality = (this.VegFriendly == newCuisine.VegFriendly);
        return (idEquality && nameEquality && vegFriendlyEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO `cuisine` (`name`, `vegFriendly`) VALUES (@CuisineName, @CuisineVegFriendly);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@CuisineName";
      name.Value = this.Name;

      MySqlParameter vegFriendly = new MySqlParameter();
      vegFriendly.ParameterName = "@CuisineVegFriendly";
      vegFriendly.Value = this.VegFriendly;

      cmd.Parameters.Add(name);
      cmd.Parameters.Add(vegFriendly);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      // more logic will go here in a moment
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Cuisine Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `cuisine` WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      string cuisineName = "";
      bool cuisineVegFriendly = false;
      int cuisineId = 0;

      while (rdr.Read())
      {
        cuisineName = rdr.GetString(1);
        cuisineVegFriendly = rdr.GetBoolean(2);
        cuisineId = rdr.GetInt32(0);
      }

      Cuisine foundCuisine = new Cuisine(cuisineName, cuisineVegFriendly, cuisineId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundCuisine;
    }
  }
}
