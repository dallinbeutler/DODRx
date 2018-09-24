using ConcurrentCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DOD
{
   public interface IDataStream<Key> : INotifyPropertyChanged //: Enumerable
   {
      bool IsInitialized { get; set; }
      string Name { get; }
      int Count { get; }
      bool HasEntity(Key ID);
      void Clear();
      bool RemoveAt(Key ID);
      void Set(Key ID, object o);
      object Get(Key ID);
      object GetOrAdd(Key ID);
      object GetOrDefault(Key ID);
      event NotifyDataStreamChangedEventHandler<Key> DSChanged;
      IObservable<EntityChangedEventArgs<Key>> AsObservable { get; }
   }

   public class HashStream: INotifyPropertyChanged
   {
      ConcurrentCollections.ConcurrentHashSet<string> Data = new ConcurrentHashSet<string>();
      public HashStream()
      {
         
      }

      public string Name { get; }

      public int Count { get; }

      public IObservable<EntityChangedEventArgs<string>> AsObservable { get; set; }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Clear()
      {
         Data.Clear();
      }

      public bool Get(string ID)
      {
         return (Data.Contains(ID));

      }

      public void Set(string ID, bool IsAdding)
      {
         if (IsAdding)
         {
            if (Data.Add(ID)) PropertyChanged.Invoke(this, new HashChangedEventArgs(ID, IsAdding));
         }
         else
         {
            if (Data.TryRemove(ID)) PropertyChanged.Invoke(this, new HashChangedEventArgs(ID, IsAdding));
         }
      }
   }
   public class HashChangedEventArgs : PropertyChangedEventArgs
   {
      public HashChangedEventArgs(string propertyName, bool BeingSet) : base(propertyName)
      {
      }
   }
}

