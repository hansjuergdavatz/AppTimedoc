using AppTimedoc.Data;
using AppTimedoc.Helpers;
using AppTimedoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTimedoc.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkDetail : ContentPage
	{
    OrderAchievement _actOrderAchievement = null;
    bool _hasCostUnit = false;
    bool _setTime = false;

    public WorkDetail(OrderAchievement oa, bool costUnit, bool signature)
    {
      InitializeComponent();

      switch (Device.RuntimePlatform)
      {
        case Device.iOS:
          btnDelete.Image = "ic_delete.png";
          btnSave.Image = "ic_save.png";
          btnSignature.Image = "ic_border_color.png";
          btnAbort.Image = "ic_arrow_back.png";
          break;
        case Device.Android:
          //playPage.Title = string.Empty;
          //playPage.Icon = "ic_access_time_black_24dp.png";
          //workPage.Title = string.Empty;
          //workPage.Icon = "ic_list_black_24dp.png";
          //settingsPage.Title = string.Empty;
          //settingsPage.Icon = "ic_error_outline_black_24dp.png";
          //aboutPage.Title = string.Empty;
          //aboutPage.Icon = "ic_accessibility_black_24dp.png";
          break;
        default:
          break;
      }

      if (signature)
        btnSignature.IsVisible = true;
      else
        btnSignature.IsVisible = false;

      _actOrderAchievement = oa;
      _hasCostUnit = costUnit;

      if (_hasCostUnit == false)
      {
        btnCost.IsVisible = false;
        btnCost.HeightRequest = 0;
      }

      if (_actOrderAchievement.Unit == "h")
      {
        DateAchie2.IsVisible = true;
        TimeAchie2.IsVisible = true;
      }
      else
      {
        DateAchie2.IsVisible = false;
        TimeAchie2.IsVisible = false;
      }

      if (signature)
      {
        btnSignature.IsVisible = true;
      }
      else
      {
        btnSignature.IsVisible = false;
        btnSignature.WidthRequest = 0;
      }

      SetData();
    }

    private void SetData()
    {
      BtnOrder.Text = _actOrderAchievement.OrderNrDesc;
      btnAchievement.Text = _actOrderAchievement.AchieName;

      if (_actOrderAchievement.CostNrDesc != null)
        btnCost.Text = _actOrderAchievement.CostNrDesc;

      _setTime = false;
      DateAchie.Date = _actOrderAchievement.DateTimeAchie.Date;
      TimeAchie.Time = new TimeSpan(_actOrderAchievement.DateTimeAchie.TimeOfDay.Ticks);

      DateAchie2.Date = _actOrderAchievement.DateTimeAchie.AddHours((double)_actOrderAchievement.Amount).Date;
      TimeAchie2.Time = new TimeSpan(_actOrderAchievement.DateTimeAchie.AddHours((double)_actOrderAchievement.Amount).TimeOfDay.Ticks);
      _setTime = true;

      Menge.Text = _actOrderAchievement.Amount.ToString("0.00");
      Unit.Text = _actOrderAchievement.Unit;
      Info.Text = _actOrderAchievement.Remark;

      if (_actOrderAchievement.FlagCharge)
        SwitchVerrechenbar.IsToggled = true;
      else
        SwitchVerrechenbar.IsToggled = false;

      switch (_actOrderAchievement.Status)
      {
        case 100:
          btnStopp.IsVisible = true;
          btnStart.IsVisible = false;
          break;
        case 200:
        case 300:
          btnStart.IsVisible = true;
          btnStopp.IsVisible = false;
          break;
        default:
          break;
      }
    }
    private void setMenge()
    {
      _actOrderAchievement.DateTimeAchie = new DateTime(DateAchie.Date.Year, DateAchie.Date.Month, DateAchie.Date.Day, TimeAchie.Time.Hours, TimeAchie.Time.Minutes, 0);
      DateTime dateTo = new DateTime(DateAchie2.Date.Year, DateAchie2.Date.Month, DateAchie2.Date.Day, TimeAchie2.Time.Hours, TimeAchie2.Time.Minutes, 0);

      if (_actOrderAchievement.Status == 100)
      {
        dateTo = DateTime.Now;
      }

      if (_actOrderAchievement.Unit == "h")
      {
        TimeSpan timeSpan = dateTo - _actOrderAchievement.DateTimeAchie;
        Double l = timeSpan.TotalHours;
        _actOrderAchievement.Amount = Convert.ToDecimal(l);
        Menge.Text = _actOrderAchievement.Amount.ToString("0.00");
      }

    }

    private async void BtnOrder_Clicked(object sender, EventArgs e)
    {
      try
      {
        var modalPage = new OrderSearch();
        modalPage.Disappearing += (sender2, e2) =>
        {
          if (modalPage._actOrder != null)
          {
            _actOrderAchievement.IdOrder = modalPage._actOrder.Id;
            _actOrderAchievement.OrderNrDesc = modalPage._actOrder.OrderNumber + " - " + modalPage._actOrder.Description;
            BtnOrder.Text = _actOrderAchievement.OrderNrDesc;
          }

        };
        await Navigation.PushModalAsync(modalPage);
      }
      catch (Exception ex)
      {
        string s = ex.Message;
        throw;
      }
    }
    private async void btnAchievement_Clicked(object sender, EventArgs e)
    {
      try
      {
        var modalPage = new AchievementSearch();
        modalPage._actOrderId = _actOrderAchievement.IdOrder;
        modalPage.Disappearing += (sender2, e2) =>
        {
          if (modalPage._actAchievement != null)
          {
            _actOrderAchievement.IdAchievement = modalPage._actAchievement.Id;
            _actOrderAchievement.AchieNumber = modalPage._actAchievement.AchieNumber;
            _actOrderAchievement.AchieName = modalPage._actAchievement.AchieName;
            _actOrderAchievement.Unit = modalPage._actAchievement.Unit;
            //btnAchievement.Text = _actOrderAchievement.AchieName;
            SetData();
          }
        };
        await Navigation.PushModalAsync(modalPage);

      }
      catch (Exception ex)
      {
        string s = ex.Message;
        throw;
      }
    }
    private async void btnCost_Clicked(object sender, EventArgs e)
    {
      try
      {
        var modalPage = new CostSearch();
        modalPage.Disappearing += (sender2, e2) =>
        {
          if (modalPage._costUnit != null)
          {
            _actOrderAchievement.IdCostUnit = modalPage._costUnit.Id;
            _actOrderAchievement.CostNrDesc = modalPage._costUnit.BST + " " + modalPage._costUnit.Description;
            SetData();
          }
        };
        await Navigation.PushModalAsync(modalPage);

      }
      catch (Exception ex)
      {
        string s = ex.Message;
        throw;
      }
    }

    private void TimeAchie_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == TimePicker.TimeProperty.PropertyName && _setTime)
        setMenge();
    }
    private void TimeAchie2_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == TimePicker.TimeProperty.PropertyName && _setTime)
        setMenge();
    }

    private async void btnStart_Clicked(object sender, EventArgs e)
    {
      waitCursor.IsVisible = true;
      waitCursor.IsRunning = true;

      App.restManager = new RestManager(new Web.RestService());

      try
      {
        _actOrderAchievement.IsActDay = true;
        var list = await App.restManager.GetNewOrderAchievementAsync(_actOrderAchievement.IdOrder.ToString(), _actOrderAchievement.IdAchievement.ToString(), true, true, _actOrderAchievement.IdCostUnit.ToString());
        if (list != null)
        {
          DependencyService.Get<IMessage>().ShortAlert("Daten gespeichert.");
          await Navigation.PopAsync();
        }
      }
      catch (Exception ex)
      {
        DependencyService.Get<IMessage>().ShortAlert(ex.Message);
      }

      waitCursor.IsVisible = false;
      waitCursor.IsRunning = false;

    }
    private async void btnStopp_Clicked(object sender, EventArgs e)
    {
      waitCursor.IsVisible = true;
      waitCursor.IsRunning = true;

      App.restManager = new RestManager(new Web.RestService());

      try
      {
        var list = await App.restManager.StartStopAsync(_actOrderAchievement.Id.ToString(), false);
        if (list != null)
        {
          DependencyService.Get<IMessage>().ShortAlert("Daten gespeichert.");
          await Navigation.PopAsync();
        }
      }
      catch (Exception ex)
      {
        DependencyService.Get<IMessage>().ShortAlert(ex.Message);
      }

      waitCursor.IsVisible = false;
      waitCursor.IsRunning = false;

    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
      _actOrderAchievement.DateTimeAchie = new DateTime(DateAchie.Date.Year, DateAchie.Date.Month, DateAchie.Date.Day, TimeAchie.Time.Hours, TimeAchie.Time.Minutes, 0);
      _actOrderAchievement.Amount = Convert.ToDecimal(Menge.Text);

      if (SwitchVerrechenbar.IsToggled)
        _actOrderAchievement.FlagCharge = true;
      else
        _actOrderAchievement.FlagCharge = false;

      if (Info.Text.Length > 0)
        _actOrderAchievement.Remark = Info.Text;

      _actOrderAchievement.IsNew = false;

      // Basic-http
      App.restManager = new RestManager(new Web.RestService());
      await App.restManager.UpdateOrderAchievement(_actOrderAchievement);

      DependencyService.Get<IMessage>().ShortAlert("Daten gespeichert.");

      await Navigation.PopAsync();

    }
    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
      App.restManager = new RestManager(new Web.RestService());
      await App.restManager.DeleteOrderAchievement(_actOrderAchievement);

      DependencyService.Get<IMessage>().ShortAlert("Daten gelöscht.");

      await Navigation.PopAsync();
    }
    private async void btnAbort_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopAsync();
    }

    private async void btnSignature_Clicked(object sender, EventArgs e)
    {
      try
      {
        var modalPage = new Signature(_actOrderAchievement);
        modalPage.Disappearing += (sender2, e2) =>
        {
          if (modalPage._actOrderAchievement != null)
          {
            _actOrderAchievement.IdSignature = modalPage._actOrderAchievement.IdSignature;
          }

        };
        await Navigation.PushModalAsync(modalPage);
      }
      catch (Exception ex)
      {
        string s = ex.Message;
        throw;
      }
    }
  }
}