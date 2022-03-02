using System;
using System.Collections.Generic;

namespace CoreTestApp;

internal class Program
{
    private static void Main()
    {
        var abort = new AbortRequest();
        var animations = GetAnimations();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            abort.IsAbortRequested = true;
        };

        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("What do you want to test:" + Environment.NewLine);
            Console.WriteLine("0 - Exit");
            Console.WriteLine("1 - Color wipe animation");
            Console.WriteLine("2 - Rainbow color animation" + Environment.NewLine);
            Console.WriteLine("Press CTRL+C to abort current test." + Environment.NewLine);
            Console.Write("What is your choice: ");
            input = int.Parse(Console.ReadLine() ?? "0");

            if (!animations.ContainsKey(input)) {
                continue;
            }

            abort.IsAbortRequested = false;
            animations[input].Execute(abort);

        } while (input != 0);
    }

    private static Dictionary<int, IAnimation> GetAnimations()
    {
        var result = new Dictionary<int, IAnimation> {
            [1] = new ColorWipe(),
            [2] = new RainbowColorAnimation()
        };

        return result;
    }
}