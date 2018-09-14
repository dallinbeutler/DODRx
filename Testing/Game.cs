using Components;
using DOD;
using Microsoft.VisualStudio.Composition;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
   //This class is purely for attaching this export so MEF can find it and add it to the components
   [Export(typeof(DOD.DSManagerLazy)),Shared]
   public class MrManager : DOD.DSManagerLazy
   {
      public MrManager() : base(null)
      {
      }
   }

   class Game
   {
      //[System.ComponentModel.Composition.ImportMany(typeof(IDataStream<long>),AllowRecomposition =true)]
      IEnumerable<Lazy<IDataStream<long>, IDSMetaData>> _DataStreams { get; set; }
      //IEnumerable<IDataStream<long>> _DataStreams { get; set; }

      //[System.ComponentModel.Composition.Import(typeof(DOD.DSManager))]
      DSManagerLazy manager;

      public Game()
      {
         using (var container = GetConfiguration())
         {
            _DataStreams = container.GetExports<Lazy<DOD.IDataStream<long>, IDSMetaData>>();
            //_DataStreams = container.GetExports<DOD.IDataStream<long>>();
            manager = container.GetExport<DOD.DSManagerLazy>();
            manager.AddCompSystems(_DataStreams/*container.GetExports<DOD.IDataStream<long>>()*/);
            //var count = _DataStreams.Count();
         }

         var nc = manager.GetCompSys<Components.NameComponentSystem>("NameComponentSystem");
         var hc = manager.GetCompSys<Components.Health>();
         //hc.Value.AsObservable.CombineLatest(nc.Value.AsObservable, (hp, name) => Tuple.Create(hp, name)).Subscribe(x =>
         hc.Value.AsObservable.CombineLatest(nc.Value.AsObservable, (x, y) => new { hp = x, name = y }).Subscribe(result =>
                {
                   //Console.WriteLine("#" + x.Item1.Entity + " -" + x.Item2.NewVal + "-" + " has" + x.Item1.NewVal + "HP");
                   //Console.WriteLine("#" + x.Item1.Entity + " -" + nc.Value.GetObjOrDefault(x.Item1.Entity) + "-" + " has" + x.NewVal + "HP");
                   //Console.WriteLine("#" + x.Item1.Entity + " -" + nc.Value.Get(x.Item1.Entity) + "-" + " has" +  + "HP");
                   Console.WriteLine("#" + result.hp.Entity + " - " + nc.Value.Get(result.hp.Entity) + "-" + " has" + hc.Value.Get(result.hp.Entity) + "HP");
                });

         hc.Initialize();
         long dougy = manager.AddEntity(new DOD.DSManagerLazy.CompPair(hc, 100), new DOD.DSManagerLazy.CompPair(nc, "DOUG"));
         long dave = manager.AddEntity(new DOD.DSManagerLazy.CompPair(hc, 100));
         hc[dougy] -= 10;
         System.Threading.Thread.Sleep(500);
         hc[dougy] -= 20;
         System.Threading.Thread.Sleep(500);
         hc[dougy] -= 20;
         System.Threading.Thread.Sleep(500);
         hc[dougy] -= 20;
         System.Threading.Thread.Sleep(500);
         hc[dougy] -= 20;
         System.Threading.Thread.Sleep(500);
         hc[dougy] -= 20;
         nc[dave] = "DAVVVE";
      }

      public static CompositionHost GetConfiguration()
      {
         var directory = AppDomain.CurrentDomain.BaseDirectory;
         var assemblies = Directory.GetFiles(directory, "*.dll")
                         .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath);
         var count = assemblies.Count();
         var configuration = new ContainerConfiguration().WithAssemblies(assemblies);
         var container = configuration.CreateContainer();
         return container;
      }

   }
}
