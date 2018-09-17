using Components;
using DOD;
using Microsoft.VisualStudio.Composition;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
   //This class is purely for attaching this export so MEF can find it and add it to the components
   [Export(typeof(DSManager)),Shared]
   public class MrManager : DSManager
   {
      public MrManager() : base(null)
      {
      }
   }

   class Game
   {
      //[System.ComponentModel.Composition.ImportMany(typeof(IDataStream<long>),AllowRecomposition =true)]
      //IEnumerable<Lazy<IDataStream<long>, IDSMetaData>> _DataStreams { get; set; }
      IEnumerable<IDataStream<long>> _DataStreams { get; set; }

      //[System.ComponentModel.Composition.Import(typeof(DOD.DSManager))]
      DSManager manager;

      public Game()
      {
         using (var container = GetConfiguration())
         {
            _DataStreams = container.GetExports<IDataStream<long>>();
            //_DataStreams = container.GetExports<DOD.IDataStream<long>>();
            manager = container.GetExport<DOD.DSManager>();
            manager.AddCompSystems(_DataStreams/*container.GetExports<DOD.IDataStream<long>>()*/);
            //var count = _DataStreams.Count();
         }

         var nc = manager.GetCompSys<Components.NameComponentSystem>("NameComponentSystem");
         var hc = manager.GetCompSys<Components.HealthCompSys>();
         hc.AsObservableDetails.CombineLatest(nc.AsObservableDetails, (hp, name) => Tuple.Create(hp, name)).Subscribe(x =>
         //hc.Value.AsObservable.CombineLatest(nc.Value.AsObservable, (hp, name) => Tuple.Create(hp, name)).Subscribe(x =>
         //hc.Value.AsObservable.CombineLatest(nc.Value.AsObservable, (x, y) => new { hp = x, name = y }).Subscribe(result =>
                {
                   Console.WriteLine("#" + x.Item1.Entity + " -" + x.Item2.NewVal + "-" + " has " + x.Item1.NewVal + "HP");
                   //Console.WriteLine("#" + x.Item1.Entity + " -" + nc.Value.GetObjOrDefault(x.Item1.Entity) + "-" + " has" + x.NewVal + "HP");
                   //Console.WriteLine("#" + x.Item1.Entity + " -" + nc.Value.Get(x.Item1.Entity) + "-" + " has" +  + "HP");
                   //Console.WriteLine("#" + result.hp.Entity + " - " + nc.Value.Get(result.hp.Entity) + "-" + " has" + hc.Value.Get(result.hp.Entity) + "HP");
                });

         hc.Initialize();
         long dougy = manager.AddEntity(new CompPair(hc, 100), new CompPair(nc, "DOUG"));
         long dave = manager.AddEntity(new CompPair(hc, 100));
         hc.Bleed(dougy,15,4);
         nc[dave] = "DAVVVE";
         System.Threading.Thread.Sleep(5000);
         var TimeManager = manager.GetCompSys<SysTime>();
         long Rick = manager.AddEntity(new CompPair(hc, 100),new CompPair(nc,"RICK"));
         TimeManager.Room.Dilation = 1.0;
         var hurtRick = TimeManager.DilatedTimer(-2, new TimeSpan(0, 0, 3)).Subscribe(x =>
         {
            hc[Rick] -= 12;
         });
         Thread.Sleep(100);
         TimeManager.Room.Dilation = .000001;
         //var hurtRick = new TimedEvent(Rick, new TimeSpan(0, 0, 1));
         //hurtRick.OnCompleted.Subscribe(x =>
         //{
         //   nc[Rick] = "Formerly Known as Rick";
         //});
         //TimeManager.timedEvents.Add(hurtRick);
         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();
         while (true)
         {
            Thread.Sleep(1000);
            Console.WriteLine(".");
            //TimeSpan delta = stopwatch.Elapsed ;
            //if (delta > new TimeSpan(0, 0, 0, 0, 1 / 60000))
            //{
            //   TimeManager.Update(delta);
            //   stopwatch.Restart();

            //}
         }
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
