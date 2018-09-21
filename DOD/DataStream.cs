using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;

namespace DOD
{
   public delegate void NotifyDataStreamChangedEventHandler<Key>(IDataStream<Key> sender, EntityChangedArgs<Key> args);
   public delegate void NotifyDataStreamChangedEventHandler<Key, T>(IDataStream<Key> sender, DSChangedArgs<Key, T> args);
   public class DSChangedArgs<Key, T> : EntityChangedArgs<Key>
   {
      public T NewVal { get; set; }

      public T OldVal { get; set; }

      public DSChangedArgs(Key entity, NotifyCollectionChangedAction action, T oldVal, T newVal) : base(entity, action)
      {
         OldVal = oldVal;
         NewVal = newVal;
      }
   }
   public class EntityChangedArgs<Key>
   {
      public Key Entity { get; set; }
      public NotifyCollectionChangedAction Action { get; set; }
      public EntityChangedArgs(Key entity, NotifyCollectionChangedAction action)
      {
         Entity = entity;
         Action = action;
      }
   }

   /// <summary>
   /// This class is basically for storing values in a way that is easier on memory, 
   /// while also generating events on property change that can be observed.
   /// </summary>
   ///  a name for the system. Used in debugging purposes as well as ensuring one instance of systems
   /// <typeparam name="T"> The datatype to be stored (structs are more memory efficient)</typeparam>
   /// <typeparam name="Key">The key for the dictionary lookup. DSManager uses longs</typeparam>
   public class DataStream<Key, T> : IDataStream<Key>//, IEnumerable<KeyValuePair<Key, T>>//, IDisposable //INotifyCollectionChanged<T>,
   {

      public string Name { get; }
      protected ConcurrentDictionary<Key, T> DataSet;
      public event NotifyDataStreamChangedEventHandler<Key, T> DSChangedDetails;
      public event NotifyDataStreamChangedEventHandler<Key> DSChanged;

      public IObservable<EntityChangedArgs<Key>> AsObservable { get; protected set; }
      public IObservable<DSChangedArgs<Key, T>> AsObservableDetails { get; protected set; }
      public T DefaultVal { get; protected set; }

      public bool IsInitialized { get; set; } = true;
      public Func<T> DefaultDataCtor { get; set; }

      public IEnumerable<KeyValuePair<Key,T>> AsEnumerable()
      {
         return DataSet.AsEnumerable();
      }

      /// <summary>
      /// Standard Datastream Constructor
      /// </summary>
      /// <param name="Name">Name for reporting purposes and unique storage into a dictionary. Defaults to type name if not provided, ensuring one</param>
      /// <param name="comps">bits to initialize with to be loaded in. null for an empty set. Can pass in dictionaries</param>
      public DataStream(string Name = null, IEnumerable<KeyValuePair<Key, T>> comps = null, T defaultValue = default(T))
      {
         DefaultVal = defaultValue;
         if (Name != null)
         {
            this.Name = Name;
         }
         else
         {
            this.Name = this.GetType().Name;
         }

         this.DSChangedDetails += DataStream_EntityChanged;
         this.DSChanged += DataStream_DataStreamChanged;
         if (comps != null)
            DataSet = new ConcurrentDictionary<Key, T>(comps);
         else
            DataSet = new ConcurrentDictionary<Key, T>();
         AsObservable = Observable
         .FromEventPattern<EntityChangedArgs<Key>>(this, "DSChanged")
         .Select(change => change.EventArgs);

         AsObservableDetails = Observable
         .FromEventPattern<DSChangedArgs<Key, T>>(this, "DSChangedDetails")
         .Select(change => change.EventArgs);
      }



      protected void DataStream_DataStreamChanged(IDataStream<Key> sender, EntityChangedArgs<Key> args)
      {
         // Method intentionally left empty. We can implement events if changed
      }

      protected Type ChangeArgs
      {
         get
         {
            return typeof(DSChangedArgs<Key, T>);
         }
      }

      protected void DataStream_EntityChanged(IDataStream<Key> sender, DSChangedArgs<Key, T> args)
      {
         DSChanged.Invoke(this, args);
      }



      public int Count
      {
         get
         {
            return DataSet.Count;
         }
      }

      public bool HasEntity(Key ID)
      {
         return DataSet.Keys.Contains(ID);
      }
      public T this[Key i]
      {
         get
         {
            //add safe way?
            return DataSet[i];
         }
         set
         {
            if (!DataSet.ContainsKey(i))
            {
               DataSet[i] = value;
               DSChangedDetails.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Add, default(T), value));
            }
            else if (!EqualityComparer<T>.Default.Equals(DataSet[i], value))
            {
               DataSet[i] = value;
               DSChangedDetails.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Replace, default(T), value));
            }
         }
      }

      public void AddRange(IEnumerable<KeyValuePair<Key,T>> keyValuePairs)
      {
         foreach(var kvp in keyValuePairs)
         {
            this[kvp.Key] = kvp.Value;
         }
      }





      public bool RemoveAt(Key ID)
      {
         if (DataSet.TryRemove(ID, out T val))
         {
            DSChangedDetails.Invoke(this, new DSChangedArgs<Key, T>(ID, NotifyCollectionChangedAction.Remove, val, default(T)));
            return true;
         }
         return false;
      }
      public T PopAt(Key i)
      {
         if (DataSet.TryRemove(i, out T val))
         {
            DSChangedDetails.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Remove, val, default(T)));
            return val;
         }
         return default(T);
      }
      public void Clear()
      {
         DataSet.Clear();
         DSChangedDetails.Invoke(this, new DSChangedArgs<Key, T>(default(Key), NotifyCollectionChangedAction.Reset, default(T), default(T))); 
      }


      public IEnumerator<KeyValuePair<Key, T>> GetEnumerator()
      {
         return DataSet.GetEnumerator();
      }

      public void Set(Key ID, object o)
      {
         if (o is T t)
            this[ID] = t;
         else
         {
            Console.WriteLine("Error converting " + o.GetType() + " to " + typeof(T));
         }
      }

      object IDataStream<Key>.Get(Key ID)
      {
         return this[ID];
      }

      public T Get(Key ID)
      {
         return this[ID];
      }


      object IDataStream<Key>.GetOrAdd(Key ID)
      {
         var outvar = DefaultDataCtor != null ? DefaultDataCtor.Invoke() : default(T);
         this[ID] = outvar;
         return outvar;
      }
      public T GetOrAdd(Key ID, T addval)
      {
         var outvar = DefaultDataCtor != null ? DefaultDataCtor.Invoke() : addval;
         this[ID] = outvar;
         return outvar;
      }
      public T GetOrAdd(Key ID)
      {
         var outvar = DefaultDataCtor != null ? DefaultDataCtor.Invoke() : DefaultVal;
         this[ID] = outvar;
         return outvar;
      }
      object IDataStream<Key>.GetOrDefault(Key ID)
      {
         if (DataSet.TryGetValue(ID, out T outval))
         {
            return outval;
         }
         else
         {
            return DefaultVal;
         }
      }

      public T GetOrDefault(Key ID)
      {
         if (DataSet.TryGetValue(ID, out T outval))
         {
            return outval;
         }
         else
         {
            return DefaultVal;
         }
      }
   }
}
