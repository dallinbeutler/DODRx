using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Runtime;

namespace Components
{


   public abstract class BaseCompSys<T> : DOD.DataStream<long, T> //, IDisposable
   {
      protected DOD.DSManagerLazy manager { get; }

      public abstract void Initialize();
      
      public abstract new string Name { get; }

      public abstract new T DefaultVal { get; }

      public BaseCompSys(DOD.DSManagerLazy manager) : base()
      {
         this.manager = manager;
         this.DataStreamChanged += manager.ComponentChange;
         //using (var container = GetConfiguration())
         //{
         //   manager = container.GetExport<DOD.DSManager>();
         //}
      }
      public  void Dispose()
      {
         this.DataStreamChanged -= manager.ComponentChange;
         //base.Dispose();
      }
      //public static System.Composition.Hosting.CompositionHost GetConfiguration()
      //{
      //   var directory = AppDomain.CurrentDomain.BaseDirectory;
      //   var assemblies = Directory.GetFiles(directory, "*.dll")
      //                   .Select(System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath);
      //   var count = assemblies.Count();
      //   var configuration = new System.Composition.Hosting.ContainerConfiguration().WithAssemblies(assemblies);
      //   var container = configuration.CreateContainer();
      //   return container;
      //}

      
   }
}
