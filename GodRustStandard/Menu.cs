using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace GodRustStandard
{
   public static class Menu
   {
      public static Scene GetMenu()
      {
         Scene s = Scene.createWithDefaultRenderer(Color.Coral);
         var e = s.addEntity(new Entity("Menu"));
         var canvas =e.addComponent<UICanvas>();
         
         
         // tables are very flexible and make good candidates to use at the root of your UI. They work much like HTML tables but with more flexibility.
         var table = canvas.stage.addElement(new Table());

         // tell the table to fill all the available space. In this case that would be the entire screen.
         table.setFillParent(true);


         // add a ProgressBar
         var bar = new ProgressBar(0, 1, 0.1f, false, ProgressBarStyle.create(Color.Black, Color.White));
         table.add(bar);

         // this tells the table to move on to the next row
         table.row();

         // add a Slider
         var slider = new Slider(0, 1, 0.1f, false, SliderStyle.create(Color.DarkGray, Color.LightYellow));
         
         table.add(slider).minSize(400,800);
         table.row();

         // if creating buttons with just colors (PrimitiveDrawables) it is important to explicitly set the minimum size since the colored textures created
         // are only 1x1 pixels
         //var button = new TextButton("Play Game",TextButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));
         var button = new Button(ButtonStyle.create(Color.Black, Color.DarkGray, Color.Green));
         
         button.add( new Label("press",new LabelStyle(Color.Black)));
         
         table.add(button).setMinWidth(200).setMinHeight(200);
         canvas.stage.setGamepadFocusElement(button);
         return s;
      }
   }
}
