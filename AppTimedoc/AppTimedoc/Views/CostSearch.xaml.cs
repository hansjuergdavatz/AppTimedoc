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
	public partial class CostSearch : ContentPage
	{
    List<CostUnit> list = null;
    public CostUnit _costUnit = null;

    public CostSearch()
    {
      InitializeComponent();
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
      try
      {
        App.restManager = new RestManager(new Web.RestService());
        list = await App.restManager.GetCostUnitAsync(txtSearch.Text);
        if (list != null)
        {
          CostListView.ItemsSource = list;
          SetDisplayText();
        }
      }
      catch (Exception)
      {
        CostListView.ItemsSource = null;
      }

    }
    private void SetDisplayText()
    {
      foreach (CostUnit item in list)
      {
        item.TxtLarge = item.BST;
        item.TxtSmall = item.Description;
      }
    }


    private async void btnAbort_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopModalAsync();
    }

    private async void CostListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      _costUnit = e.SelectedItem as CostUnit;
      if (_costUnit == null)
        return;

      await Navigation.PopModalAsync();
    }
  }
}