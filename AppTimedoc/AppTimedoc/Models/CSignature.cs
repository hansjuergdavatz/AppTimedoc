using System;
using System.Collections.Generic;
using System.Text;

namespace AppTimedoc.Models
{
  public class CSignature
  {
    public Guid Id { get; set; }
    public byte[] Signature1 { get; set; }
    public byte[] Signature2 { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }

  }
}
