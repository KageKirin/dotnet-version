using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;

namespace dotnet_version
{
    [Verb("set", HelpText = "set the current version")]
    public struct SetOptions : IOptions
    {
        public string Project { get; set; }

        [Value(1, MetaName = "version", HelpText = "Version string in semver format", Required = true)] //
        public string Version { get; set; }
    }

    public class Set
    {
        public static async Task<int> ExecuteAsync(SetOptions opts)
        {
            Match match = Project.VersionRegex.Match(opts.Version);

            if (match.Success)
            {
                Project proj = new Project(opts.Project);
                proj.Version = opts.Version;

                await proj.Write();

                return await Get.ExecuteAsync(new GetOptions { Project = proj.FileName });
            }

            Console.Error.WriteLine($"The given version string '{opts.Version}' is not valid.");
            return 1;
        }
    }
}
