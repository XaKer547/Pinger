using System.Net;
using System.Net.NetworkInformation;

namespace Pinger
{
    internal class Program
    {
        const ConsoleColor BaseColor = ConsoleColor.Green;
        static async Task Main(string[] args)
        {
            SetBaseColor();

            if (args.Length != 2)
            {
                WriteLineError("Нужно два аргумента, начальный адрес и конечный");

                return;
            }

            if (!IPAddress.TryParse(args[0], out var addressStart))
            {
                WriteLineError("Не удалось опознать первый IP адрес");
                return;
            }

            if (!IPAddress.TryParse(args[1], out var addressEnd))
            {
                WriteLineError("Не удалось опознать второй IP адрес");
                return;
            }

            if (addressStart.AddressFamily != addressEnd.AddressFamily)
            {
                WriteLineError("Адреса разного типа");
                return;
            }

            if (addressStart.GetAddressBytes().Length == 4)
            {
                await RunIPv4Ping(addressStart, addressEnd);
            }
            else
            {
                throw new NotImplementedException();

                await RunIPv6Ping(addressStart, addressEnd);
            }
        }

        static async Task RunIPv4Ping(IPAddress addressStart, IPAddress addressEnd)
        {
            using Ping pinger = new();

            var startBytes = addressStart.GetAddressBytes();

            var endBytes = addressEnd.GetAddressBytes();

            while (true)
            {
                var address = string.Join(".", startBytes);

                await PingAsync(pinger, address);

                if (Enumerable.SequenceEqual(startBytes, endBytes))
                    break;

                TryIncrement(3, ref startBytes);
            }
        }

        static void TryIncrement(int index, ref byte[] array)
        {
            if (index < 0 || index >= array.Length)
                return;

            if (array[index] == byte.MaxValue)
            {
                array[index] = 0;

                TryIncrement(index - 1, ref array);
            }
            else array[index]++;
        }

        static async Task RunIPv6Ping(IPAddress addressStart, IPAddress addressEnd)
        {
            using Ping pinger = new();

            for (int i = 235; i < 256; i++)
            {
                var address = $"{12}.{i}";

                await PingAsync(pinger, address);
            }
        }

        static async Task PingAsync(Ping pinger, string address)
        {
            var reply = await pinger.SendPingAsync(address, 3);

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine($"{address} обнаружен");
            }
            else
            {
                Console.WriteLine($"{address} не доступен");
            }
        }

        static void WriteLineError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(text);

            SetBaseColor();
        }

        static void SetBaseColor() => Console.ForegroundColor = BaseColor;
    }
}