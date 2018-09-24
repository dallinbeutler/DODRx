using System;
using System.Collections.Generic;
using System.Text;
using DOD;

namespace Testing
{
   public class Game2
   {
      public Game2()
      {
         GameManager manager = new DOD.GameManager();

         var hp = new DataStream<long,int>("Current Health",null, 10);
         var maxhp = new CDS<int>("Max Health", 10);
         var name = new CDS<string>("Name", "<UNNAMED>");
         manager.AddComps(
            hp,maxhp,name
            );
         hp.AsObservableDetails.Subscribe(x =>
         {
            Console.WriteLine(x.Entity + " - " +name.GetOrDefault(x.Entity) + " - " + x.NewVal + " hp" ); 
         });

         var dave = manager.getUniqueID();
         hp[dave] = 100;
         maxhp[dave] = 100;
         name[dave] = "DAVE";

      }
   }

   public class System
   {
      public System(params string[] requirements)
      {

      }
   }
   public class Entity
   {
      public Entity(params System[] Requiremnets)
      {

      }
   }
}
