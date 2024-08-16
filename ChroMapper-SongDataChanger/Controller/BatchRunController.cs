using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using ChroMapper_SongDataChanger.Configuration;
using ChroMapper_SongDataChanger.Util;

namespace ChroMapper_SongDataChanger.Controller
{
    public class BatchRunController
    {
        public bool IsRunning { get; private set; } = false;
        public event Action OnBatchFinished;
        public Process _batchRunProcess;
        private static BatchRunController instance;
        public static BatchRunController Instance
        {
            get
            {
                if (instance is null)
                    instance = new BatchRunController();
                return instance;
            }
        }
        public IEnumerator BatchRun()
        {
            this.IsRunning = true;
            var songFIle = Path.Combine(BeatSaberSongContainer.Instance.Song.Directory, BeatSaberSongContainer.Instance.Song.SongFilename);
            if (!File.Exists(songFIle) || !File.Exists(Options.Instance.batachFilePath))
            {
                this.IsRunning = false;
                yield break;
            }
            var si = new ProcessStartInfo
            {
                FileName = Options.Instance.batachFilePath,
                Arguments = $@"""{songFIle}""",
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                UseShellExecute = true
            };
            using (this._batchRunProcess = new Process())
            {
                this._batchRunProcess.EnableRaisingEvents = false;
                this._batchRunProcess.PriorityBoostEnabled = true;
                this._batchRunProcess.StartInfo = si;
                Task.Run(() =>
                {
                    this._batchRunProcess.Start();
                    this._batchRunProcess.WaitForExit();
                    this._batchRunProcess.Close();
                });
                var startProcessTimeout = new TimeoutTimer(Options.Instance.batchStartTimeout);
                yield return new WaitUntil(() => IsProcessRunning(this._batchRunProcess) || startProcessTimeout.HasTimedOut);
                startProcessTimeout.Stop();
                var timeout = new TimeoutTimer(Options.Instance.batchRunTimeout);
                yield return new WaitUntil(() => !IsProcessRunning(this._batchRunProcess) || timeout.HasTimedOut);
                timeout.Stop();
                DisposeProcess(this._batchRunProcess);
            }
            this._batchRunProcess = null;
            OnBatchFinished?.Invoke();
            this.IsRunning = false;
        }
        public static void DisposeProcess(Process process)
        {
            if (process == null)
            {
                return;
            }
            int processId;
            try
            {
                processId = process.Id;
            }
            catch (Exception)
            {
                return;
            }
            UnityEngine.Debug.Log($"[{processId}] Cleaning up process");
            if (!process.HasExited)
                process.Kill();
            process.Dispose();
        }
        public static bool IsProcessRunning(Process process)
        {
            try
            {
                if (!process.HasExited)
                    return true;
            }
            catch (Exception e)
            {
                if (!(e is InvalidOperationException))
                {
                    UnityEngine.Debug.LogWarning(e);
                }
            }
            return false;
        }
    }
}
