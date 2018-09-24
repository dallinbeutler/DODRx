using System;
using DynamicData;


namespace RxDOD
{
   public class Foo
   {
      public string Key;
   }
   class Program
   {
      //DynamicData.Cache.Internal.
      SourceCache<Foo, string> data { get; set; } = new SourceCache<Foo, string>(x=>x.Key);

      static void Main(string[] args)
      {
         Program p = new Program();
         var f = p.data.Lookup("foo");
         f.Value;
         Console.WriteLine("Hello World!");
      }
   }
}
