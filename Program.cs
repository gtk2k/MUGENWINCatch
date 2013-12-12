using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using WindowsInput;
using System.Collections.Generic;

namespace MUGENWINCatch
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
        [DllImport("KERNEL32.DLL")]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

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
            var loiloPath = Properties.Settings.Default.loiloPath;
            var loiloDir = Path.GetDirectoryName(loiloPath);
            var mugenPath = Properties.Settings.Default.mugenPath;
            var mugenDir = Path.GetDirectoryName(mugenPath);
            var ffmpegPath = Properties.Settings.Default.ffmpegPath;
            var ffmpegDir = Path.GetDirectoryName(ffmpegPath);

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

            // MUGENのログファイルが残ってる場合は削除する
            // ログは追記され、扱いが面倒になるため
            var mugenLogFile = Path.Combine(mugenDir, "log.txt");
            if (File.Exists(mugenLogFile))
            {
                File.Delete(mugenLogFile);
            }

            // AI(CPU)同士でクイック戦闘モード起動のコマンドライン引数を設定
            var mugenargs = "-p1 " + p1 + " -p1.ai 1 -p1.life 1" + " -p2 " + p2 + " -p2.ai 1 -p2.life 1 -rounds 2 -log log.txt";
            ProcessStartInfo mugenPsi = new ProcessStartInfo(mugenPath, mugenargs);
            mugenPsi.WorkingDirectory = mugenDir;
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
            // 起動直後、録画の準備が整うまでPAUSEキーをおして一時停止しておく
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
            // MUGENの一時停止を解除
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

            // MUGENが終了するまで待機
            mugen.WaitForExit();

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

            // ffmpegでmp4に変換
            var fileName = Path.GetFileNameWithoutExtension(aviPath);
            var mp4FilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Properties.Settings.Default.mp4Path);
            mp4FilePath = Path.Combine(mp4FilePath, fileName + ".mp4");
            var ffmpegArgs = "-i \"" + aviPath + "\" -vcodec libx264 -vprofile high -preset slow -b:v 1000k -vf scale=-1:720 -threads 0 -acodec libvo_aacenc -b:a 196k \"" + mp4FilePath + "\"";
            var ffmpegPsi = new ProcessStartInfo(Properties.Settings.Default.ffmpegPath, ffmpegArgs);
            var ffmpeg = Process.Start(ffmpegPsi);
            ffmpeg.WaitForExit();

            // AVIファイル削除
            File.Delete(aviPath);

            // MUGENのログファイルよりバトル結果を取得
            var battleResult = new List<string>();
            var sb = new StringBuilder(1024);
            for (var i = 1; i <= 3; i++)
            {
                GetPrivateProfileString("Match 1 Round " + i, "winningteam", "", sb, 1024, mugenLogFile);
                if (sb.Length > 0)
                {
                    battleResult.Add(sb.ToString());
                }
                sb.Length = 0;
            }
            GetPrivateProfileString("Match 1 Round 3", "winningteam", "", sb, 1024, mugenLogFile);
            var r3 = sb.ToString();

            // バトル結果出力
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(mp4FilePath), Path.GetFileNameWithoutExtension(mp4FilePath) + "_result.txt"), string.Join("\r\n", battleResult.ToArray()));

            // MP4を再生してみる
            Process.Start(mp4FilePath);
        }

        static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            // 作成されたAVIファイルパスを取得
            aviPath = e.FullPath;
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
