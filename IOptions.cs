using System;
using CommandLine;

namespace dotnet_version
{
    interface IOptions
    {
        [Value(0, MetaName = "project", HelpText = "Project file", Default = null)] //
        public string Project { get; set; }
    }
}
