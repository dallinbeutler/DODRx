using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
//using System.ComponentModel.Composition;

namespace DOD
{
   public struct CompPair
   {
      public IDataStream<long> system { get; }
      public object Value { get; }
      public CompPair(IDataStream<long> System, object value)
      {
         this.system = System;
         this.Value = value;

         //Do we need to add to components list?
      }
   }
   public interface IDSManager
   {
      DataStream<long, List<IDataStream<long>>> Entities { get; }
      long AddEntity(params CompPair[] compPairs);
      void ComponentChange(IDataStream<long> sender, EntityChangedArgs<long> args);
      void KillEntity(long ID);
      IEnumerable<IDataStream<long>> GetComponents(long ID);
   }

   public abstract class DSManager : IDSManager 
   {
      private long CurID;
      public DataStream<long, List<IDataStream<long>>> Entities { get; } = new DataStream<long, List<IDataStream<long>>>();

      protected Dictionary<string, IDataStream<long>> ComponentSystems { get; }
      //public abstract void AddCompSystems(IEnumerable<T> datastreams);
      //public abstract long AddEntity(params CompPair[] compPairs);
      public DSManager(IEnumerable<KeyValuePair<string, IDataStream<long>>> systems)
      {
         if (systems != null)
            ComponentSystems = new Dictionary<string, IDataStream<long>>(systems.ToDictionary(x => x.Key, x => x.Value));
         else
            ComponentSystems = new Dictionary<string, IDataStream<long>>();
      }
      protected long getUniqueID()
      {
         return Interlocked.Increment(ref CurID);
      }

      //public abstract TT GetCompSys<TT>();
      //public abstract TT GetCompSys<TT>(string name);

      public TT GetCompSystem<TT>() where TT : IDataStream<long>
      {
         return (TT)ComponentSystems[typeof(TT).Name];
      }
      public TT GetCompSystem<TT>(string name) where TT : IDataStream<long>
      {
         return (TT)ComponentSystems[name];
      }

      private void ComponentSystems_DataStreamChanged(IDataStream<string> sender, EntityChangedArgs<string> args)
      {
         var myargs = args as DSChangedArgs<string, IDataStream<long>>;
         if (myargs.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
         {
            myargs.NewVal.DSChanged += ComponentChange;
         }
         else if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
         {
            myargs.NewVal.DSChanged -= ComponentChange;
         }
      }

      public void ComponentChange(IDataStream<long> sender, EntityChangedArgs<long> args)
      {
         if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
         {
            Entities[args.Entity].Add(sender);
         }
         if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
         {
            Entities[args.Entity].Remove(sender);
         }
      }

      public void AddCompSystems(IEnumerable<IDataStream<long>> datastreams)
      {
         foreach (var ds in datastreams)
         {
            ComponentSystems.Add(ds.Name, ds);
         }
      }

      public long AddEntity(params CompPair[] compPairs)
      {
         long ID = getUniqueID();

         foreach (CompPair p in compPairs)
         {
            Entities[ID] = new List<IDataStream<long>>();
            ComponentSystems[p.system.Name].Set(ID, p.Value);
         }
         return ID;
      }

      public TT GetCompSys<TT>() where TT : IDataStream<long>
      {
         return (TT)ComponentSystems[typeof(TT).Name];

      }
      public TT GetCompSys<TT>(string name) where TT : IDataStream<long>
      {
         return (TT)ComponentSystems[name];

      }
      public IDataStream<long> GetCompSys(string name)
      {
         return ComponentSystems[name];
      }

      public void KillEntity(long ID)
      {
         Entities[ID].ForEach(x => x.RemoveAt(ID));
      }

      /// <summary>
      /// adding or removing comps from this list will not change the entity
      /// </summary>
      /// <param name="ID"></param>
      /// <returns></returns>
      public IEnumerable<IDataStream<long>> GetComponents(long ID)
      {
         // we could just do this and get rid of Entity?
         //ComponentSystems.Where(x => x.Value.HasEntity(ID));

         return Entities[ID];
      }
      public static IEnumerable<KeyValuePair<string, Lazy<IDataStream<long>, IDSMetaData>>> MakeDict(IEnumerable<Lazy<IDataStream<long>, IDSMetaData>> systems)
      {
         return systems.ToDictionary(x => x.Metadata.Name, x => x);
      }
   }

