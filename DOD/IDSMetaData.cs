using System;
using System.Collections.Generic;
using System.Text;

namespace DOD
{
   
   public class IDSMetaData
   {
      [System.ComponentModel.DefaultValue("Name MetaData Not Provided!")]
      public string Name { get; set; } = "FUNCKY";

      public object DefaultValue { get; set; }
   }
}
