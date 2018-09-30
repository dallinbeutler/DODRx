#region Using Statements
using GodRustStandard;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
//using MonoGame.Extended;
//using MonoGame.Extended.Graphics;
//using MonoGame.Extended.ViewportAdapters;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace GodRustStandard
{

   public enum LoopState
   {
      Initialize,
      UpdateBegin,
      Update,
      UpdateEnd,
      Load,
      Unload,
      Draw
   }
   /// <summary>
   /// This is the main type for your game.
   /// </summary>
   public delegate void LoopArgsEventHandler(object sender, LoopArgs loopArgs);
   public struct LoopArgs
   {
      public LoopState LoopState { get; set; }
      public GameTime GameTime { get; set; }
      public LoopArgs(LoopState ls, GameTime gt)
      {
         this.LoopState = ls;
         GameTime = gt;
      }
   }

   public class Game1 : Core
   {
      //      GraphicsDeviceManager graphics;
      //      SpriteBatch spriteBatch;
      //      Camera2D Camera;
      //      InputHandler InputHandler;


      SpriteFont Font;

      public event LoopArgsEventHandler StateChange = (x, y) => { };
      public IObservable<LoopArgs> AsObservable => Observable.FromEventPattern<LoopArgs>(this, "StateChange").Select(x => x.EventArgs);

      public Game1() : base()
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
         var first = s.createEntity("first entity", new Vector2(100, 100));

         var c = first.addComponent(new CircleCollider(10));
         s.camera.entity = first;

         Observable.Interval(new TimeSpan(0, 0, 0, 0, 200)).Subscribe(x =>
         {
            first.position += new Vector2(8, 8);
         });

         //scene = s;
         scene = TestingScenes.TestingScene01();

         StateChange.Invoke(this, new LoopArgs(LoopState.Initialize, null));

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
         StateChange.Invoke(this, new LoopArgs(LoopState.Load, null));
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update(GameTime gameTime)
      {
         base.Update(gameTime);
         // For Mobile devices, this logic will close the Game when the Back button is pressed
         // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
             Keyboard.GetState().IsKeyDown(Keys.Escape))
         {
            Exit();
         }
#endif
         // TODO: Add your update logic here			
         StateChange.Invoke(this, new LoopArgs(LoopState.UpdateBegin, gameTime));
         StateChange.Invoke(this, new LoopArgs(LoopState.Update, gameTime));
         StateChange.Invoke(this, new LoopArgs(LoopState.UpdateEnd, gameTime));
      }

      protected override void UnloadContent()
      {
         base.UnloadContent();
         StateChange.Invoke(this, new LoopArgs(LoopState.Unload, null));
      }


      protected override void Draw(GameTime gameTime)
      {
         base.Draw(gameTime);
         StateChange.Invoke(this, new LoopArgs(LoopState.Draw, gameTime));
      }


   }
}
