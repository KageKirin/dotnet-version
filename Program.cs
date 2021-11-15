using System;
using System.Threading.Tasks;
using CommandLine;

namespace dotnet_version
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Parser.Default
                .ParseArguments< //
                    GetOptions,  //
                    SetOptions,  //
                    BumpOptions  //
                    >(args)
                .MapResult(                                        //
                    (GetOptions opts)  => Get.ExecuteAsync(opts),  //
                    (SetOptions opts)  => Set.ExecuteAsync(opts),  //
                    (BumpOptions opts) => Bump.ExecuteAsync(opts), //
                    (errs)             => Task.Run(() => 1));
        }
    }
}
