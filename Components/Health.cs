using System;
using System.Linq;
using System.Reactive.Linq;
//using DOD;
namespace Components
{
   [System.Composition.Export(typeof(DOD.IDataStream<long>)),System.Composition.Shared]
   public class Health : DOD.DataStream<long, int>
   {
      DOD.DSManager manager;
      [System.Composition.ImportingConstructor]
      public Health([System.Composition.Import] DOD.DSManager manager):base()
      {
         this.manager = manager;
         Console.WriteLine("HEALTH!");
      }

      public void Initialize()
      {
         var namecs = manager.GetCompSystem<Components.NameComponentSystem>();
         
         this.AsObservableDetails.Where(x => x.NewVal <= 0).Subscribe(x =>
         {
            Console.WriteLine(x.Entity + "- "+ (namecs.HasEntity(x.Entity)? namecs[x.Entity]:"" )+" is Dead!");
         });
      }
   }
}
