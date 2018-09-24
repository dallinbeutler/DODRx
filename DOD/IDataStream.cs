using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ConcurrentCollections;

namespace DOD
{
   public interface IDataStream<Key> //: Enumerable
   {
      bool IsInitialized { get; set; }
      string Name { get; }
      int Count { get; }
      bool HasEntity(Key ID);
      void Clear();
      bool RemoveAt(Key ID);
      void Set(Key ID, object o);
      object Get(Key ID);
      object GetOrAdd(Key ID);
      object GetOrDefault(Key ID);
      event NotifyDataStreamChangedEventHandler<Key> DSChanged;
      IObservable<EntityChangedArgs<Key>> AsObservable { get; }
   }

   public enum BitFlags
   {
      solid = 1,

   }
}
   
