using System;
using System.Collections.Generic;
using System.Text;

namespace DOD.Curves
{
   interface ICurve
    {
   /// <summary>
   /// Easing equation function for an exponential (2^t) easing out: 
   /// decelerating from zero velocity.
   /// </summary>
   /// <param name="t">Current time in seconds.</param>
   /// <param name="b">Starting value.</param>
   /// <param name="c">Final value.</param>
   /// <param name="d">Duration of animation.</param>
   /// <returns>The correct value.</returns>
      float Execute(float t, float b, float c, float d);
      /// <summary>
      /// Easing equation function for an exponential (2^t) easing out: 
      /// decelerating from zero velocity.
      /// </summary>
      /// <param name="t">Current time in seconds.</param>
      /// <param name="b">Starting value.</param>
      /// <param name="c">Final value.</param>
      /// <param name="d">Duration of animation.</param>
      /// <returns>The correct value.</returns>
      double Execute(double t, double b, double c, double d);


   }

   public class CurveLinear : ICurve
   {
      public float Execute(float t, float b, float c, float d)
      {
         throw new NotImplementedException();
         DODFS.Easing.BackEaseIn backEaseIn = new DODFS.Easing.BackEaseIn(1f,2d,3,4);
         

      }

      public double Execute(double t, double b, double c, double d)
      {
         throw new NotImplementedException();
      }


   }
}
