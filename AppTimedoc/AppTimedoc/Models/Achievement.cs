using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimedoc.Models
{
  public class Achievement
  {
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid IdMandant { get; set; }
    public string AchieNumber { get; set; }
    public string AchieName { get; set; }
    public string Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public string TxtLarge { get; set; }
    public string TxtSmall { get; set; }
  }
}
