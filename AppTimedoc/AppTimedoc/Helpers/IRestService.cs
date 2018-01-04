using AppTimedoc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppTimedoc.Helpers
{
  public interface IRestService
  {
    Task<Coworker> GetCoworkerAsync(string loginName);

    Task<WorkingTime> GetWorkingTimeCoworkerAsync(Coworker user);
    Task<List<WorkingTime>> GetWorkingTimeAsync(DateTime date);
    Task<bool> UpdateWorkingTimeAsync(WorkingTime item);
    Task DeleteWorkingTimeAsync(Guid id);

    // Leistungserfassung
    Task<List<OrderAchievement>> GetNewOrderAchievementAsync(string idOrder, string idAchievement, bool start, bool listDetail, string idCostUni);
    Task<List<OrderAchievement>> StartStopAsync(string idOrderAchievement, bool start);
    Task<List<OrderAchievement>> GetListOrderAchievementAsync(DateTime day, bool listDetail);
    Task<bool> UpdateOrderAchievement(OrderAchievement item);
    Task<bool> DeleteOrderAchievement(OrderAchievement item);

    Task<List<Order>> GetOrderList(string search);
    Task<List<Achievement>> GetAchievementList(string idOrder, string searchValue, bool filterPosition);

    Task<Setting> GetSettingAsync(string name);

    Task<List<CostUnit>> GetCostUnitAsync(string name);

    Task<bool> SetSignatureAsync(CSignature unterschrift);

    Task<CSignature> GetSignatureAsync(string IdSignature);

    Task<bool> DeleteSignatureAsync(string IdSignature);

    Task<bool> SetSignatureAssign(string IdSignature, string IdOrder, DateTime dateAssign);
    Task<bool> DeleteSignatureAssign(string IdSignature, string IdOrder, DateTime dateAssign);

  }
}
