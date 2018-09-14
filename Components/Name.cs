using System;
using System.Collections.Generic;
using System.Text;
using DOD;

namespace Components
{
   [System.Composition.Export(typeof(DOD.IDataStream<long>))]
   [System.Composition.ExportMetadata("Name", nameof(NameComponentSystem))]
   public class NameComponentSystem : BaseCompSys<string>
   {
      [System.Composition.ImportingConstructor]
      public NameComponentSystem(DSManager manager) : base(manager)
      {
         Console.WriteLine("Name!");
      }

      public override string Name => "Name Comp";

      public override string DefaultVal => "<EMPTY>";

      public override void Initialize()
      {
         
      }
   }
}
