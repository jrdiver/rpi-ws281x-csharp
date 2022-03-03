#region

using System;
using System.Collections.Generic;

#endregion

namespace CoreTestApp;

internal class Program {
	private static AbortRequest _abort;
	private static Dictionary<int, IAnimation> _animations;
	private static int _input;

	private static void Main() {
		_abort = new AbortRequest();
		_animations = GetAnimations();
		Console.CancelKeyPress += (_, e) => {
			e.Cancel = true;
			_abort.IsAbortRequested = true;
		};

		do {
			MainMenu();
		} while (_input != 0);
	}

	private static void MainMenu() {
		Console.Clear();
		Console.WriteLine("What do you want to test:" + Environment.NewLine);
		Console.WriteLine("0 - Exit");
		Console.WriteLine("1 - Color wipe animation");
		Console.WriteLine("2 - Rainbow color animation" + Environment.NewLine);
		Console.WriteLine("Press CTRL+C to abort current test." + Environment.NewLine);
		Console.Write("What is your choice: ");
		_input = int.Parse(Console.ReadLine() ?? "0");
		if (_input == 3) {
			LedCountTest();
		}

		if (!_animations.ContainsKey(_input)) {
			return;
		}

		_abort.IsAbortRequested = false;
		_animations[_input].Execute(_abort);
	}

	private static void LedCountTest() {
	}

	private static Dictionary<int, IAnimation> GetAnimations() {
		var result = new Dictionary<int, IAnimation> {
			[1] = new ColorWipe(),
			[2] = new RainbowColorAnimation()
		};

		return result;
	}
}