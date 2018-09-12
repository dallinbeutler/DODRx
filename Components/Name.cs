using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
   [System.Composition.Export(typeof(DOD.IDataStream<long>))]
   public class NameComponentSystem : DOD.DataStream<long,string>
   {
      public NameComponentSystem(): base(defaultValue: "<UNNAMED>")
      {
         Console.WriteLine("name Comp!");
         
      }
   }
}
