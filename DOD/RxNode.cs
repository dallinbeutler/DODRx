using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Linq;

namespace DOD
{
   public class RxTree
   {
      public RxNode root;

   }

   public delegate void NodeEventHandler(RxNode sender, NodeEventArgs e);
   public class NodeEventArgs : EventArgs
   {
      public bool Handled { get; set; }



   }
   public class RxNode : INotifyPropertyChanged, IDisposable
   {
      private RxNode parent;
      private RxNode FirstChild;
      private RxNode Right;
      private RxNode Left;

      RxNode Parent
      {
         get => parent;
         set
         {
            if (parent == value) return;
            if (parent != null)
            {
               parent.PropertyChanged -= Parent_PropertyChanged;
               parent.Disposing -= Parent_Disposing;
            }
            parent = value;
            parent.PropertyChanged += Parent_PropertyChanged;
            parent.Disposing += Parent_Disposing;
            parent.ChildAdded.Invoke(this, new NodeEventArgs());
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Parent"));
         }
      }

      private void Detach()
      {
         parent.PropertyChanged -= Parent_PropertyChanged;
         parent.Disposing -= Parent_Disposing;

         if (parent.FirstChild == this)
         {
            if (this.Right != null)
            {
               parent.FirstChild = this.Right;
            }
            else
            {
               parent.FirstChild = null;
            }
         }
         else
         {
            this.Left.Right = this.Right;
            if (this.Right != null)
            {
               this.Right.Left = this.Left;
            }
         }
         
      }
      public void SetParent(RxNode node, int order)
      {
         if (parent == node) return;

         if (parent != null)
         {
            Detach();
         }
         parent = value;
         parent.PropertyChanged += Parent_PropertyChanged;
         parent.Disposing += Parent_Disposing;
         parent.ChildAdded.Invoke(this, new NodeEventArgs());
         PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Parent"));
      }


      protected virtual void Parent_Disposing(object sender, EventArgs e)
      {
         this.Dispose();
      }

      public IEnumerable<RxNode> Siblings(IEnumerable<RxNode> tree)
      {
         return tree.Where(x => x.Parent == Parent);
      }
      public IEnumerable<RxNode> Children(IEnumerable<RxNode> tree)
      {
         return tree.Where(x => x.Parent == this);
      }

      public RxNode FindRoot()
      {
         var p = parent;
         while (p != null) p = p.parent;
         return p;
      }

      public int TreeDepth()
      {
         int depth = 0;
         var p = parent;
         while (p != null)
         {
            depth++;
            p = p.Parent;
         }
         return depth;
      }

      protected virtual void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
      }
      //public event GetChildren GetChildrenEvent;
      //public delegate RxNode GetChildren();
      public event PropertyChangedEventHandler PropertyChanged;

      public event NodeEventHandler ChildAdded;
      public event NodeEventHandler Disposing;
      public void Dispose()
      {
         Disposing.Invoke(this, new NodeEventArgs());
      }

      static IEnumerable<T> CallBackToEnumerable<T>(Action<Action<T>> functionReceivingCallback)
      {
         return Observable.Create<T>(o =>
         {
            // Schedule this onto another thread, otherwise it will block:
            System.Reactive.Concurrency.Scheduler.Schedule(NewThreadScheduler.Default, () =>
            {
               functionReceivingCallback(o.OnNext);
               o.OnCompleted();
            });
            return () => { };
         }).ToEnumerable();
      }
   }

   interface IBeginUpdate
   {
      void BeginUpdate(float delta);
   }

   interface IUpdate
   {
      void Update(float delta);
   }

   interface IRender
   {
      int ZOrder { get; set; }
      void Render(float delta);
   }

   interface IDil : IUpdate
   {
      float Dilation { get; set; }
      bool AffectedByParent { get; set; }
   }


}
