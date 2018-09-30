using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System.Reactive.Linq;

namespace GodRustStandard
{
   class PlayerInputHandler : InputHandler
   {
     
      public PlayerInputHandler(Game1 game) : base()
      {
         gplistener = new GamePadListener();
         gplistener.ButtonDown += Gplistener_ButtonDown;
         gplistener.ButtonUp += Gplistener_ButtonUp;
         gplistener.ThumbStickMoved += Gplistener_ThumbStickMoved;
         //get reactive instead?
         //game.AsObservable.Where(x => x.LoopState == LoopState.UpdateBegin).Subscribe((x) =>
         //{
         //   gplistener.Update(x.GameTime);
         //});
      }

      public override void update()
      {
         gplistener.Update(Nez.Time.);
      }

      private void Gplistener_ThumbStickMoved(object sender, GamePadEventArgs e)
      {
         if (e.Button == Microsoft.Xna.Framework.Input.Buttons.LeftStick)
            LeftStick = e.ThumbStickState;
         else
            RightStick = e.ThumbStickState;
      }

      private void Gplistener_ButtonUp(object sender, GamePadEventArgs e)
      {
         ButtonStates[e.Button] = false;
      }

      private void Gplistener_ButtonDown(object sender, GamePadEventArgs e)
      {
         ButtonStates[e.Button] = true;
      }
      GamePadListener gplistener { get; set; }
   }
}
