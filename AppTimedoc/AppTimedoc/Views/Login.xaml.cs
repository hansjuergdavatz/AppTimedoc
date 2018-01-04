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
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
      SetUIHandlers();
    }

    private async void SetUIHandlers()
    {
      var _user = await App.Database.GetCoworker();

      if (_user == null)
        btnLogin.Text = "Anmelden";
      else
      {
        LoginId.Text = _user.LoginId;
        Password.Text = _user.Password;
        btnLogin.Text = "Abmelden";
      }
    }
    protected async override void OnAppearing()
    {
      base.OnAppearing();

      var _user = await App.Database.GetCoworker();
      if (_user == null || _user.IsValid == false)
      {
        LoginId.Text = string.Empty;
        Password.Text = string.Empty;
        btnLogin.Text = "Anmelden";
      }

    }

    public async Task OnLogin(object o, EventArgs e)
    {
      CoworkerStorage coStore = new CoworkerStorage();
      var user = await App.Database.GetCoworker();
      if (user == null)
      {
        Coworker coworker = new Coworker();
        // coworker.Id = Guid.NewGuid();
        coworker.LoginId = LoginId.Text;
        coworker.Password = Password.Text;

        // Basic-http
        App.restManager = new RestManager(new Web.RestService(coworker.LoginId.ToLower(), coworker.Password));
        Coworker rc = await App.restManager.GetCoworkerAsync(coworker.LoginId.ToLower());

        if (rc != null)
        {
          coworker.Id = rc.Id;
          coworker.IdMandant = rc.IdMandant;
          coworker.IsValid = true;
          coStore.SaveCoworker(coworker);
          btnLogin.Text = "Abmelden";
          DependencyService.Get<IMessage>().ShortAlert("Anmeldung erfolgreich.");
        }
        else
          DependencyService.Get<IMessage>().ShortAlert("Anmeldung nicht erfolgreich.");
      }
      else
      {
        LoginId.Text = string.Empty;
        Password.Text = string.Empty;
        btnLogin.Text = "Anmelden";
        coStore.DeleteCoworker(user);
      }

    }

  }
}
