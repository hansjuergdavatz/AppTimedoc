using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimedoc.Models
{
  [Table("WorkingTimes")]
  public class WorkingTime //: INotifyPropertyChanged
  {
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid IdCoworker { get; set; }
    public Guid IdAchievement { get; set; }
    public Guid IdMandant { get; set; }
    private DateTime _comeTime;
    public DateTime ComeTime
    {
      get
      {
        return _comeTime;
      }
      set
      {
        this._comeTime = value;
        //OnPropertyChanged(nameof(ComeTime));
      }
    }
    private DateTime _goTime;
    public DateTime? GoTime
    {
      get
      {
        return _goTime;
      }
      set
      {
        if (value == null)
          this._goTime = DateTime.MinValue;
        else
          this._goTime = (DateTime)value;
        //OnPropertyChanged(nameof(GoTime));
      }
    }
    private string _remark;
    public string Remark
    {
      get
      {
        return _remark;
      }
      set
      {
        this._remark = value;
        //OnPropertyChanged(nameof(Remark));
      }
    }
    public bool ManuelInput { get; set; }
    public bool Confirmed { get; set; }
    public bool Error { get; set; }
    public string ErrorText { get; set; }
    public string AchieName { get; set; }
    public DateTime TimeDate { get; set; }
    public string TimeAmount { get; set; }
    public decimal? Duration { get; set; }
    public string OrderAchieAmount { get; set; }
    public string OrderAchieAchieName { get; set; }
    public int DayType { get; set; }
    public bool IsDuplicate { get; set; }
    public Guid IdAbsenceRequest { get; set; }
    public string CheckError { get; set; }

    public string DurationTxt { get; set; }
    public bool Post { get; set; }
    public string PostChange { get; set; }
    public string PostDelete { get; set; }
    public bool MaxDayTime { get; set; }

    public string Info { get; set; }

    public DateTime ComeTimeOriginal { get; set; }
    public DateTime GoTimeOriginal { get; set; }

    public bool IsNew { get; set; }

    public string TxtLarge { get; set; }
    public string TxtSmall { get; set; }

    //public event PropertyChangedEventHandler PropertyChanged;
    //private void OnPropertyChanged(string propertyName)
    //{
    //  this.PropertyChanged?.Invoke(this,
    //    new PropertyChangedEventArgs(propertyName));
    //}


  }
}
