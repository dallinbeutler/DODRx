using System;
using System.Collections.Generic;
using System.Text;
using DOD;

namespace Components
{
   public struct InvItem
   {
      public long ID { get; private set; }
      public string ImgSource { get; set; }
   }
   public class SysInventory : LazyDataStream<long, InvItem>
   {
      public SysInventory():base()
      {

      }
   }
}
