using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Composition;

internal class MefHosting
{
   /// <summary>
   /// The MEF discovery module to use (which finds both MEFv1 and MEFv2 parts).
   /// </summary>
   private readonly PartDiscovery discoverer = PartDiscovery.Combine(
       new AttributedPartDiscovery(Resolver.DefaultInstance, isNonPublicSupported: true),
       new AttributedPartDiscoveryV1(Resolver.DefaultInstance));

   /// <summary>
   /// Gets the names of assemblies that belong to the application .exe folder.
   /// </summary>
   /// <returns>A list of assembly names.</returns>
   private static IEnumerable<string> GetAssemblyNames()
   {
      string directoryToSearch = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
      foreach (string file in Directory.EnumerateFiles(directoryToSearch, "*.dll"))
      {
         string assemblyFullName = null;
         try
         {
            var assemblyName = AssemblyName.GetAssemblyName(file);
            if (assemblyName != null)
            {
               assemblyFullName = assemblyName.FullName;
            }
         }
         catch (Exception)
         {
         }

         if (assemblyFullName != null)
         {
            yield return assemblyFullName;
         }
      }
   }

   /// <summary>
   /// Creates a catalog with all the assemblies from the application .exe's directory.
   /// </summary>
   /// <returns>A task whose result is the <see cref="ComposableCatalog"/>.</returns>
   public async Task<ComposableCatalog> CreateProductCatalogAsync()
   {
      var assemblyNames = GetAssemblyNames();
      var assemblies = assemblyNames.Select(Assembly.Load);
      var discoveredParts = await this.discoverer.CreatePartsAsync(assemblies);
      var catalog = ComposableCatalog.Create(Resolver.DefaultInstance)
          .AddParts(discoveredParts);
      return catalog;
   }
}