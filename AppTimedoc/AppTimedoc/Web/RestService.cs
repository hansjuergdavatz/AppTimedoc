using AppTimedoc.Data;
using AppTimedoc.Helpers;
using AppTimedoc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppTimedoc.Web
{
  public class RestService : IRestService
  {
    HttpClient client;

    public List<WorkingTime> Items { get; private set; }

    public RestService()
    {
      CoworkerStorage coStore = new CoworkerStorage();
      Coworker savedCoworker = coStore.LoadCoworker();
      if (savedCoworker == null)
        return;

      SetClient(savedCoworker.LoginId, savedCoworker.Password);

    }
    public RestService(string loginId, string password)
    {
      SetClient(loginId, password);
    }
    private void SetClient(string loginId, string password)
    {
      var authData = string.Format("{0}:{1}", loginId, password);
      var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
    }

    // Zeiterfassung
    public async Task<Coworker> GetCoworkerAsync(string loginName)
    {
      Coworker coworker = null;

      // http://localhost:63491/api/coworker?loginName=h.davatz%40gmx.ch
      string resource = String.Format("{0}coworker?loginname={1}", Constants.RestUrl, loginName);

      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          coworker = JsonConvert.DeserializeObject<Coworker>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return coworker;
    }
    public async Task<WorkingTime> GetWorkingTimeCoworkerAsync(Coworker coworker)
    {
      WorkingTime workingTime = null;

      // http://localhost:63491/api/workingTime?IdMandant=8dc0122e-5a47-44d5-a0ef-02325c5d7956&IdCoworker=b639dacf-8307-4ef0-9bea-7addcb8bbe62
      string resource = String.Format("{0}workingTime?IdMandant={1}&IdCoworker={2}", Constants.RestUrl, coworker.IdMandant, coworker.Id);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          workingTime = JsonConvert.DeserializeObject<WorkingTime>(content);
        }
        else
        {
          WorkingTime wt = new WorkingTime();
          workingTime = wt;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return workingTime;
    }
    public async Task<List<WorkingTime>> GetWorkingTimeAsync(DateTime date)
    {
      List<WorkingTime> list = null;

      // http://localhost:63491/api/workingTime/list?date=2017-07-12&displayOldRunning=1
      string resource = String.Format("{0}workingTime/list?date={1}&displayOldRunning=1", Constants.RestUrl, date.ToString("yyyy-MM-dd"));
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<WorkingTime>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }
    public async Task<bool> UpdateWorkingTimeAsync(WorkingTime workingTime)
    {

      // RestUrl = http://localhost:63491/api/workingTime

      string resource = String.Format("{0}workingTime", Constants.RestUrl);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var json = JsonConvert.SerializeObject(workingTime);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = null;

        response = await client.PostAsync(uri, content);

        if (response.IsSuccessStatusCode)
        {
          Debug.WriteLine(@"				WorkingTime successfully saved.");
          return true;
        }
        return false;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
        return false;
      }
    }
    public async Task DeleteWorkingTimeAsync(Guid id)
    {
      // http://localhost:63491/api/workingTime?idWorkingTime=8289714b-9e05-443f-ad62-aca20f5ded00
      string resource = String.Format("{0}workingTime?idWorkingTime={1}", Constants.RestUrl, id);
      var uri = new Uri(string.Format(resource, string.Empty));

      //string resource = String.Format("{0}workingTime", Constants.RestUrl);

      try
      {
        var response = await client.DeleteAsync(uri);

        if (response.IsSuccessStatusCode)
        {
          Debug.WriteLine(@"				WorkingTime successfully deleted.");
        }

      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
    }

    // Leistungserfassung
    public async Task<List<OrderAchievement>> GetNewOrderAchievementAsync(string idOrder, string idAchievement, bool start, bool listDetail, string idCostUnit)
    {
      // http://localhost:63491/api/orderAchievement/neu?idOrder=00000000-0000-0000-0000-000000000000&idAchievement=00000000-0000-0000-0000-000000000000&start=true&listDetail=true&sortDesc=true

      List<OrderAchievement> list = null;
      string resource = string.Empty;
      string guidEmpty = "00000000-0000-0000-0000-000000000000";

      if (idAchievement.Length == 0 && idOrder.Length == 0)
        resource = String.Format("{0}orderAchievement/neu?idOrder={1}&idAchievement={2}&start={3}&listDetail={4}&sortDesc=true", Constants.RestUrl, guidEmpty, guidEmpty, start, listDetail);
      else
        resource = String.Format("{0}orderAchievement/neu?idOrder={1}&idAchievement={2}&start={3}&listDetail={4}&idCostUnit={5}&sortDesc=true", Constants.RestUrl, idOrder, idAchievement, start, listDetail, idCostUnit);

      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var cont = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        var response = await client.PutAsync(uri, cont);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<OrderAchievement>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }

    public async Task<List<OrderAchievement>> StartStopAsync(string idOrderAchievement, bool start)
    {
      // http://localhost:63491/api/orderAchievement/start/stop?idOrderAchievement=f1f99fdc-3643-42ad-9ffa-4367c147477e&Start=true&listDetail=true&sortDesc=true

      List<OrderAchievement> list = null;
      string resource = String.Format("{0}orderAchievement/start/stop?idOrderAchievement={1}&start={2}&listDetail=true&sortDesc=true", Constants.RestUrl, idOrderAchievement, start);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var cont = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        var response = await client.PutAsync(uri, cont);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<OrderAchievement>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }


    public async Task<List<OrderAchievement>> GetListOrderAchievementAsync(DateTime day, bool listDetail)
    {
      // http://localhost:63491/api/orderAchievement/list/detail?start=2017-11-10
      // http://localhost:63491/api/orderAchievement/list/cumulated?start=2017-11-10
      // http://localhost:63491/api/orderAchievement/list/detail?start=2017-11-17&sortDesc=true

      List<OrderAchievement> list = null;

      string ListTyp = "cumulated";
      if (listDetail)
        ListTyp = "detail";

      string resource = String.Format("{0}orderAchievement/list/{1}?&start={2}&sortDesc=true", Constants.RestUrl, ListTyp, day.ToString("yyyy-MM-dd"));
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<OrderAchievement>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }
    public async Task<bool> UpdateOrderAchievement(OrderAchievement orderAchievement)
    {

      // RestUrl = http://localhost:63491/api/orderAchievement

      string resource = String.Format("{0}orderAchievement", Constants.RestUrl);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var json = JsonConvert.SerializeObject(orderAchievement);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = null;

        response = await client.PostAsync(uri, content);

        if (response.IsSuccessStatusCode)
        {
          Debug.WriteLine(@"				OrderAchievement successfully saved.");
          return true;
        }
        return false;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
        return false;
      }
    }
    public async Task<bool> DeleteOrderAchievement(OrderAchievement orderAchievement)
    {

      // RestUrl = http://localhost:63491/api/orderAchievement?idOrderAchievement=e156db40-97e2-49c6-9a03-3f3bf05f38f3

      string resource = String.Format("{0}orderAchievement?idOrderAchievement={1}", Constants.RestUrl, orderAchievement.Id);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        //var json = JsonConvert.SerializeObject(orderAchievement);
        //var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = null;

        response = await client.DeleteAsync(uri);

        if (response.IsSuccessStatusCode)
        {
          Debug.WriteLine(@"				OrderAchievement successfully saved.");
          return true;
        }
        return false;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
        return false;
      }
    }

    public async Task<List<Order>> GetOrderList(string search)
    {
      List<Order> list = null;

      // http://localhost:63491/api/order/list/readcount?SearchValue=Manufacture%20Wolf%20SA
      string resource = String.Format("{0}order/list?SearchValue={1}", Constants.RestUrl, search);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<Order>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }
    public async Task<List<Achievement>> GetAchievementList(string idOrder, string searchValue, bool filterPosition)
    {
      List<Achievement> list = null;

      // http://localhost:63491/api/achivement/order/list?IdOrder=3CFD45D0-6674-4741-A5FB-852699981E23&SearchValue=spe&filterPosition=false

      string resource = String.Format("{0}achivement/order/list?IdOrder={1}&SearchValue={2}&filterPosition={3}", Constants.RestUrl, idOrder, searchValue, filterPosition);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<Achievement>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }

    public async Task<Setting> GetSettingAsync(string name)
    {
      Setting setting = null;

      // http://localhost:63491/api/setting?name=CostUnit
      string resource = String.Format("{0}setting?name={1}", Constants.RestUrl, name);

      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();


          setting = JsonConvert.DeserializeObject<Setting>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return setting;
    }

    public async Task<List<CostUnit>> GetCostUnitAsync(string name)
    {
      List<CostUnit> list = null;

      // http://localhost:63491/api/costunit/list?search=4
      string resource = String.Format("{0}costunit/list?search={1}", Constants.RestUrl, name);

      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<CostUnit>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return list;
    }

    public async Task<bool> SetSignatureAsync(CSignature unterschrift)
    {
      // http://localhost:63491/api/signature

      string resource = String.Format("{0}signature", Constants.RestUrl);
      var uri = new Uri(string.Format(resource, unterschrift));

      try
      {
        var json = JsonConvert.SerializeObject(unterschrift);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = null;

        response = await client.PostAsync(uri, content);
        if (response.IsSuccessStatusCode)
        {
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return false;
    }
    public async Task<CSignature> GetSignatureAsync(string idSignature)
    {
      CSignature signature = null;

      string resource = String.Format("{0}signature?idSignature={1}", Constants.RestUrl, idSignature);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();
          signature = JsonConvert.DeserializeObject<CSignature>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return signature;
    }
    public async Task<bool> DeleteSignatureAsync(string idSignature)
    {
      string resource = String.Format("{0}signature?idSignature={1}", Constants.RestUrl, idSignature);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.DeleteAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return false;
    }
    public async Task<bool> SetSignatureAssign(string IdSignature, string IdOrder, DateTime dateAssign)
    {
      // http://localhost:63491/api/signature

      string dateString = string.Empty;
      if (dateAssign != DateTime.MinValue)
        dateString = dateAssign.ToString("yyyy-MM-dd");

      string resource = String.Format("{0}signature/toAssign?idSignature={1}&idOrder={2}&dateSignature={3}", Constants.RestUrl, IdSignature, IdOrder, dateString);
      var uri = new Uri(string.Format(resource, string.Empty));
      var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

      try
      {
        var response = await client.PostAsync(uri, content);
        if (response.IsSuccessStatusCode)
        {
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return false;
    }
    public async Task<bool> DeleteSignatureAssign(string IdSignature, string IdOrder, DateTime dateAssign)
    {
      // http://localhost:63491/api/signature

      string dateString = string.Empty;
      if (dateAssign != DateTime.MinValue)
        dateString = dateAssign.ToString("yyyy-MM-dd");

      string resource = String.Format("{0}signature/toAssign?idSignature={1}&idOrder={2}&dateSignature={3}", Constants.RestUrl, IdSignature, IdOrder, dateString);
      var uri = new Uri(string.Format(resource, string.Empty));

      try
      {
        var response = await client.DeleteAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return false;
    }

  }
}
