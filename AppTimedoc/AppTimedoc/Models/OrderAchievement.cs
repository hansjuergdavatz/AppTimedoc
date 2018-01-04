using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppTimedoc.Models
{
  [Table("OrderAchievement")]
  public class OrderAchievement //: INotifyPropertyChanged
  {
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid IdMandant { get; set; }
    public Guid IdOrder { get; set; }
    public Guid IdAchievement { get; set; }
    public Guid IdCoworker { get; set; }
    public DateTime DateTimeAchie { get; set; }
    public DateTime? DateTimeAchie2 { get; set; }
    public decimal Amount { get; set; }
    public int Status { get; set; }
    public string Remark { get; set; }
    public string Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public bool FlagCharge { get; set; }
    public string FlagUnit { get; set; }
    public int GroupNr { get; set; }
    public Guid? IdTransfer { get; set; }
    public Guid? IdTask { get; set; }
    public bool TransferConfirmed { get; set; }

    public string OrderNumber { get; set; }
    public string OrderDescription { get; set; }
    public string AchieName { get; set; }
    public string AmountInfo { get; set; }
    public string DateAchie { get; set; }
    public string UserName { get; set; }
    public string OrderRemark { get; set; }
    public string OrderNrDesc { get; set; }
    public string Start { get; set; }
    public string Stop { get; set; }
    public string AmountTime { get; set; }

    public string HostUnit { get; set; }
    public string AchieNumber { get; set; }
    public string UserNameShort { get; set; }
    public string AchieHostId { get; set; }
    public bool IsNew { get; set; }
    public string TxtLarge { get; set; }
    public string TxtSmall { get; set; }
    public string TxtSmall2 { get; set; }
    public string TxtSmall3 { get; set; }
    public string Image { get; set; }

    public Guid IdCostUnit { get; set; }
    public string CostNrDesc { get; set; }

    public bool IsActDay { get; set; }
    public Guid IdSignature { get; set; }
    public Guid IdSignatureOrder { get; set; }


    public OrderAchievement() { }

    //public event PropertyChangedEventHandler PropertyChanged;
    //private void OnPropertyChanged(string propertyName)
    //{
    //  this.PropertyChanged?.Invoke(this,
    //    new PropertyChangedEventArgs(propertyName));
    //}

  }
}
