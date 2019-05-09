using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace FoodGuide.Models
{
  public class Restaurant
  {
    public int Id {get; set;}
    public string Name {get; set;}
    public int CuisineId {get; set;}
    public int Price {get; set;}

    //public string ImageURL {get; set;}

    public Restaurant (string name, bool cuisineId, int price, int id = 0) {
      Name = name;
      CuisineId = cuisineId;
      Price = price;
      Id = id;
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurant;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        bool cuisineId = rdr.GetBoolean(2);
        int price = rdr.GetInt32(3);

        Restaurant newRestaurant = new Restaurant(name, cuisineId, price, id);
        allRestaurants.Add(newRestaurant);
      }
      conn.Close();

      if (conn != null)
      {
        conn.Dispose();
      }
      return allRestaurants;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurant";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool idEquality = (this.Id == newRestaurant.Id);
        bool nameEquality = (this.Name == newRestaurant.Name);
        bool cuisineIdEquality = (this.CuisineId == newRestaurant.CuisineId);
        bool priceEquality = (this.Price == newRestaurant.Price);
        return (idEquality && nameEquality && cuisineIdEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"INSERT INTO `restaurant` (`name`, `cuisineId`, `price`) VALUES (@RestaurantName, @RestaurantCuisineId, @RestaurantPrice);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@RestaurantName";
      name.Value = this.Name;

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@RestaurantCuisineId";
      cuisineId.Value = this.CuisineId;

      MySqlParameter Price = new MySqlParameter();
      Price.ParameterName = "@RestaurantCuisineId";
      Price.Value = this.Price;

      cmd.Parameters.Add(name);
      cmd.Parameters.Add(cuisineId);
      cmd.Parameters.Add(Price);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      // more logic will go here in a moment
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Restaurant Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `restaurant` WHERE id = @thisId;";
      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      string restaurantName = "";
      bool restaurantCuisineId = false;
      int restaurantId = 0;

      while (rdr.Read())
      {
        restaurantName = rdr.GetString(1);
        restaurantCuisineId = rdr.GetBoolean(2);
        restaurantId = rdr.GetInt32(0);
      }

      Restaurant foundRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRestaurant;
    }
  }
}
