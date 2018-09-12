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
   [Export(typeof(DOD.DSManager)),Shared]
   public class MrManager : DOD.DSManager
   {
      public MrManager() : base(null)
      {
         
      }


   }

   class Game
   {
      [System.ComponentModel.Composition.ImportMany]
      IEnumerable<DOD.IDataStream<long>> _DataStreams { get; set; }

      [System.ComponentModel.Composition.Import(typeof(DOD.DSManager))]
      DOD.DSManager manager;

      public Game()
      {
         Compose();

         manager.addCompSystems(_DataStreams);
         //MrManager manager = new MrManager(_DataStreams);

         //var hc = manager.ComponentSystems["Health"] as Components.Health;
         
         
         var nc = manager.GetCompSystem<Components.NameComponentSystem>("NameComponentSystem");
         var hc = manager.GetCompSystem<Components.Health>();
         hc.AsObservableDetails.CombineLatest(nc.AsObservableDetails, (x, y) => Tuple.Create(x, y)).Subscribe(x =>
                {
                   Console.WriteLine("#" + x.Item1.Entity + " -" + x.Item2.NewVal + "-" + " has" + x.Item1.NewVal + "HP");
                //Console.WriteLine("#" +x.Item1.Entity + " -"+nc.GetOrDefault(x.Entity) + "-"+ " has" + x.NewVal + "HP");
             });

         hc.Initialize();
         long dougy = manager.AddEntity(new DOD.DSManager.CompPair(hc, 100), new DOD.DSManager.CompPair(nc, "DOUG"));
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
      }
      private void Compose()
      {
         var configuration = GetConfiguration();
         using (var container = GetConfiguration())
         {
            _DataStreams = container.GetExports<DOD.IDataStream<long>>();
            manager = container.GetExport<DOD.DSManager>();
            var count = _DataStreams.Count();
         }

      }
      private CompositionHost GetConfiguration()
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
