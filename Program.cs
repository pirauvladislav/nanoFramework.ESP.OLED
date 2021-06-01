//************
// EEprom references
// https://github.com/Dweaver309/nanoframework.EEprom
// https://www.hackster.io/dweaver309/eeprom-i2c-driver-for-nanoframework-718669
//************

//************
// SS1306 Oled display references
// https://github.com/Dweaver309/nanoframework.SS1306
// https://www.hackster.io/dweaver309/oled-display-driver-for-nanoframework-8744e7
//************


using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using nanoframework.i2c.SS1306;

namespace nanoframework.I2C.driver
{
    public class Program
    {
        private static OLED oled;
        public static void Main()
        {
            oled = new OLED(OLED.DeviceConnectionSting.I2C1, 0x3C);
            oled.Initialize();

            DisplayShowStartScreen();

            var gpioController = new GpioController();
            var yesButton = gpioController.OpenPin(12, PinMode.Input);
            yesButton.DebounceTimeout = TimeSpan.FromMilliseconds(10);
            yesButton.ValueChanged += YesButton_ValueChanged;


            var noButton = gpioController.OpenPin(14, PinMode.Input);
            noButton.DebounceTimeout = TimeSpan.FromMilliseconds(10);
            noButton.ValueChanged += NoButton_ValueChanged;

            while (true) { Thread.Sleep(3000); };
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
            oled.ClearScreen();

            oled.Write(0, 1, "Hello Daniel !!!");

            oled.Write(0, 3, "Do you want to be");
            oled.Write(0, 5, "a programmer ?");
            oled.Write(0, 7, "Yes     No");
        }

        private static void DisplayShowYesAnswer()
        {
            oled.ClearScreen();

            oled.Write(0, 1, "COOOOOOLLLLLL !!!");

            oled.Write(0, 3, "You will be one ");
            oled.Write(0, 5, "of the greatest");
            oled.Write(0, 7, "programmer!");
        }

        private static void DisplayShowNoAnswer()
        {
            oled.ClearScreen();

            oled.Write(0, 1, "Gooooooddddd !!!");
            oled.Write(0, 3, "And which of");
            oled.Write(0, 5, "great profession");
            oled.Write(0, 7, "will you choose?");
        }
    }
}
