using System;
using System.Linq;
using System.Reactive.Linq;
using DOD;
namespace Components
{
   [System.Composition.Export(typeof(DOD.IDataStream<long>)),System.Composition.Shared]
   [System.Composition.ExportMetadata("Name", nameof(Health))]
   public class Health : BaseCompSys<int>
   {
      public override string Name => nameof(Health);

      public override int DefaultVal => 100;

      //DOD.DSManager manager;
      //[System.Composition.ImportingConstructor]
      //public Health([System.Composition.Import] DOD.DSManager manager):base()
      //{
      //   this.manager = manager;
      //   Console.WriteLine("HEALTH!");
      //}
      [System.Composition.ImportingConstructor]
      public Health(DOD.DSManagerLazy manager) :base(manager)
      {
         Console.WriteLine("HEALTH!");
      }
      public override void Initialize()
      {
         var namecs = manager.GetCompSys<Components.NameComponentSystem>();
         
         this.AsObservableDetails.Where(x => x.NewVal <= 0).Subscribe(x =>
         {
            //Console.WriteLine(x.Entity + "- "+ (namecs.HasEntity(x.Entity)? namecs[x.Entity]:"" )+" is Dead!");
            Console.WriteLine(x.Entity + "- "+ namecs.GetObjOrDefault(x.Entity)+" is Dead!");
         });
      }
      //public override void Dispose()
      //{
      //   base.Dispose();
      //}
   }
}
