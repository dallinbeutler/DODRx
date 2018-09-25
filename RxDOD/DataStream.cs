using System;
using System.Collections.Generic;
using System.Text;
using DynamicData;


namespace RxDOD
{
   class DataStream<T> : SourceCache<KeyValuePair<long, T>, long>
   {
      public DataStream() : base( x=>x.Key)
      {
         
      }
   }

   //public class IDataStream
   //{
   //   long ID { get; set; }
   //}
}