   //public class DSManagerLazy : DSManager<Lazy<IDataStream<long>, IDSMetaData>>
   //{
   //   public DSManagerLazy(IEnumerable<KeyValuePair<string, Lazy<IDataStream<long>, IDSMetaData>>> systems) : base(systems)
   //   {
   //   }

   //   public override void AddCompSystems(IEnumerable<Lazy<IDataStream<long>, IDSMetaData>> datastreams)
   //   {
   //      foreach (var ds in datastreams)
   //      {
   //         ComponentSystems.Add(ds.Metadata.Name, ds);
   //      }
   //   }

   //   public override long AddEntity(params CompPair[] compPairs)
   //   {
   //      long ID = getUniqueID();

   //      foreach (CompPair p in compPairs)
   //      {
   //         Entities[ID] = new List<IDataStream<long>>();
   //         ComponentSystems[p.system.Name].Value.Set(ID, p.Value);
   //      }
   //      return ID;
   //   }

   //   public Lazy<IDataStream<long>, IDSMetaData> GetCompSys<TT>() where TT : IDataStream<long>
   //   {
   //      return ComponentSystems[typeof(TT).Name];
   //   }

   //   public Lazy<IDataStream<long>, IDSMetaData> GetCompSys<TT>(string name) where TT : IDataStream<long>
   //   {
   //      return ComponentSystems[name];
   //   }
   //   public TT GetCompSysActive<TT>(string name) where TT : IDataStream<long>
   //   {
   //      return (TT)ComponentSystems[name].Value;
   //   }
   //}

   //public class DSManagerActive : DSManager<IDataStream<long>>
   //{
   //   public DSManagerActive(IEnumerable<KeyValuePair<string, IDataStream<long>>> systems) : base(systems)
   //   {
   //   }

   //   public override void AddCompSystems(IEnumerable<IDataStream<long>> datastreams)
   //   {
   //      foreach (var ds in datastreams)
   //      {
   //         ComponentSystems.Add(ds.Name, ds);
   //      }
   //   }

   //   public override long AddEntity(params CompPair[] compPairs)
   //   {
   //      long ID = getUniqueID();

   //      foreach (CompPair p in compPairs)
   //      {
   //         Entities[ID] = new List<IDataStream<long>>();
   //         ComponentSystems[p.system.Name].Set(ID, p.Value);
   //      }
   //      return ID;
   //   }

   //   public TT GetCompSys<TT>() where TT : IDataStream<long>
   //   {
   //      return (TT)ComponentSystems[typeof(TT).Name];

   //   }
   //   public TT GetCompSys<TT>(string name) where TT : IDataStream<long>
   //   {
   //      return (TT)ComponentSystems[name];

   //   }
   //   public IDataStream<long> GetCompSys(string name)
   //   {
   //      return ComponentSystems[name];
   //   }
   //}
   //public static class LazyEXT
   //{
   //   public static T GetOrDefault <T>( this Lazy<DataStream<long,T>, IDSMetaData> lazy, long ID)
   //   {
   //      if (!lazy.IsValueCreated)
   //         return (T)lazy.Metadata.DefaultValue;
   //      else
   //      {
   //         return lazy.Value.GetOrDefault(ID);
   //      }
   //   }
   //   public static object GetObjOrDefault(this Lazy<IDataStream<long>, IDSMetaData> lazy, long ID)
   //   {
   //      if (!lazy.IsValueCreated)
   //         return lazy.Metadata.DefaultValue;
   //      else
   //      {
   //         return lazy.Value.GetObjOrDefault(ID);
   //      }
   //   }

   //   public static bool HasEntity(this Lazy<IDataStream<long>, IDSMetaData> lazy, long ID)
   //   {
   //      return !lazy.IsValueCreated ? false : lazy.Value.HasEntity(ID);
   //   }

   //   public static IObservable<IDataStream<long>> AsObservable(this Lazy<IDataStream<long>, IDSMetaData> lazy, long ID)
   //   {

   //      return lazy.IsValueCreated ? lazy.Value.AsObservable : Observable.Empty();
   //   }
   //}
}
