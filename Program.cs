//************
// SS1306 Oled display references
// https://github.com/Dweaver309/nanoframework.SS1306
// https://www.hackster.io/dweaver309/oled-display-driver-for-nanoframework-8744e7
//************


using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using ESP32.OLED.SS1306;

namespace ESP32.OLED
{
    public class Program
    {
        private static OledDisplay display;
        public static void Main()
        {
            display = new OledDisplay(OledDisplay.DeviceConnectionSting.I2C1, 0x3C);
            display.Initialize();

            DisplayShowStartScreen();

            var gpioController = new GpioController();
            var yesButton = gpioController.OpenPin(12, PinMode.Input);
            yesButton.DebounceTimeout = TimeSpan.FromMilliseconds(10);
            yesButton.ValueChanged += YesButton_ValueChanged;


            var noButton = gpioController.OpenPin(14, PinMode.Input);
            noButton.DebounceTimeout = TimeSpan.FromMilliseconds(10);
            noButton.ValueChanged += NoButton_ValueChanged;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void NoButton_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.ChangeType == PinEventTypes.Rising)
                DisplayShowNoAnswer();
            else
            {
                Thread.Sleep(3000);
                DisplayShowStartScreen();
            }
        }

        private static void YesButton_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            if (e.ChangeType == PinEventTypes.Rising)
                DisplayShowYesAnswer();
            else
            {
                Thread.Sleep(3000);
                DisplayShowStartScreen();
            }
        }

        private static void DisplayShowStartScreen()
        {
            display.ClearScreen();

            display.Write(0, 1, "Hello Daniel !!!");

            display.Write(0, 3, "Do you want to be");
            display.Write(0, 5, "a programmer ?");
            display.Write(0, 7, "Yes     No");
        }

        private static void DisplayShowYesAnswer()
        {
            display.ClearScreen();

            display.Write(0, 1, "COOOOOOLLLLLL !!!");

            display.Write(0, 3, "You will be one ");
            display.Write(0, 5, "of the greatest");
            display.Write(0, 7, "programmer!");
        }

        private static void DisplayShowNoAnswer()
        {
            display.ClearScreen();

            display.Write(0, 1, "Gooooooddddd !!!");
            display.Write(0, 3, "And which of");
            display.Write(0, 5, "great profession");
            display.Write(0, 7, "will you choose?");
        }
    }
}
