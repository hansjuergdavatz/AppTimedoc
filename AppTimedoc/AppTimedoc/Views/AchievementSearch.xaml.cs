using AppTimedoc.Data;
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
	public partial class AchievementSearch : ContentPage
	{
    List<Achievement> list = null;
    public Achievement _actAchievement = null;
    public Guid _actOrderId = Guid.Empty;

    public AchievementSearch()
    {
      InitializeComponent();
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
      try
      {
        App.restManager = new RestManager(new Web.RestService());
        list = await App.restManager.GetAchievementList(_actOrderId.ToString(), txtSearch.Text, false);
        if (list != null)
        {
          AchievementListView.ItemsSource = list;
          SetDisplayText();
        }
      }
      catch (Exception)
      {
        AchievementListView.ItemsSource = null;
      }
    }

    private void SetDisplayText()
    {
      foreach (Achievement item in list)
      {
        item.TxtLarge = item.AchieNumber;
        item.TxtSmall = item.AchieName + " " + item.Unit;
      }
    }

    private async void AchievementListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      _actAchievement = e.SelectedItem as Achievement;
      if (_actAchievement == null)
        return;

      await Navigation.PopModalAsync();
    }

    private async void btnAbort_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopModalAsync();
    }
  }
}