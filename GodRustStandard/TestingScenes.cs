using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodRustStandard
{
   public static class TestingScenes
   {
      public static Scene TestingScene01()
      {
         Scene scene = Scene.createWithDefaultRenderer(Microsoft.Xna.Framework.Color.CornflowerBlue);

         Entity Player = new Entity("Player");
         var c = Player.addComponent<BoxCollider>();
         
         return scene;
      }
   }
}
