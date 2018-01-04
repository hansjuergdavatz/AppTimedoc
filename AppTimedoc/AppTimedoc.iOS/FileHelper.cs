using AppTimedoc.Helpers;
using AppTimedoc.iOS;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(FileHelper))]
namespace AppTimedoc.iOS
{
  public class FileHelper : IFileHelper
  {
    public string GetLocalFilePath(string filename)
    {
      string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
      return Path.Combine(path, filename);
    }
  }
}
