using System;
using System.Threading.Tasks;
using Thorocaine.Cli;

namespace Thorocaine.ConsoleTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var con = new ConsoleWriter();
            con
               .Progress(0, 0, 10)
               .WriteStatus(1, "Doing Something");
            await Wait();
            con.Progress(0, 1, 10).WriteStatus(2, "Something else");
            await Wait();
            con.Progress(0, 2, 10).WriteStatus(3, ConsoleColor.Red, "Something else again");
            await Wait();
            con.Progress(0, 3, 10).WriteStatus(1, "...Changing");
            await Wait();
            con.Progress(0, 4, 10).WriteStatus(2, "...Changing");
            await Wait();
            con.Progress(0, 5, 10).WriteStatus(3, "...Changing");
            await Wait();
            con.Progress(0, 6, 10).WriteStatus(1, "OKAY", ConsoleColor.Green, "[.]");
            await Wait();
            con.Progress(0, 7, 10).WriteStatus(2, "✅");
            await Wait();
            con.Progress(0, 8, 10).WriteStatus(3, "Done ✔");
            await Wait();
            con.Progress(0, 9, 10).WriteStatus(4, "...Finishing up");
            await Wait();
            con.Progress(0, 10, 10).WriteStatus(4,  ConsoleColor.Green,"Complete");
        }

        static Task Wait() => Task.Delay(1000);
    }
}
