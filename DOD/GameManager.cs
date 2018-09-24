using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace DOD
{
   public class GameManager
   {
      private long CurID;

      public DataStream<string, IDataStream<long>> Data { get; set; } = new DataStream<string, IDataStream<long>>();

      public IDataStream<long> this[string CompName]
      {
         get
         {
            return Data[CompName];

         }
         set
         {
            Data[CompName] = value;
         }
      }
      public object this[string CompName, long Entity]
      {
         get
         {
            return Data[CompName].GetOrDefault(Entity);

         }
         set
         {
            Data[CompName].Set(Entity, value);
         }
      }
      public void AddComps(params IDataStream<long>[] comps )
      {
         foreach(var c in comps)
         {
            Data[c.Name] = c;
         }
      }
      public long getUniqueID()
      {
         return Interlocked.Increment(ref CurID);
      }

      public IEnumerable<KeyValuePair<string,IDataStream<long>>> GetEntityComps(long ID)
      {
         return Data.AsEnumerable().Where(x => x.Value.HasEntity(ID));
      }

      public IEnumerable<Tuple<string, IDataStream<long>,object>> GetEntityCompValues(long ID)
      {
         return Data.AsEnumerable().Where(x => x.Value.HasEntity(ID)).Select(x=> 
         {
            return Tuple.Create(x.Key, x.Value, x.Value.Get(ID));
         });
      }

   }
}
