#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using MonoGame.Extended;
//using MonoGame.Extended.Graphics;
//using MonoGame.Extended.ViewportAdapters;
using System.Reactive;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Nez;
#endregion

namespace GodRustShared
{
   /// <summary>
   /// This is the main type for your game.
   /// </summary>
   public class Game1 : Core
   {
      //      GraphicsDeviceManager graphics;
      //      SpriteBatch spriteBatch;
      //      Camera2D Camera;
      //      InputHandler InputHandler;
      SpriteFont Font;
      public Game1(): base()
      { 
         //graphics = new GraphicsDeviceManager(this);
         //Content.RootDirectory = "../../Content";

         //graphics.IsFullScreen = true;
      }



      //      /// <summary>
      //      /// Allows the game to perform any initialization it needs to before starting to run.
      //      /// This is where it can query for any required services and load any non-graphic
      //      /// related content.  Calling base.Initialize will enumerate through any components
      //      /// and initialize them as well.
      //      /// </summary>
      //      /// 
      //      float vx = 0f;
      //      float vy = 0f;
      protected override void Initialize()
      {
         base.Initialize();
         Core.debugRenderEnabled = true;
         //         // TODO: Add your initialization logic here
         var s = Scene.createWithDefaultRenderer(Color.CornflowerBlue);
         var first = s.createEntity("first entity",new Vector2(100,100));
         
         var c = first.addComponent(new CircleCollider(10));
         
         scene = s;
        

         //         var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);

         //         Camera = new Camera2D(viewportAdapter);
         //         InputHandler = new InputHandler();
         //         var obs = InputHandler.ButtonStates.AsObservableDetails;
         //         obs.Subscribe(x =>
         //         {

         //         switch (x.Entity)
         //            {
         //               case Buttons.LeftThumbstickLeft:
         //                  vx -= 1;
         //                  break;
         //               case Buttons.LeftThumbstickRight:
         //                  vx += 1;
         //                  break;
         //               case Buttons.LeftThumbstickUp:
         //                  vy -= 1;
         //                  break;
         //               case Buttons.LeftThumbstickDown:
         //                  vy += 1;
         //                  break;
         //            }
         //            var move = new Vector2(vx, vy);
         //            //Camera.Move(move);
         //            Window.Title = move.ToString();
         //            //Camera.Move(new Vector2(vx, vy));
         //         });

      }


      //      /// <summary>
      //      /// LoadContent will be called once per game and is the place to load
      //      /// all of your content.
      //      /// </summary>
      protected override void LoadContent()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         //spriteBatch = new SpriteBatch(GraphicsDevice);
         Font = Content.Load<SpriteFont>("FontArial");
         //TODO: use this.Content to load your game content here 

      }

      //      /// <summary>
      //      /// Allows the game to run logic such as updating the world,
      //      /// checking for collisions, gathering input, and playing audio.
      //      /// </summary>
      //      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      //      protected override void Update(GameTime gameTime)
      //      {
      //         // For Mobile devices, this logic will close the Game when the Back button is pressed
      //         // Exit() is obsolete on iOS
      //#if !__IOS__ && !__TVOS__
      //         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
      //             Keyboard.GetState().IsKeyDown(Keys.Escape))
      //         {
      //            Exit();
      //         }
      //#endif
      //         InputHandler.Update(gameTime);

      //         // TODO: Add your update logic here			
      //         base.Update(gameTime);
      //      }

      //      /// <summary>
      //      /// This is called when the game should draw itself.
      //      /// </summary>
      //      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      //      protected override void Draw(GameTime gameTime)
      //      {
      //         GraphicsDevice.Clear(Color.CornflowerBlue);

      //         //TODO: Add your drawing code here

      //         base.Draw(gameTime);


      //         GraphicsDevice.Clear(Color.CornflowerBlue);
      //         // TODO: Add your drawing code here

      //         base.Draw(gameTime);
      //         //GraphicsDevice.DrawPrimitives(PrimitiveType.)

      //         spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());
      //         spriteBatch.DrawRectangle(new Rectangle((int)vx, (int)vy, 100, 100), Color.Black);
      //         spriteBatch.DrawString(Font, "FUCK", new Vector2(), Color.Black);
      //         spriteBatch.End();
      //      }


      protected override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);
         //this.
      }
   }
}
