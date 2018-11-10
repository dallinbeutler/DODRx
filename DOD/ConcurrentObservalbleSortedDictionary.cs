using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
   [Serializable]
   public class Node
   {
      Node Parent;
      string Name;
      Dictionary<string, Node> Children = null; //= new Dictionary<string, Node>();

      public Node()
      {
      }
      public bool CanHaveChildren
      {
         get
         {
            return Children != null;
         }
         set
         {
            if (value == CanHaveChildren) return;
            if (value)
               Children = new Dictionary<string, Node>();
            else
               Children = null;
         }
      }
      public int ChildCount
      {
         get
         {
            return Children?.Count??0;
         }
      }

      public Node this[string i]
      {
         get
         {
            return Children[i];
         }
         set
         {
            Children[i] = value;
         }
      }




      private void accesstest(Node layer)
      {
         var poo = this[""][""][""];
      }
   }


}
