using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using R5T.T0132;


namespace R5T.L0061
{
    [FunctionalityMarker]
    public partial interface IReflectionOperator : IFunctionalityMarker
    {
        public MetadataLoadContext Get_MetadataLoadContext(MetadataAssemblyResolver metadataAssemblyResolver)
        {
            var output = new MetadataLoadContext(metadataAssemblyResolver);
            return output;
        }

        public MetadataLoadContext Get_MetadataLoadContext(IEnumerable<string> assemblyFilePaths)
        {
            var assemblyResolver = this.Get_PathAssemblyResolver(assemblyFilePaths);

            var output = this.Get_MetadataLoadContext(assemblyResolver);
            return output;
        }

        public PathAssemblyResolver Get_PathAssemblyResolver(IEnumerable<string> assemblyFilePaths)
        {
            var output = new PathAssemblyResolver(assemblyFilePaths);
            return output;
        }

        public void In_AssemblyContext(
            string assemblyFilePath,
            Action<Assembly> action)
        {
            var assemblyFilePaths = Instances.AssemblyFilePathOperator.Get_DependencyAssemblyFilePaths_Inclusive(
                assemblyFilePath);

            this.In_MetadataLoadContext(
                assemblyFilePaths,
                metadataLoadContext =>
                {
                    var assembly = metadataLoadContext.LoadFromAssemblyPath(assemblyFilePath);

                    action(assembly);
                });
        }

        public async Task In_AssemblyContext(
            string assemblyFilePath,
            Func<Assembly, Task> action)
        {
            var assemblyFilePaths = Instances.AssemblyFilePathOperator.Get_DependencyAssemblyFilePaths_Inclusive(
                assemblyFilePath);

            await this.In_MetadataLoadContext(
                assemblyFilePaths,
                async metadataLoadContext =>
                {
                    var assembly = metadataLoadContext.LoadFromAssemblyPath(assemblyFilePath);

                    await action(assembly);
                });
        }

        public void In_MetadataLoadContext(
            IEnumerable<string> assemblyFilePaths,
            Action<MetadataLoadContext> action)
        {
            using var metadataLoadContext = this.Get_MetadataLoadContext(assemblyFilePaths);

            action(metadataLoadContext);
        }

        public async Task In_MetadataLoadContext(
            IEnumerable<string> assemblyFilePaths,
            Func<MetadataLoadContext, Task> action)
        {
            using var metadataLoadContext = this.Get_MetadataLoadContext(assemblyFilePaths);

            await action(metadataLoadContext);
        }
    }
}
