using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using System.Resources;
using System.Threading;
using WindowsInput;

namespace MUGENWINCatch
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        [DllImport("User32.dll")]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [Flags]
        internal enum SetWindowPosFlags : int
        {
            SWP_NOSIZE = 1,
            SWP_SHOWWINDOW = 0x40
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        static Process loilo;
        static Process mugen;
        static Bitmap checkBmp;
        static FileSystemWatcher watcher;
        static string aviPath;
        static string p1, p2;
        static ManualResetEvent mevt;
        static StringBuilder battleResult = new StringBuilder();

        [STAThread]
        static void Main(string[] args)
        {
            Properties.Settings.Default.ffmpegPath = @".\ffmpeg\bin\ffmpeg.exe";

            // 引数でキャラクターを設定
            if (args.Length == 2)
            {
                p1 = args[0];
                p2 = args[1];
            }
            else
            {
                // 引数がない(または2つ以外が設定されている)場合はデフォルトキャラを使用
                p1 = "kfm";
                p2 = "kfm";
            }

           
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.loiloPath))
            {
                var frm = new frmSetting();
                frm.txtFFmpegPath.Text = Properties.Settings.Default.ffmpegPath;
                if (frm.ShowDialog() != DialogResult.OK)
                {
                    Console.WriteLine("設定ウィンドウを閉じるボタンで閉じたため終了します...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                if (!File.Exists(Properties.Settings.Default.loiloPath) ||
                    !Directory.Exists(Properties.Settings.Default.aviPath) ||
                    !File.Exists(Properties.Settings.Default.mugenPath) ||
                    !File.Exists(Properties.Settings.Default.ffmpegPath) ||
                    !Directory.Exists(Properties.Settings.Default.mp4Path))
                {
                    var frm = new frmSetting();
                    frm.txtLoiLoPath.Text = Properties.Settings.Default.loiloPath;
                    frm.txtLoiLoAVIOutputFolder.Text = Properties.Settings.Default.aviPath;
                    frm.txtMUGENPath.Text = Properties.Settings.Default.mugenPath;
                    frm.txtFFmpegPath.Text = Properties.Settings.Default.ffmpegPath;
                    frm.txtMP4OutputFolder.Text = Properties.Settings.Default.mp4Path;

                    if (File.Exists(Properties.Settings.Default.loiloPath))
                    {
                        frm.txtLoiLoPath.Text = Properties.Settings.Default.loiloPath;
                    }
                    else
                    {
                        frm.lblLoiLoPath.Text = "LoiLo Game Recorderの実行ファイルが見つかりませんでした。再設定してください。";
                    }
                    if (Directory.Exists(Properties.Settings.Default.aviPath))
                    {
                        frm.txtLoiLoAVIOutputFolder.Text = Properties.Settings.Default.aviPath;
                    }
                    else
                    {
                        frm.lblLoiLoAVIFoloderPath.Text = "LoiLo Game Recorderの保存先フォルダが見つかりませんでした。再設定してください。";
                    }
                    if (File.Exists(Properties.Settings.Default.mugenPath))
                    {
                        frm.txtMUGENPath.Text = Properties.Settings.Default.mugenPath;
                    }
                    else
                    {
                        frm.lblMUGENPath.Text = "M.U.G.E.Nの実行ファイルが見つかりませんでした。再設定してください。";
                    }
                    if (File.Exists(Properties.Settings.Default.ffmpegPath))
                    {
                        frm.txtFFmpegPath.Text = Properties.Settings.Default.ffmpegPath;
                    }
                    else
                    {
                        frm.lblFFmpegPath.Text = "FFmpeg のEXEファイルが見つかりませんでした。再設定してください。";
                    }
                    if (Directory.Exists(Properties.Settings.Default.mp4Path))
                    {
                        frm.txtMP4OutputFolder.Text = Properties.Settings.Default.mp4Path;
                    }
                    else
                    {
                        frm.lblMP4OutputFolderPath.Text = "変換後のMP4ファイル保存先フォルダが見つかりませんでした。再設定してください。";
                    }
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        Console.WriteLine("設定ウィンドウを閉じるボタンで閉じたため終了します...");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            // すでに起動されているLoiLoやMUGENがあれば強制終了させる
            var procs = Process.GetProcesses();
            foreach (var proc in procs)
            {
                if (proc.ProcessName == "mugen")
                {
                    proc.Kill();
                    proc.WaitForExit();
                }
                else if (proc.ProcessName == "LoiLoGameRecorder")
                {
                    proc.Kill();
                    proc.WaitForExit();
                }
            }

            // AI(CPU)同士でクイック戦闘モード起動のコマンドライン引数を設定
            var mugenargs = "-p1 " + p1 + " -p1.ai 1 -p1.life 1" + " -p2 " + p2 + " -p2.ai 1 -p2.life 1 -rounds 2";
            ProcessStartInfo mugenPsi = new ProcessStartInfo(Properties.Settings.Default.mugenPath, mugenargs);
            mugenPsi.WorkingDirectory = Path.GetDirectoryName(Properties.Settings.Default.mugenPath);
            // MUGENを起動
            mugen = Process.Start(mugenPsi);
            mugen.WaitForInputIdle();
            Color px;
            do
            {
                if (mugen.MainWindowHandle == IntPtr.Zero)
                {
                    px = Color.Black;
                }
                else
                {
                    using (var bmp = PrintWnd(mugen.MainWindowHandle))
                    {
                        
                        px = bmp.GetPixel(10, 1);
                    }
                }
            } while (px.R == 0 || px.G == 0 || px.B == 0);
            Thread.Sleep(100);
            // mugenをアクティブにする
            SetForegroundWindow(mugen.MainWindowHandle);
            Thread.Sleep(100);
            // 起動直後、録画の準備が整うまでPAUSEキーをおして一時停止させる
            InputSimulator.SimulateKeyPress(VirtualKeyCode.PAUSE);
            //SetWindowPos(mugen.MainWindowHandle, IntPtr.Zero, 1500, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_SHOWWINDOW);

            // LoiLo Game Recorderのコマンドライン引数を設定
            // /t: ターゲット(MUGEN)のウィンドウハンドル
            // /x: LoiLo Game Recorderのウィンドウの位置x
            // /y: LoiLo Game Recorderのウィンドウの位置y
            // /m: レコーダーモード 0=GrabBackBuffer, 1=WindowShot, 2=ScreenShot
            var loiloargs = "/t:" + mugen.MainWindowHandle.ToInt64().ToString() + " /x:0 /y:0 /m:1"; 
            ProcessStartInfo loiloPsi = new ProcessStartInfo(Properties.Settings.Default.loiloPath, loiloargs);
            loiloPsi.WorkingDirectory = Path.GetDirectoryName(Properties.Settings.Default.loiloPath);
            // LoiLo Game Recorderを起動
            loilo = Process.Start(loiloPsi);
            loilo.WaitForInputIdle();
            // 録画開始できるようになるまでループ
            SetForegroundWindow(loilo.MainWindowHandle);
            do
            {
                if (loilo.MainWindowHandle == IntPtr.Zero)
                {
                    px = Color.Black;
                }
                else
                {
                    using (var bmp = PrintWnd(loilo.MainWindowHandle))
                    {
                        px = bmp.GetPixel(440, 70);
                    }
                }
            } while (px.R != 224 || px.G != 225 || px.B != 225);
            // mugenをアクティブにする
            SetForegroundWindow(mugen.MainWindowHandle);
            Thread.Sleep(100);
            // 一時停止を解除
            InputSimulator.SimulateKeyPress(VirtualKeyCode.PAUSE);
            Thread.Sleep(1000);


            // AVI出力フォルダ監視開始
            watcher = new FileSystemWatcher();
            watcher.Path = Properties.Settings.Default.aviPath;
            watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Created += watcher_Created;
            watcher.EnableRaisingEvents = true;

            // 録画開始
            InputSimulator.SimulateKeyPress(VirtualKeyCode.F6);

            checkBmp = (Bitmap)Properties.Resources.check;

            StateObjClass StateObj = new StateObjClass();
            StateObj.TimerCanceled = false;
            StateObj.mugen = mugen;
            RunTimer(StateObj);
            mugen.WaitForExit();

            //Console.ReadKey();
            mevt = new ManualResetEvent(false);
            mevt.WaitOne();
        }

        static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            // 作成されたAVIファイルパスを取得
            aviPath = e.FullPath;
        }

        static void mugen_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private class StateObjClass
        {
            public Process mugen;
            public int Left2StateCnt;
            public int Right2StateCnt;
            public int Left1StateCnt;
            public int Right1StateCnt;
            public System.Threading.Timer TimerReference;
            public bool TimerCanceled;
        }

        static void RunTimer(StateObjClass stateObj)
        {
            System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(TimerTask);
            var TimerItem = new System.Threading.Timer(TimerDelegate, stateObj, 700, 700);
            stateObj.TimerReference = TimerItem;
        }

        static void TimerTask(object StateObj)
        {
            StateObjClass State = (StateObjClass)StateObj;

            if (State.TimerCanceled)
            {
                State.TimerReference.Dispose();
                System.Diagnostics.Debug.WriteLine("wincatchスレッド終了");
            }
            
            //SetForegroundWindow(State.Target.MainWindowHandle); 
            using (var mugenBmp = PrintWnd(mugen.MainWindowHandle))
            {
                if (mugenBmp == null)
                {
                    Console.Write("勝負結果の取得に失敗した可能性があります。");
                    State.TimerCanceled = true;
                    stopRecord();
                    return;
                }
                var p = mugenBmp.GetPixel(558, 60);
                if (p.R != 8 || p.G != 12 || p.B != 8)
                {
                    // 戦闘中じゃない
                    return;
                }

                if (!Compare(mugenBmp, 558, 50))
                {
                    State.Left2StateCnt++;
                    if (State.Left2StateCnt == 2)
                    {
                        Console.WriteLine("左2勝");
                        battleResult.AppendLine("左2勝");
                        State.TimerCanceled = true;
                        stopRecord();
                        return;
                    }
                }
                else
                {
                    State.Left2StateCnt = 0;
                }

                if (!Compare(mugenBmp, 718, 50))
                {
                    State.Right2StateCnt++;
                    if (State.Right2StateCnt == 2)
                    {
                        Console.WriteLine("右2勝");
                        battleResult.AppendLine("右2勝");
                        State.TimerCanceled = true;
                        stopRecord();
                        return;
                    }
                }
                else
                {
                    State.Right2StateCnt = 0;
                }

                using (var bmp = new Bitmap(10, 10))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(mugenBmp, new Rectangle(0, 0, 10, 10), new Rectangle(582, 50, 10, 10), GraphicsUnit.Pixel);
                    bmp.Save("left1.png", ImageFormat.Png);
                }
                if (!Compare(mugenBmp, 582, 50))
                {
                    State.Left1StateCnt++;
                    if (State.Left1StateCnt == 2)
                    {
                        Console.WriteLine("左1勝");
                        battleResult.AppendLine("左1勝");
                    }
                }
                else
                {
                    State.Left1StateCnt = 0;
                }

                using (var bmp = new Bitmap(10, 10))
                using (var g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(mugenBmp, new Rectangle(0, 0, 10, 10), new Rectangle(694, 50, 10, 10), GraphicsUnit.Pixel);
                    bmp.Save("right1.png", ImageFormat.Png);
                }
                if (!Compare(mugenBmp, 694, 50))
                {
                    State.Right1StateCnt++;
                    if (State.Right1StateCnt == 3)
                    {
                        Console.WriteLine("右1勝");
                        battleResult.AppendLine("右1勝");
                    }
                }
                else
                {
                    State.Right1StateCnt = 0;
                }
            }
        }

        static bool Compare(Bitmap targetBmp, int offsetX, int offsetY)
        {
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    var tColor = targetBmp.GetPixel(x + offsetX, y + offsetY);
                    var cColor = checkBmp.GetPixel(x, y);
                    if (tColor.R != cColor.R || tColor.G != cColor.G || tColor.B != cColor.B)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void stopRecord()
        {
            // クイック対戦モードは対戦が終了すると、自動的にアプリも終了するみたい。
            // LoiLo Game Recorderもターゲットのアプリが終了すると自動的に録画も停止するため
            // プログラム側から停止する必要もない
            //// 戦闘終了5秒後に録画停止
            //Thread.Sleep(5000);
            //InputSimulator.SimulateKeyPress(VirtualKeyCode.F6);

            // AVIファイル出力完了待機
            var flg = true;
            while (flg)
            {
                try
                {
                    // 500ms待機
                    Thread.Sleep(500);
                    var fs = File.Open(aviPath, FileMode.Open);
                    fs.Close();
                    flg = false;
                }
                catch (Exception)
                {
                    // ファイルが他のプロセス(LoiLoGameRecorder)がつかんでいるかどうかは
                    // 例外でしか判断できない
                }
            }

            // LoiLo Game Recorderを(強制)終了
            loilo.Kill();

            //// MUGENを(強制)終了
            //// クイックモードのときは戦闘終了すると自動で閉じるみたい。
            //try
            //{
            //    mugen.Close();
            //} 
            //catch(Exception)
            //{
            //    mugen.Kill();
            //}
            
            // ffmpegでmp4に変換
            var fileName = Path.GetFileNameWithoutExtension(aviPath);
            var mp4FilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Properties.Settings.Default.mp4Path); 
                mp4FilePath = Path.Combine(mp4FilePath, fileName + ".mp4");
            var args = "-i \"" + aviPath + "\" -vcodec libx264 -vprofile high -preset slow -b:v 1000k -vf scale=-1:720 -threads 0 -acodec libvo_aacenc -b:a 196k \"" + mp4FilePath + "\"";
            var ffmpegPsi = new ProcessStartInfo(Properties.Settings.Default.ffmpegPath, args);
            var ffmpeg = Process.Start(ffmpegPsi);
            ffmpeg.WaitForExit();

            // AVIファイル削除
            File.Delete(aviPath);

            // バトル結果を出力
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(mp4FilePath),  Path.GetFileNameWithoutExtension(mp4FilePath) + "_result.txt"), battleResult.ToString());

            // MP4を再生してみる
            Process.Start(mp4FilePath);

            mevt.Set();
        }

        static Bitmap PrintWnd(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);
            if (rect.Bottom == 0) return null;
            Bitmap img = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top);
            Graphics memg = Graphics.FromImage(img);
            IntPtr dc = memg.GetHdc();
            PrintWindow(hWnd, dc, 0);
            memg.ReleaseHdc(dc);
            memg.Dispose();
            return img;
        }
    }
}
