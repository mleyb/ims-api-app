using GetHandlers;
using ImportHandlers;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            ImportResponse ir = await new Import().HandleRequestAsync(new ImportRequest { CustomerId = "Dog", VIN = "WDBJF65J1YB039105" });

            GetResponse gr = await new Get().HandleRequestAsync(new GetRequest { VIN = "WDBJF65J1YB039105" }, null);

            Console.WriteLine();
        }
    }
}
