#region

using System.Drawing;
using System.Linq;

#endregion

namespace rpi_ws281x;

/// <summary>
///     Represents the controller which drives the LEDs
/// </summary>
public class Controller {
	/// <summary>
	///     Returns a value which indicates if the signal needs to be inverted.
	///     Set to true to invert the signal (when using NPN transistor level shift).
	/// </summary>
	public bool Invert { get; }

	/// <summary>
	///     Gets or sets the brightness of the LEDs
	///     0 = darkest, 255 = brightest
	/// </summary>
	public byte Brightness { get; set; }

	/// <summary>
	///     The type of controller (i.e. PWM, PCM, SPI  )
	/// </summary>
	/// <value></value>
	public ControllerType ControllerType { get; }

	/// <summary>
	///     The number of LEDs in the strip
	/// </summary>
	public int LEDCount {
		get => LEDColors.Length;
		set {
			var nc = new Color[value];
			for (var i = 0; i < value; i++) {
				nc[i] = Color.FromArgb(0, 0, 0, 0);
			}

			IsDirty = true;
		}
	}

	/// <summary>
	///     Returns the type of the channel.
	///     The type defines the ordering of the colors.
	/// </summary>
	public StripType StripType { get; }

	/// <summary>
	///     Indicates if the colors assigned to the LED has changed and the LED should be updated.
	/// </summary>
	internal bool IsDirty { get; set; }

	/// <summary>
	///     Returns the GPIO pin which is connected to the LED strip
	/// </summary>
	internal int GPIOPin { get; }

	private Color[] LEDColors { get; set; }

	internal Controller(int ledCount, Pin pin, byte brightness, bool invert, StripType stripType,
		ControllerType controllerType) {
		IsDirty = false;

		GPIOPin = (int)pin;
		Invert = invert;
		Brightness = brightness;
		StripType = stripType;
		ControllerType = controllerType;

		LEDColors = Enumerable.Range(0, ledCount).Select(_ => Color.FromArgb(0, 0, 0, 0)).ToArray();
	}

	/// <summary>
	///     Set LED to a Color
	/// </summary>
	/// <param name="ledID">LED to set (0 based)</param>
	/// <param name="color">Color to use</param>
	public void SetLED(int ledID, Color color) {
		var cName = StripType.ToString();
		if (cName.Contains("W") && cName.Contains("SK")) {
			color = ColorClamp.ClampAlpha(color);
		}

		LEDColors[ledID] = color;
		IsDirty = true;
	}

	public void SetLEDS(Color[] color) {
		LEDColors = color;
		IsDirty = true;
	}

	/// <summary>
	///     Set all the LEDs in the strip to same color
	/// </summary>
	/// <param name="color">color to set all the LEDs</param>
	public void SetAll(Color color) {
		var cName = StripType.ToString();
		if (cName.Contains("W") && cName.Contains("SK")) {
			color = ColorClamp.ClampAlpha(color);
		}

		var nc = new Color[LEDCount];
		for (var i = 0; i < LEDCount; i++) {
			nc[i] = color;
		}

		IsDirty = true;
	}


	/// <summary>
	///     Turn off all the LEDs in the strip
	/// </summary>
	public void Reset() {
		var nc = new Color[LEDCount];
		for (var i = 0; i < LEDCount; i++) {
			nc[i] = Color.FromArgb(0, 0, 0, 0);
		}

		LEDColors = nc;
		IsDirty = true;
	}

	/// <summary>
	///     array of LEDs with numeric color values
	/// </summary>
	/// <param name="clearDirty">reset dirty flag</param>
	internal int[] GetColors(bool clearDirty = false) {
		if (clearDirty) {
			IsDirty = false;
		}

		return LEDColors.Select(x => x.ToArgb()).ToArray();
	}
}