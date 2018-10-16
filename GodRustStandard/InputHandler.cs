using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using DOD;
using Nez;

namespace GodRustStandard
{
   public class InputHandler : IUpdatable
   {
      public DataStream<Buttons, bool> ButtonStates = new DataStream<Buttons, bool>();
      public Vector2 LeftStick = new Vector2();
      public Vector2 RightStick = new Vector2();
      public InputHandler()
      {

         
         ButtonStates[Buttons.LeftThumbstickLeft] = false;
         ButtonStates[Buttons.LeftThumbstickRight] = false;
         ButtonStates[Buttons.LeftThumbstickUp] = false;
         ButtonStates[Buttons.LeftThumbstickDown] = false;
         ButtonStates[Buttons.A] = false;
         ButtonStates[Buttons.B] = false;
         ButtonStates[Buttons.X] = false;
         ButtonStates[Buttons.Y] = false;
         ButtonStates[Buttons.LeftTrigger] = false;
         ButtonStates[Buttons.RightTrigger] = false;
         ButtonStates[Buttons.Start] = false;

         //var p = new GamePadState()

      }

      public bool enabled => true;

      public int updateOrder => 0;

      public  virtual void update()
      {
         
      }
               

   }
}
