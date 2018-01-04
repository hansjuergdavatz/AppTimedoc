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
	public partial class OrderSearch : ContentPage
	{
    List<Order> list = null;
    public Order _actOrder = null;


    public OrderSearch()
    {
      InitializeComponent();
    }

    private async void btnSearch_Clicked(object sender, EventArgs e)
    {
      try
      {
        if (txtSearch.Text.Length == 0)
        {
          DependencyService.Get<IMessage>().ShortAlert("Filter muss eingegeben werden.");
          return;
        }

        App.restManager = new RestManager(new Web.RestService());
        list = await App.restManager.GetOrderList(txtSearch.Text);
        if (list != null)
        {
          OrderListView.ItemsSource = list;
          SetDisplayText();
        }
      }
      catch (Exception)
      {
        OrderListView.ItemsSource = null;
      }
    }

    private void SetDisplayText()
    {
      foreach (Order item in list)
      {
        item.TxtLarge = item.OrderNumber;
        item.TxtSmall = item.Description;
      }
    }


    private async void OrderListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      _actOrder = e.SelectedItem as Order;
      if (_actOrder == null)
        return;

      await Navigation.PopModalAsync();
    }

    private async void btnAbort_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopModalAsync();
    }

  }
}