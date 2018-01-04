using AppTimedoc.Data;
using AppTimedoc.Helpers;
using AppTimedoc.Models;
using Java.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTimedoc.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Time : ContentPage
	{
    DateTime _dateSelected = DateTime.MinValue;
    Coworker _user = null;
    List<WorkingTime> list = null;
    WorkingTime _actWorkingTime = null;

    public Time ()
		{
			InitializeComponent ();
      SetUIHandlers();
    }
    private async void SetUIHandlers()
    {
      DayDate.Date = DateTime.Now;
      _dateSelected = DayDate.Date;

      _user = await App.Database.GetCoworker();

      if (_user != null)
        await LoadList(0);
    }
    protected async override void OnAppearing()
    {
      base.OnAppearing();

      _user = await App.Database.GetCoworker();

      if (_user == null)
      {
        var tabbedPage = this.Parent as MainTabbedPage;
        tabbedPage.SwitchTab(3);
      }
      else
      {
        DayDate.Date = DateTime.Now;
        _dateSelected = DayDate.Date;
        await LoadList(0);
      }
    }

    async Task LoadList(int typ)
    {
      //if (CheckConnectivity() == false)
      //{
      //  WorkingTimeView.ItemsSource = null;
      //  return;
      //}

      // Basic-http
      App.restManager = new RestManager(new Web.RestService());
      bool rc = false;

      if (typ == 1 || typ == 2)
      {
        rc = await App.restManager.UpdateWorkingTimeAsync(_actWorkingTime);

        if (rc == false)
        {
          // Lokal speichern da Problem mit dem Speichern auf dem Server.
          // TODO lokale Einträge synchronisieren
          //WorkingTimeStorage dbWt = new WorkingTimeStorage();
          //dbWt.SaveWorkingTime(_actWorkingTime);

          //var wt = dbWt.LoadWorkingTime(_actWorkingTime.Id);
          //var lst = dbWt.LoadOfflineWorkingTimes();
          //dbWt.DeleteWorkingTime(_actWorkingTime);

        }

      }

      _actWorkingTime = await App.restManager.GetWorkingTimeCoworkerAsync(_user);
      if (_actWorkingTime.ComeTime == DateTime.MinValue)
      {
        btnCome.IsEnabled = true;
        btnGo.IsEnabled = false;
      }
      else
      {
        btnCome.IsEnabled = false;
        btnGo.IsEnabled = true;
      }

      try
      {
        list = await App.restManager.GetWorkingTimeAsync(_dateSelected);
        if (list != null)
        {
          SetDisplayText();
          WorkingTimeView.ItemsSource = list;
        }
      }
      catch (Exception)
      {
        WorkingTimeView.ItemsSource = null;
      }
    }


    public static bool isOnline()
    {
      try
      {
        URLConnection urlConnection = new URL(Constants.RestUrl + "/ping").OpenConnection();
        urlConnection.ReadTimeout = 400;
        urlConnection.Connect();
        return true;
      }
      catch (Exception)
      {
        return false;
      }

    }

    // TCP/HTTP/DNS (depending on the port, 53=DNS, 80=HTTP, etc.)
    public bool isOnline0()
    {
      try
      {
        int timeoutMs = 1500;
        Socket sock = new Socket();
        SocketAddress sockaddr = new InetSocketAddress("8.8.8.8", 53);

        sock.Connect(sockaddr, timeoutMs);
        sock.Close();

        return true;
      }
      catch (IOException)
      {
        return false;
      }
    }

    private bool CheckConnectivity()
    {
      var networkConnection = DependencyService.Get<INetworkConnection>();
      networkConnection.CheckNetworkConnection();
      var networkStatus = networkConnection.IsConnected;

      networkStatus = isOnline();

      //var isConnected = Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
      //var isConnected = DependencyService.Get<INetworkConnection>().IsConnected;
      if (networkStatus)
      {
        lblConnect.IsVisible = true;
        lblConnect.Text = "-- OFFLINE --";
        lblConnect.BackgroundColor = Color.Red;
        return true;
      }
      else
      {
        lblConnect.IsVisible = false;
        return false;
      }
    }

    private async void btnGo_Clicked(object sender, EventArgs e)
    {
      _actWorkingTime.GoTime = DateTime.Now;
      await LoadList(1);
    }

    private async void btnCome_Clicked(object sender, EventArgs e)
    {
      _actWorkingTime.ComeTime = DateTime.Now;
      await LoadList(1);
    }

    private void Date_DateSelected(object sender, DateChangedEventArgs e)
    {
    }

    private void SetDisplayText()
    {
      foreach (WorkingTime item in list)
      {
        string menge = "------";
        if (item.Duration != null && item.Duration > 0)
          menge = item.DurationTxt;

        string dateGo = string.Empty;
        if (item.GoTime != null)
        {
          DateTime d = (DateTime)item.GoTime;
          dateGo = d.ToString("HH:mm");
        }
        item.TxtLarge = item.ComeTime.ToString("HH:mm") + " - " + dateGo + " : " + menge;
        item.TxtSmall = item.AchieName + ": " + item.Remark;

        if (item.ComeTime.Date != DateTime.Now.Date)
          item.TxtSmall = item.AchieName + " " + item.ComeTime.ToString("dd.MM") + ": " + item.Remark;
      }

    }

    private async void WorkingTimeView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      try
      {
        _actWorkingTime = e.SelectedItem as WorkingTime;
        if (_actWorkingTime == null)
          return;

        var modalPage = new TimeDetail(_actWorkingTime);
        modalPage.Disappearing += (sender2, e2) =>
        {
          WorkingTimeView.SelectedItem = null;
          Refresh();
        };
        await Navigation.PushModalAsync(modalPage);
      }
      catch (Exception ex)
      {
        string s = ex.Message;
        throw;
      }

    }

    private async void Refresh()
    {
      await LoadList(0);
    }

    private async void DayDate_DateSelected(object sender, DateChangedEventArgs e)
    {
      _dateSelected = e.NewDate;
      if (e.NewDate != e.OldDate)
        await LoadList(0);
    }

  }
}