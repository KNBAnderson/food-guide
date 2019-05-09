
using System;
using MySql.Data.MySqlClient;
using FoodGuide;
using System.Collections.Generic;

namespace FoodGuide.Models
{
  public class DB
  {
    public static MySqlConnection Connection()
    {
      MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
      return conn;
    }
  }
}
