

namespace DODRx
{
   using System;
   using System.Collections;
   using System.Collections.Concurrent;
   using System.Collections.Generic;
   using System.Collections.Specialized;
   using System.Linq;
   using System.Reactive.Linq;


   public delegate void NotifyDataStreamChangedEventHandler<Key>(IDataStream<Key> sender, EntityChangedArgs<Key> args);
   public delegate void NotifyDataStreamChangedEventHandler<Key, T>(IDataStream<Key> sender, DSChangedArgs<Key, T> args);
   public class DSChangedArgs<Key, T> : EntityChangedArgs<Key>
   {
      public T OldVal;
      public T NewVal;
      public DSChangedArgs(Key entity, NotifyCollectionChangedAction action, T oldVal, T newVal) : base(entity, action)
      {
         OldVal = oldVal;
         NewVal = newVal;
      }
   }
   public class EntityChangedArgs<Key>
   {
      public Key Entity;
      public NotifyCollectionChangedAction Action;
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
   public class DataStream<Key, T> : IDataStream<Key>, IEnumerable<KeyValuePair<Key, T>>//, IDisposable //INotifyCollectionChanged<T>,
   {
      public string Name { get; }
      private Lazy<ConcurrentDictionary<Key, T>> DataSet;
      public event NotifyDataStreamChangedEventHandler<Key, T> EntityChanged;
      public event NotifyDataStreamChangedEventHandler<Key> DataStreamChanged;

      public IObservable<EntityChangedArgs<Key>> AsObservable { get; }
      public IObservable<DSChangedArgs<Key, T>> AsObservableDetails { get; }
      //public string Name { get; }
      public T DefaultVal { get; }

      /// <summary>
      /// Standard Datastream Constructor
      /// </summary>
      /// <param name="Name">Name for reporting purposes and unique storage into a dictionary. Defaults to type name if not provided, ensuring one</param>
      /// <param name="comps">bits to initialize with to be loaded in. null for an empty set. Can pass in dictionaries</param>
      public DataStream(string Name = null, IEnumerable<KeyValuePair<Key, T>> comps = null, T defaultValue = default(T))
      {
         DefaultVal = defaultValue;
         this.Name = Name ?? this.GetType().Name;

         this.EntityChanged += DataStream_EntityChanged;
         this.DataStreamChanged += DataStream_DataStreamChanged;

         DataSet = new Lazy<ConcurrentDictionary<Key, T>>(() => { return new ConcurrentDictionary<Key, T>(comps ?? new List<KeyValuePair<Key, T>>()); }, true);
         AsObservable = Observable
         .FromEventPattern<EntityChangedArgs<Key>>(this, "DataStreamChanged")
         .Select(change => change.EventArgs);

         AsObservableDetails = Observable
         .FromEventPattern<DSChangedArgs<Key, T>>(this, "EntityChanged")
         .Select(change => change.EventArgs);
      }



      private void DataStream_DataStreamChanged(IDataStream<Key> sender, EntityChangedArgs<Key> args)
      {
      }

      private Type ChangeArgs
      {
         get
         {
            return typeof(DSChangedArgs<Key, T>);
         }
      }

      private void DataStream_EntityChanged(IDataStream<Key> sender, DSChangedArgs<Key, T> args)
      {
         DataStreamChanged.Invoke(this, args);
      }



      public int Count
      {
         get
         {
            return DataSet.Value.Count;
         }
      }


      public bool HasEntity(Key ID)
      {
         return DataSet.Value.Keys.Contains(ID);
      }
      public T this[Key i]
      {
         get
         {
            //add safe way?
            return DataSet.Value[i];
         }
         set
         {
            if (!DataSet.Value.ContainsKey(i))
            {
               DataSet.Value[i] = value;
               EntityChanged.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Add, default(T), value));
            }
            else if (!EqualityComparer<T>.Default.Equals(DataSet.Value[i], value))
            {
               DataSet.Value[i] = value;
               EntityChanged.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Replace, default(T), value));//(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<int, T>(i, value)));
            }
         }
      }

      public void AddRange(IEnumerable<KeyValuePair<Key, T>> keyValuePairs)
      {
         foreach (var kvp in keyValuePairs)
         {
            this[kvp.Key] = kvp.Value;
         }
      }


      public T GetOrDefault(Key ID)
      {
         if (DataSet.Value.TryGetValue(ID, out T outval))
         {
            return outval;
         }
         else
         {
            return DefaultVal;
         }
      }
      public object GetObjOrDefault(Key ID)
      {
         if (DataSet.Value.TryGetValue(ID, out T outval))
         {
            return outval;
         }
         else
         {
            return DefaultVal;
         }
      }

      public bool RemoveAt(Key i)
      {
         T val;
         if (DataSet.Value.TryRemove(i, out val))
         {
            EntityChanged.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Remove, val, default(T)));//this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<int, T>(i, val)));
            return true;
         }
         return false;
      }
      public T PopAt(Key i)
      {
         T val;
         if (DataSet.Value.TryRemove(i, out val))
         {
            EntityChanged.Invoke(this, new DSChangedArgs<Key, T>(i, NotifyCollectionChangedAction.Remove, val, default(T)));//this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<int, T>(i, val)));
            return val;
         }
         return default(T);
      }
      public void Clear()
      {
         DataSet.Value.Clear();
         EntityChanged.Invoke(this, new DSChangedArgs<Key, T>(default(Key), NotifyCollectionChangedAction.Reset, default(T), default(T))); //this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }


      public IEnumerator<KeyValuePair<Key, T>> GetEnumerator()
      {
         return DataSet.Value.GetEnumerator();
      }

      //IEnumerator IEnumerable.GetEnumerator()
      //{
      //   return DataSet.GetEnumerator();
      //}

      public void Set(Key ID, object o)
      {
         if (o is T t)
            this[ID] = t;
         else
         {
            Console.WriteLine("Error converting " + o.GetType() + " to " + typeof(T));
         }
      }

      public object Get(Key ID)
      {
         return this[ID];
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return DataSet.Value.GetEnumerator();
      }

      //public virtual void Dispose()
      //{
      //   DataSet.Clear();
      //}
   }
}
