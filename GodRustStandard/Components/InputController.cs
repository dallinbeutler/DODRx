using System;
using System.Collections.Generic;
using System.Text;
using Nez;

namespace GodRustStandard.Components
{
   public class InputController : Component, IUpdatable
   {
      public void update()
      {
         Nez.Input.gamePads[0]
      }

   }
}
