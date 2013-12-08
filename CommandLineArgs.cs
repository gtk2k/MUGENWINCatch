using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUGENWINCatch
{
    public enum RecorderMode
    {
        GrabBackBuffer,
        WindowShot,
        ScreenShot
    }


    public class CommandLineArgs
    {
        public bool IsNothing
        {
            get;
            private set;
        }

        public static CommandLineArgs Nothing
        {
            get;
            private set;
        }

        public RecorderMode RecorderMode
        {
            get;
            private set;
        }

        public IntPtr WindowHandle
        {
            get;
            private set;
        }

        public double WindowStartupLeft
        {
            get;
            private set;
        }

        public double WindowStartupTop
        {
            get;
            private set;
        }

        static CommandLineArgs()
        {
            CommandLineArgs.Nothing = new CommandLineArgs(true, IntPtr.Zero, 0, 0, RecorderMode.GrabBackBuffer);
        }

        private CommandLineArgs(bool isNothing, IntPtr windowHandle, double windowStartupLeft, double windowStartupTop, RecorderMode recorderMode)
        {
            this.IsNothing = isNothing;
            this.WindowHandle = windowHandle;
            this.WindowStartupLeft = windowStartupLeft;
            this.WindowStartupTop = windowStartupTop;
            this.RecorderMode = recorderMode;
        }

        public CommandLineArgs(IntPtr windowHandle, double windowStartupLeft, double windowStartupTop, RecorderMode recorderMode)
            : this(false, windowHandle, windowStartupLeft, windowStartupTop, recorderMode)
        {
        }

        private static string GetCommandLineValue(string arg, string option)
        {
            if (arg.Length <= option.Length || !arg.StartsWith(option, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            return arg.Substring(option.Length, arg.Length - option.Length);
        }

        public static CommandLineArgs Parse(string[] args)
        {
            int num;
            long num1;
            double num2;
            double num3;
            int num4;
            if ((int)args.Length <= 0)
            {
                return CommandLineArgs.Nothing;
            }
            IntPtr? nullable = null;
            double? nullable1 = null;
            double? nullable2 = null;
            RecorderMode? nullable3 = null;
            string[] strArrays = args;
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                if (nullable.HasValue && nullable1.HasValue && nullable2.HasValue && nullable3.HasValue)
                {
                    break;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    if (!nullable.HasValue)
                    {
                        string commandLineValue = CommandLineArgs.GetCommandLineValue(str, "/t:");
                        if (string.IsNullOrEmpty(commandLineValue))
                        {
                            goto Label1;
                        }
                        if (IntPtr.Size == 4)
                        {
                            if (int.TryParse(commandLineValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
                            {
                                nullable = new IntPtr?(new IntPtr(num));
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (long.TryParse(commandLineValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out num1))
                        {
                            nullable = new IntPtr?(new IntPtr(num1));
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                Label1:
                    if (!nullable1.HasValue)
                    {
                        string commandLineValue1 = CommandLineArgs.GetCommandLineValue(str, "/x:");
                        if (string.IsNullOrEmpty(commandLineValue1))
                        {
                            continue;
                        }
                        if (double.TryParse(commandLineValue1, NumberStyles.Float, CultureInfo.InvariantCulture, out num2))
                        {
                            nullable1 = new double?(num2);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                Label2:
                    if (!nullable2.HasValue)
                    {
                        string str1 = CommandLineArgs.GetCommandLineValue(str, "/y:");
                        if (string.IsNullOrEmpty(str1))
                        {
                            goto Label3;
                        }
                        if (double.TryParse(str1, NumberStyles.Float, CultureInfo.InvariantCulture, out num3))
                        {
                            nullable2 = new double?(num3);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                Label3:
                    if (!nullable3.HasValue)
                    {
                        string commandLineValue2 = CommandLineArgs.GetCommandLineValue(str, "/m:");
                        if (!string.IsNullOrEmpty(commandLineValue2) && int.TryParse(commandLineValue2, NumberStyles.Integer, CultureInfo.InvariantCulture, out num4))
                        {
                            nullable3 = new RecorderMode?(RecorderModeConverter.PresetAt(num4));
                        }
                    }
                }
            }
            if (!nullable.HasValue || !nullable1.HasValue || !nullable2.HasValue || !nullable3.HasValue)
            {
                return CommandLineArgs.Nothing;
            }
            return new CommandLineArgs(nullable.Value, nullable1.Value, nullable2.Value, nullable3.Value);
        }

        public override string ToString()
        {
            string str;
            if (this.IsNothing)
            {
                return string.Empty;
            }
            if (IntPtr.Size != 4)
            {
                long num = this.WindowHandle.ToInt64();
                str = num.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                int num1 = this.WindowHandle.ToInt32();
                str = num1.ToString(CultureInfo.InvariantCulture);
            }
            object[] objArray = new object[] { str, null, null, null };
            double windowStartupLeft = this.WindowStartupLeft;
            objArray[1] = windowStartupLeft.ToString(CultureInfo.InvariantCulture);
            double windowStartupTop = this.WindowStartupTop;
            objArray[2] = windowStartupTop.ToString(CultureInfo.InvariantCulture);
            int recorderMode = (int)this.RecorderMode;
            objArray[3] = recorderMode.ToString(CultureInfo.InvariantCulture);
            return string.Format("/t:{0} /x:{1} /y:{2} /m:{3}", objArray);
        }
    }
    public static class RecorderModeConverter
    {
        public static RecorderMode PresetAt(int presetIndex)
        {
            RecorderMode recorderMode;
            IEnumerator enumerator = Enum.GetValues(typeof(RecorderMode)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    RecorderMode current = (RecorderMode)enumerator.Current;
                    if ((int)current != presetIndex)
                    {
                        continue;
                    }
                    recorderMode = current;
                    return recorderMode;
                }
                return RecorderMode.GrabBackBuffer;
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
