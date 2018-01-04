using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimedoc.Models
{
  public class Coworker
  {
    [PrimaryKey]
    public Guid Id { get; set; }
    public Guid IdMandant { get; set; }
    public string LoginId { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Email { get; set; }
    public bool IsValid { get; set; }
  }
}
