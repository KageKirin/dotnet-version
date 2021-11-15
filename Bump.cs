using System;
using System.Threading.Tasks;
using CommandLine;

namespace dotnet_version
{
    [Verb("bump", HelpText = "bump the current version")]
    public struct BumpOptions : IOptions
    {
        [Option('M', "major", HelpText = "Bump version major")] //
        public bool Major { get; set; }

        [Option('m', "minor", HelpText = "Bump version minor")] //
        public bool Minor { get; set; }

        [Option('p', "patch", HelpText = "Bump version patch")] //
        public bool Patch { get; set; }
        public string Project { get; set; }
    }

    public class Bump
    {
        public static async Task<int> ExecuteAsync(BumpOptions opts)
        {
            Project proj                      = new Project(opts.Project);
            var     version                   = proj.Version;
            (int major, int minor, int patch) = proj.VersionTuple;

            if (opts.Major)
                major++;

            if (opts.Minor)
                minor++;

            if (opts.Patch)
                patch++;

            proj.Version = $"{major}.{minor}.{patch}";
            await proj.Write();

            return await Get.ExecuteAsync(new GetOptions { Project = proj.FileName });
        }
    }
}
