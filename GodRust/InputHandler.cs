using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Input.InputListeners;

using MonoGame.Extended.Input;
using Microsoft.Xna.Framework;

namespace GodRust
{
   public enum eInput
   {
      LLEFT,
      LRIGHT,
      LUP,
      LDOWN,
      RRIGHT,
      RLEFT,
      RUP,
      RDOWN,
      LCLICK,
      RCLICK,
      START,
      SELECT,
      A,
      B,
      X,
      Y,
      LB,
      LT,
      RT,
      RB,
      DUP,
      DDOWN,
      DLEFT,
      DRIGHT,
   }
   public class InputArgs
   {
      int Player;
      eInput Input;

   }
   public class Input
   {
      bool Pressed;
      Keys key;
      public Input(Keys key)
      {
         this.key = key;
      }
   }
   public class InputHandler
   {
      public delegate void InputEventHandler(InputHandler sender, int Player, eInput input, bool isPressed);

      public float LX;
      public float LY;
      public float RX;
      public float RY;

      public Dictionary<eInput, bool> GPMappings = new Dictionary<eInput, bool>
      {
          {eInput.A,     false}//new Input(Keys.Z) }
         ,{eInput.LUP,   false}//new Input(Keys.W) }
         ,{eInput.LRIGHT,false}//new Input(Keys.A) }
         ,{eInput.LDOWN, false}//new Input(Keys.S) }
         ,{eInput.LLEFT, false}//new Input(Keys.D) }
         ,{eInput.RUP,   false}//new Input(Keys.W) }
         ,{eInput.RRIGHT,false}//new Input(Keys.A) }
         ,{eInput.RDOWN, false}//new Input(Keys.S) }
         ,{eInput.RLEFT, false}//new Input(Keys.D) }
         ,{eInput.A,   false}//new Input(Keys.W) }
         ,{eInput.B,false}//new Input(Keys.A) }
         ,{eInput.X, false}//new Input(Keys.S) }
         ,{eInput.Y, false}//new Input(Keys.D) }
      };
      public event InputEventHandler InputRecieved;
      public InputHandler(Game game)
      {
         gplistener= new GamePadListener();
         //gplistener.ButtonDown += Gplistener_ButtonDown;
         //gplistener.ButtonUp += Gplistener_ButtonUp;
         ButtonDown = Observable.FromEventPattern<GamePadEventArgs>(gplistener, "ButtonDown").Select(x=>x.EventArgs);
         ButtonUp = Observable.FromEventPattern<GamePadEventArgs>(gplistener, "ButtonUp").Select(x=>x.EventArgs);
         //KeyboardListener kblistener = new KeyboardListener();
         //ButtonEvents = ButtonDown.Merge(ButtonUp);
         //var b2 = ButtonDown.Zip(Observable.Repeat(false),(x,y)=> Tuple.Create(x,y));
         ButtonEvents = ButtonDown.Select(x => Tuple.Create(x,true)).Merge(ButtonUp.Select(x=>Tuple.Create(x,false)));
         //InputRecieved +=
         
      }
      private GamePadListener gplistener { get; set; }
      public IObservable<GamePadEventArgs> ButtonDown { get; set; }
      public IObservable<GamePadEventArgs> ButtonUp { get; set; }
      public IObservable<Tuple<GamePadEventArgs,bool>> ButtonEvents { get; set; }

      //private void Gplistener_ButtonUp(object sender, GamePadEventArgs e)
      //{
      //   throw new NotImplementedException();
      //}

      //private void Gplistener_ButtonDown(object sender, GamePadEventArgs e)
      //{

      //   throw new NotImplementedException();
      //}

      //public void Update()
      //{
      //   var kstate = Keyboard.GetState();
      //   kstate.IsKeyDown(Keys.);
      //   for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
      //   {
      //      bool current;
      //      var gpstate = GamePad.GetState(i);

      //      current = GPMappings[eInput.A];
      //      if ((gpstate.Buttons.A == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.A] = !current;
      //         InputRecieved.Invoke(this, i, eInput.A, !current);
      //      }

      //      current = GPMappings[eInput.B];
      //      if ((gpstate.Buttons.B == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.B] = !current;
      //         InputRecieved.Invoke(this, i, eInput.B, !current);
      //      }

      //      current = GPMappings[eInput.X];
      //      if ((gpstate.Buttons.X == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.X] = !current;
      //         InputRecieved.Invoke(this, i, eInput.X, !current);
      //      }
      //      current = GPMappings[eInput.Y];
      //      if ((gpstate.Buttons.Y == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.Y] = !current;
      //         InputRecieved.Invoke(this, i, eInput.Y, !current);
      //      }
      //      current = GPMappings[eInput.LUP];
      //      if ((gpstate.ThumbSticks.Left.Y < -.5 ) != current)
      //      {
      //         GPMappings[eInput.LUP] = !current;
      //         InputRecieved.Invoke(this, i, eInput.LUP, !current);
      //      }
      //      current = GPMappings[eInput.LLEFT];
      //      if ((gpstate.ThumbSticks.Left.X < -.5) != current)
      //      {
      //         GPMappings[eInput.LLEFT] = !current;
      //         InputRecieved.Invoke(this, i, eInput.LLEFT, !current);
      //      }
      //      current = GPMappings[eInput.LRIGHT];
      //      if ((gpstate.ThumbSticks.Left.X > -.5) != current)
      //      {
      //         GPMappings[eInput.LRIGHT] = !current;
      //         InputRecieved.Invoke(this, i, eInput.LRIGHT, !current);
      //      }
      //      current = GPMappings[eInput.LDOWN];
      //      if ((gpstate.ThumbSticks.Left.Y > .5) != current)
      //      {
      //         GPMappings[eInput.LDOWN] = !current;
      //         InputRecieved.Invoke(this, i, eInput.LDOWN, !current);
      //      }
      //      current = GPMappings[eInput.s];
      //      if ((gpstate.Buttons.s == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.s] = !current;
      //         InputRecieved.Invoke(this, i, eInput.s, !current);
      //      }
      //      current = GPMappings[eInput.s];
      //      if ((gpstate.Buttons.s == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.s] = !current;
      //         InputRecieved.Invoke(this, i, eInput.s, !current);
      //      }
      //      current = GPMappings[eInput.s];
      //      if ((gpstate.Buttons.s == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.s] = !current;
      //         InputRecieved.Invoke(this, i, eInput.s, !current);
      //      }
      //      current = GPMappings[eInput.s];
      //      if ((gpstate.Buttons.s == ButtonState.Pressed) != current)
      //      {
      //         GPMappings[eInput.s] = !current;
      //         InputRecieved.Invoke(this, i, eInput.s, !current);
      //      }
      //   }
      //}
   }
}
