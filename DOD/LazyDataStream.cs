using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace DOD
{
   public delegate void InitializedDSEventHandler<Key>(IDataStream<Key> sender);
   public class LazyDataStream<Key,T> : DataStream<Key,T>
   {
      public new T DefaultVal { get; }
      public new string Name { get; }
      public new bool IsInitialized { get; private set; }
      public event InitializedDSEventHandler<Key> Initialized;
      private Func<IEnumerable<KeyValuePair<Key, T>>> Ctor { get; }
      public LazyDataStream(Func<IEnumerable<KeyValuePair<Key, T>>> ctor = null, T defaultValue = default(T), string name = null)
      {
         this.Ctor = ctor;
         this.Initialized += LazyDataStream_Initialized;
         this.DefaultVal = defaultValue;
         this.Name = name?? this.GetType().Name;

         this.EntityChanged += DataStream_EntityChanged;
         this.DataStreamChanged += DataStream_DataStreamChanged;

         AsObservable = Observable
         .FromEventPattern<EntityChangedArgs<Key>>(this, "DataStreamChanged")
         .Select(change => change.EventArgs);

         AsObservableDetails = Observable
         .FromEventPattern<DSChangedArgs<Key, T>>(this, "EntityChanged")
         .Select(change => change.EventArgs);
      }
      public void Initialize()
      {
         Initialized.Invoke(this);
      }

      private void LazyDataStream_Initialized(IDataStream<Key> sender)
      {
         if (Ctor != null)
            DataSet = new ConcurrentDictionary<Key, T>(Ctor.Invoke());
         else
            DataSet = new ConcurrentDictionary<Key, T>();
         IsInitialized = true;
         Console.WriteLine(Name + " Initialized");
      }
   }
}
