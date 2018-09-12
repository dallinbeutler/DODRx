using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reactive;
using System.Reactive.Linq;
//using System.ComponentModel.Composition;

namespace DOD
{
   public class DSManager
   {
      private long CurID;
      DataStream<string, IDataStream<long>> ComponentSystems { get; }//   = new DataStream<string, IDataStream<long>>("CompSystems");
      DataStream<long, List<IDataStream<long>>> Entities = new DataStream<long, List<IDataStream<long>>>("Entities");

      public DSManager(IEnumerable<KeyValuePair<string,IDataStream<long>>> systems)
      {
         ComponentSystems = new DataStream<string, IDataStream<long>>("CompSystems",systems);
         ComponentSystems.DataStreamChanged += ComponentSystems_DataStreamChanged;
      }
      long getUniqueID()
      {
         return Interlocked.Increment(ref CurID);
      }

      public TT GetCompSystem<TT>() where TT: IDataStream<long>
      {
         return (TT)ComponentSystems[typeof(TT).Name];
      }
      public TT GetCompSystem<TT>(string name) where TT : IDataStream<long>
      {
         return (TT)ComponentSystems[name];
      }

      private void ComponentSystems_DataStreamChanged(IDataStream<string> sender, EntityChangedArgs<string> args)// DSChangedArgs<string, IDataStream<long>> args)
      {
         var myargs = args as DSChangedArgs<string, IDataStream<long>>;
         if (myargs.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
         {
            myargs.NewVal.DataStreamChanged += ComponentChange;
         }
         else if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
         {
            myargs.NewVal.DataStreamChanged -= ComponentChange;
         }
      }

      private void ComponentChange(IDataStream<long> sender, EntityChangedArgs<long> args)
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
      public void KillEntity(long ID)
      {
         Entities[ID].ForEach(x => x.RemoveAt(ID));
      }

      public void addCompSystems(IEnumerable<DOD.IDataStream<long>> dataStreams)
      {
         ComponentSystems.AddRange(MakeDict(dataStreams));
      }

      /// <summary>
      /// adding or removing comps from this list will not change the entity
      /// </summary>
      /// <param name="ID"></param>
      /// <returns></returns>
      public IEnumerable<IDataStream<long>> GetComponents(long ID)
      {
         //ComponentSystems.Where(x => x.Value.HasEntity(ID));

         return Entities[ID];
      }
      public static IEnumerable<KeyValuePair<string, IDataStream<long>>> MakeDict(IEnumerable<IDataStream<long>> systems)
      {
         return systems.ToDictionary(x => x.Name, x => x);
      }
   }
}
