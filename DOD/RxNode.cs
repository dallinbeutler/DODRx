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
   class RxNode : INotifyPropertyChanged, IDisposable
   {
      private RxNode parent;

      RxNode Parent { get => parent;
         set
         {
            if (parent == value) return;
            if (parent != null)
            {
               parent.PropertyChanged -= Parent_PropertyChanged;
            }
            parent = value;
            parent.PropertyChanged += Parent_PropertyChanged;
            PropertyChanged.Invoke(this,new PropertyChangedEventArgs("Parent"));
         }
      }

      public IEnumerable<RxNode> Siblings(IEnumerable<RxNode> tree)
      {
         return tree.Where(x => x.Parent == Parent);
      }
      public IEnumerable<RxNode> Children(IEnumerable<RxNode> tree)
      {
         return tree.Where(x => x.Parent == this);
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

      protected void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
      }
      //public event GetChildren GetChildrenEvent;
      //public delegate RxNode GetChildren();
      public event PropertyChangedEventHandler PropertyChanged;

      public event EventHandler Disposing;
      public void Dispose()
      {
         Disposing.Invoke(this, new EventArgs());
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
