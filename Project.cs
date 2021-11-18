using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommandLine;

namespace dotnet_version
{
    public class Project
    {
        public static Regex VersionRegex = new Regex(
            @"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private XDocument _document { get; }
        private XElement  _version { get; }

        public string FileName { get; }
        public string Version
        {
            get {
                return (_version != null) ? _version.Value : null;
            }
            set {
                if (_version != null)
                    _version.Value = value;
            }
        }

        public Tuple<int, int, int, string, string> VersionTuple
        {
            get {
                int    major = 0, minor = 0, patch = 0;
                string prerelease = "", buildmetadata = "";
                Match  match = VersionRegex.Match(Version);
                if (match.Success)
                {
                    if (match.Groups.ContainsKey("major") && !string.IsNullOrEmpty(match.Groups["major"].Value))
                    {
                        major = Int32.Parse(match.Groups["major"].Value);
                    }
                    if (match.Groups.ContainsKey("minor") && !string.IsNullOrEmpty(match.Groups["minor"].Value))
                    {
                        minor = Int32.Parse(match.Groups["minor"].Value);
                    }
                    if (match.Groups.ContainsKey("patch") && !string.IsNullOrEmpty(match.Groups["patch"].Value))
                    {
                        patch = Int32.Parse(match.Groups["patch"].Value);
                    }
                    if (match.Groups.ContainsKey("prerelease") && !string.IsNullOrEmpty(match.Groups["prerelease"].Value))
                    {
                        prerelease = match.Groups["prerelease"].Value;
                    }
                    if (match.Groups.ContainsKey("buildmetadata") && !string.IsNullOrEmpty(match.Groups["buildmetadata"].Value))
                    {
                        buildmetadata = match.Groups["buildmetadata"].Value;
                    }
                }
                else
                {
                    throw new Exception("failed to parse version string");
                }
                return new Tuple<int, int, int, string, string>(major, minor, patch, prerelease, buildmetadata);
            }
        }

        public Project(string filename)
        {
            if (filename != null)
            {
                FileName = filename;
            }
            else
            {
                var files = Directory.GetFiles(".", "*.csproj");
                if (files.Length > 0)
                {
                    FileName = files[0];
                }
            }

            if (FileName == null)
            {
                throw new Exception("no .csproj found");
            }


            if (FileName != null)
            {
                _document = XDocument.Load(FileName);
            }

            if (_document != null)
            {
                var versionQuery = _document.Root.Elements("PropertyGroup").Elements("Version");

                foreach (var v in versionQuery)
                {
                    _version = v;
                    break;
                }
            }
        }

        public async Task Write()
        {
            using (StreamWriter writer = File.CreateText(FileName))
            {
                await _document.SaveAsync(writer, SaveOptions.None, new CancellationToken());
                await writer.WriteAsync("\n");
            }
        }
    }
}
