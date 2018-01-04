using AppTimedoc.Helpers;
using AppTimedoc.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppTimedoc.Data
{
  public class CoworkerStorage
  {
    virtual public Coworker LoadCoworker()
    {
      string s = DependencyService.Get<IFileHelper>().GetLocalFilePath("TimedocSQLite.db3");

      using (SQLiteConnection connection = new SQLiteConnection(s))
      {
        return connection.Table<Coworker>().FirstOrDefault();
      }
    }
    virtual public void SaveCoworker(Coworker coworker)
    {
      string s = DependencyService.Get<IFileHelper>().GetLocalFilePath("TimedocSQLite.db3");

      using (SQLiteConnection connection = new SQLiteConnection(s))
      {
        if (connection.Table<Coworker>().Count() > 0)
        {
          throw new InvalidOperationException("There already exist an Coworker in the database!");
        }
        connection.Insert(coworker);
      }
    }
    virtual public void DeleteCoworker(Coworker coworker)
    {
      string s = DependencyService.Get<IFileHelper>().GetLocalFilePath("TimedocSQLite.db3");
      //      using (SQLiteConnection connection = new SQLiteConnection(AppConfigurationController.DBPath))
      using (SQLiteConnection connection = new SQLiteConnection(s))
      {
        if (connection.Table<Coworker>().Count() <= 0)
        {
          throw new InvalidOperationException("There exist no Coworker in the database");
        }

        connection.Delete(coworker);
      }
    }

  }
}
