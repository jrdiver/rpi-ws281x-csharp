#region

using System;
using System.Drawing;
using System.Threading;
using rpi_ws281x;

#endregion

namespace CoreTestApp;

public class ColorWipe : IAnimation {
	public void Execute(AbortRequest request) {
		Console.Clear();
		Console.Write("How many LEDs do you want to use: ");

		var ledCount = int.Parse(Console.ReadLine() ?? string.Empty);
		var settings = Settings.CreateDefaultSettings();

		settings.AddController(ledCount, Pin.Gpio18, StripType.WS2811_STRIP_RGB);
		using var device = new WS281x(settings);
		while (!request.IsAbortRequested) {
			Wipe(device, Color.Red);
			Wipe(device, Color.Green);
			Wipe(device, Color.Blue);
		}

		device.Reset();
	}

	private static void Wipe(WS281x device, Color color) {
		var controller = device.GetController();
		for (var i = 0; i < controller.LEDCount; i++) {
			controller.SetLED(i, color);
			device.Render();
			var waitPeriod = (int)Math.Max(500.0 / controller.LEDCount, 5.0);
			Thread.Sleep(waitPeriod);
		}
	}
}