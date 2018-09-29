using Nez;
using System;
using System.Collections.Generic;
using System.Text;

namespace GodRustStandard
{
   public class BaseScene 
   {
      public Scene scene;

      public Component PlayerController;

      public Entity player;

      public BaseScene()
      {
         scene = Scene.createWithDefaultRenderer();

         player = new Entity("");
      }

   }
}
