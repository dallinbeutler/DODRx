using System;
using System.Collections.Generic;
using System.Text;

namespace DOD
{
   public class CDS<T> : DataStream<long, T>
   {
      public CDS(string name, T defaultval):base(Name:name, defaultValue:defaultval)
      {

      }


   }
}
