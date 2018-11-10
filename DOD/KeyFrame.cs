using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;


namespace DOD
{
   public enum KeyFrameType
   {
      constant
      , continuous
         ,
   }
   public interface IKeyFrame
   {
      TimeSpan StartTime { get; set; }

   }
   public class Timeline
   {
      public List<IKeyFrame> KeyFrames { get; private set; }
      public void AddFromEnd(IKeyFrame frame)
      {
         int index = 0;
         
         for(int i = KeyFrames.Count; i > 0; i--)
         {
            if (KeyFrames[i].StartTime < frame.StartTime)
            {
               index = i;
               break;
            }
         }
         KeyFrames.Insert(index, frame);
      }
   }

}
