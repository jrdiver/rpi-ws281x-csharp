#region

using System;
using System.Drawing;
using System.Threading;
using rpi_ws281x;

#endregion

namespace CoreTestApp;

public class SetAll : IAnimation {
	public void Execute(AbortRequest request) {
		Console.Clear();
		Console.Write("How many LEDs do you want to use: ");

		var ledCount = int.Parse(Console.ReadLine() ?? string.Empty);
		var settings = Settings.CreateDefaultSettings();

		settings.AddController(ledCount, Pin.Gpio18, StripType.WS2811_STRIP_RGB);
		using var device = new WS281x(settings);
		while (!request.IsAbortRequested) {
			Setall(device, Color.Red);
			Setall(device, Color.Green);
			Setall(device, Color.Blue);
		}

		device.Reset();
	}

	private static void Setall(WS281x device, Color color) {
		var controller = device.GetController();
		controller.SetAll(color);
		device.Render();
		var waitPeriod = (int)Math.Max(500.0 / controller.LEDCount, 5.0);
		Thread.Sleep(waitPeriod);
	}
}