using System;
using System.Linq;
using System.Reactive.Linq;
using DOD;
namespace Components
{
   public class HealthComp
   {
      public int currentHealth;
      public int MaxHealth;
   }
   [System.Composition.Export(typeof(DOD.IDataStream<long>)),System.Composition.Shared]
   [System.Composition.ExportMetadata("Name", nameof(HealthCompSys))]
   public class HealthCompSys : BaseCompSys<HealthComp>
   {
      public override string Name => nameof(HealthCompSys);

      public override HealthComp DefaultVal => new HealthComp() { currentHealth = 2,MaxHealth =10};

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
         
         this.AsObservableDetails.Where(x => x.NewVal.currentHealth <= 0).Subscribe(x =>
         {
            //Console.WriteLine(x.Entity + "- "+ (namecs.HasEntity(x.Entity)? namecs[x.Entity]:"" )+" is Dead!");
            Console.WriteLine(x.Entity + "- "+ namecs.GetOrDefault(x.Entity)+" is Dead!");
         });
      }

      public void Bleed(long entity, int amount, int duration)
      {
         if (duration <= 0) return;
         Observable.Timer(new TimeSpan(0, 0, 1)).Subscribe(x =>
         {
            this[entity].currentHealth -= amount;
            Bleed(entity, amount, duration - 1);
         });
      }

      //public override void Dispose()
      //{
      //   base.Dispose();
      //}
   }
}
