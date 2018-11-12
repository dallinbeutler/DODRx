using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace REPL
{
    class Program
    {
      private static ScriptState<object> scriptState = null;
      static void Main(string[] args)
        {
         Execute(
                     //This could be code submitted from the editor
//using System.Linq;
                     @"
   public class ScriptedClass
   {
      public List<string> HelloWorld { get; set; } = new List<string>() { ""wow"", ""Amazing"" };
      public ScriptedClass()
      {
         HelloWorld.Add(""foo"");
      }
   }");
         //And this from the REPL
         Console.WriteLine(Execute("new ScriptedClass().HelloWorld"));
         Console.ReadKey();
      }

      public static object Execute(string code)
      {
         scriptState = scriptState == null ? CSharpScript.RunAsync(code, ScriptOptions.Default.WithReferences("System.Linq","System.Collections.Generic")).Result : scriptState.ContinueWithAsync(code).Result;
         if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
            return scriptState.ReturnValue;
         return null;
         
      }
   }

   public class ScriptedClass
   {
      public List<string> HelloWorld { get; set; } = new List<string>() { "wow", "Amazing" };
      public ScriptedClass()
      {
         HelloWorld.Add("foo");
      }
      
   }
}
