using System;
using System.Threading.Tasks;
using CommandLine;


namespace dotnet_version
{
    [Verb("get", HelpText = "get the current version")]
    public struct GetOptions : IOptions
    {
        public bool   Major { get; set; }
        public bool   Minor { get; set; }
        public bool   Patch { get; set; }
        public string Project { get; set; }
    }

    public class Get
    {
        public static async Task<int> ExecuteAsync(GetOptions opts)
        {
            Project proj = new Project(opts.Project);
            Console.WriteLine($"{proj.Version}");
            // Console.WriteLine($"{proj.VersionTuple}");

            return await Task.Run(() => 0);
        }
    }
}
