using System;
using System.Collections.Generic;

namespace Alitz;
public static class Program
{
    public static void Main(string[] args)
    {
        var loop = MainLoop.CreateBuilder()
            .SetInputAction(
                (consoleKeyInfo, loopStopper) =>
                {
                    if (consoleKeyInfo is not null)
                    {
                        if (consoleKeyInfo.Value.Key == ConsoleKey.Escape)
                        {
                            loopStopper();
                        }
                        List<string> inputParts = new(4);
                        var modifiers = consoleKeyInfo.Value.Modifiers;
                        if (modifiers.HasFlag(ConsoleModifiers.Control))
                        {
                            inputParts.Add("Ctrl");
                        }
                        if (modifiers.HasFlag(ConsoleModifiers.Shift))
                        {
                            inputParts.Add("Shift");
                        }
                        if (modifiers.HasFlag(ConsoleModifiers.Alt))
                        {
                            inputParts.Add("Alt");
                        }
                        inputParts.Add(consoleKeyInfo.Value.KeyChar.ToString());
                        string inputPartsCombined = string.Join('+', inputParts);
                        Console.WriteLine($"Input: {inputPartsCombined}");
                    }
                })
            .Build();

        loop.Start();
    }
}
