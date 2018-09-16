using System;
using System.Linq;
using System.Reactive.Linq;
using DOD;
namespace Components
{
   [System.Composition.Export(typeof(DOD.IDataStream<long>)),System.Composition.Shared]
   [System.Composition.ExportMetadata("Name", nameof(HealthCompSys))]
   public class HealthCompSys : BaseCompSys<int>
   {
      public override string Name => nameof(HealthCompSys);

      public override int DefaultVal => 100;

      //DOD.DSManager manager;
      //[System.Composition.ImportingConstructor]
      //public Health([System.Composition.Import] DOD.DSManager manager):base()
      //{
      //   this.manager = manager;
      //   Console.WriteLine("HEALTH!");
      //}
      [System.Composition.ImportingConstructor]
      public HealthCompSys(DOD.DSManager manager) :base(manager)
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

      public void Bleed(long entity, int amount, int duration)
      {
         if (duration <= 0) return;
         Observable.Timer(new TimeSpan(0, 0, 1)).Subscribe(x =>
         {
            this[entity] -= amount;
            Bleed(entity, amount, duration - 1);
         });
      }

      //public override void Dispose()
      //{
      //   base.Dispose();
      //}
   }
}
