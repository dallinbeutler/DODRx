//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using MonoGame.Extended.Input;
//using MonoGame.Extended.Input.InputListeners;
//using System;
//using System.Collections.Generic;
//using System.Collections.Concurrent;
//using System.Collections.Specialized;
//using System.Reactive.Linq;
//using System.Collections.ObjectModel;
//using DOD;

//namespace GodRustShared
//{
//   public class InputHandler
//   {
//      public DataStream<Buttons, bool> ButtonStates = new DataStream<Buttons, bool>() ;
//      public InputHandler()
//      {
//         gplistener = new GamePadListener();
//         gplistener.ButtonDown += Gplistener_ButtonDown;
//         gplistener.ButtonUp += Gplistener_ButtonUp;

//         ButtonStates[Buttons.LeftThumbstickLeft] = false;
//         ButtonStates[Buttons.LeftThumbstickRight] = false;
//         ButtonStates[Buttons.LeftThumbstickUp] = false;
//         ButtonStates[Buttons.LeftThumbstickDown] = false;

//      }

//      private void Gplistener_ButtonUp(object sender, GamePadEventArgs e)
//      {
//         ButtonStates[e.Button] = false;
//      }

//      private void Gplistener_ButtonDown(object sender, GamePadEventArgs e)
//      {
//         ButtonStates[e.Button] = true;
//      }
//      GamePadListener gplistener { get; set; }
//      public void Update(GameTime gameTime)
//      {
//         gplistener.Update(gameTime);
//      }
      
//   }
//}
