using AppTimedoc.Helpers;
using AppTimedoc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppTimedoc.Data
{
  public class RestManager
  {
    IRestService restService;

    public RestManager(IRestService service)
    {
      restService = service;
    }

    public Task<Coworker> GetCoworkerAsync(string name)
    {
      return restService.GetCoworkerAsync(name);
    }

    public Task<WorkingTime> GetWorkingTimeCoworkerAsync(Coworker user)
    {
      return restService.GetWorkingTimeCoworkerAsync(user);
    }

    public Task<List<WorkingTime>> GetWorkingTimeAsync(DateTime date)
    {
      return restService.GetWorkingTimeAsync(date);
    }

    public Task<bool> UpdateWorkingTimeAsync(WorkingTime workingTime)
    {
      return restService.UpdateWorkingTimeAsync(workingTime);
    }
    public Task DeleteWorkingTimeAsync(Guid Id)
    {
      return restService.DeleteWorkingTimeAsync(Id);
    }

    // Leistungserfassung
    public Task<List<OrderAchievement>> GetNewOrderAchievementAsync(string IdOrder, string IdAchievement, bool start, bool listDetail, string idCostUni)
    {
      return restService.GetNewOrderAchievementAsync(IdOrder, IdAchievement, start, listDetail, idCostUni);
    }
    public Task<List<OrderAchievement>> StartStopAsync(string IdOrderAchievement, bool start)
    {
      return restService.StartStopAsync(IdOrderAchievement, start);
    }
    public Task<List<OrderAchievement>> GetListOrderAchievementAsync(DateTime day, bool listDetail)
    {
      return restService.GetListOrderAchievementAsync(day, listDetail);
    }
    public Task<bool> UpdateOrderAchievement(OrderAchievement orderAchievement)
    {
      return restService.UpdateOrderAchievement(orderAchievement);
    }
    public Task<bool> DeleteOrderAchievement(OrderAchievement orderAchievement)
    {
      return restService.DeleteOrderAchievement(orderAchievement);
    }

    public Task<List<Order>> GetOrderList(string search)
    {
      return restService.GetOrderList(search);
    }
    public Task<List<Achievement>> GetAchievementList(string idOrder, string search, bool filterPosition)
    {
      return restService.GetAchievementList(idOrder, search, filterPosition);
    }

    public Task<Setting> GetSettingAsync(string name)
    {
      return restService.GetSettingAsync(name);
    }

    public Task<List<CostUnit>> GetCostUnitAsync(string search)
    {
      return restService.GetCostUnitAsync(search);
    }
    public Task<bool> SetSignatureAsync(CSignature unterschrift)
    {
      return restService.SetSignatureAsync(unterschrift);
    }
    public Task<CSignature> GetSignatureAsync(string IdSignature)
    {
      return restService.GetSignatureAsync(IdSignature);
    }
    public Task<bool> DeleteSignatureAsync(string IdSignature)
    {
      return restService.DeleteSignatureAsync(IdSignature);
    }

    public Task<bool> SetSignatureAssign(string IdSignature, string IdOrder, DateTime dateAssign)
    {
      return restService.SetSignatureAssign(IdSignature, IdOrder, dateAssign);
    }
    public Task<bool> DeleteSignatureAssign(string IdSignature, string IdOrder, DateTime dateAssign)
    {
      return restService.DeleteSignatureAssign(IdSignature, IdOrder, dateAssign);
    }


  }
}
