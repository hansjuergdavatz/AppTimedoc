using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppTimedoc.Droid;
using AppTimedoc.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(NetworkHelper))]
namespace AppTimedoc.Droid
{
  public class NetworkHelper : INetworkConnection
  {
    public bool IsConnected { get; set; }
    public void CheckNetworkConnection()
    {
      var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
      var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
      if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
      {
        IsConnected = true;
      }
      else
      {
        IsConnected = false;
      }
    }

  }
}