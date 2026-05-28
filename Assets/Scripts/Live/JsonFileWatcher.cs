// C11 — JsonFileWatcher (F5).
// FileSystemWatcher + 100 ms debounce + lock-file handling + polling fallback.
// Thread-safe: watcher events arrive on a background thread; this class
// marshals them to the main thread via a volatile flag + Update() polling.
// Polling fallback fires every 500 ms in case the watcher misses an event
// (R5 in the risk register).

#if DEVELOPMENT_BUILD || UNITY_EDITOR

using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace RIMA.Live
{
    /// <summary>
    /// Watches a single JSON file for changes and fires <see cref="OnFileChanged"/>
    /// on the Unity main thread after a 100 ms debounce.
    /// Attach to a GameObject via <see cref="LiveRoomReloader"/> (not standalone).
    /// </summary>
    public sealed class JsonFileWatcher : MonoBehaviour
    {
        // ── Public API ─────────────────────────────────────────────────────────

        /// <summary>Fired on the main thread when the watched file has changed and is ready to read.</summary>
        public event Action OnFileChanged;

        // ── Config ─────────────────────────────────────────────────────────────

        private const double DebounceMs     = 100.0;
        private const double LockTimeoutMs  = 500.0;
        private const double PollIntervalMs = 500.0;

        // ── State ──────────────────────────────────────────────────────────────

        private FileSystemWatcher _watcher;

        /// <summary>Set from background thread; consumed on main thread in Update.</summary>
        private volatile bool _pendingNotify;

        /// <summary>Timestamp of last watcher event (background thread writes, main thread reads).</summary>
        private double _lastEventTimeMs;   // written under _lock
        private readonly object _lock = new object();

        private string _filePath;
        private string _lockFilePath;

        // Polling fallback
        private double _lastPollCheckMs;
        private DateTime _lastKnownWriteTime;

        // ── Init / Teardown ────────────────────────────────────────────────────

        /// <summary>
        /// Start watching the given file. Safe to call from main thread only.
        /// </summary>
        public void StartWatching(string filePath)
        {
            _filePath     = filePath;
            _lockFilePath = filePath + ".lock";

            // Prime the known write time so we don't fire immediately.
            if (File.Exists(_filePath))
                _lastKnownWriteTime = File.GetLastWriteTimeUtc(_filePath);
            else
                _lastKnownWriteTime = DateTime.MinValue;

            _lastPollCheckMs = Time.realtimeSinceStartupAsDouble * 1000.0;

            try
            {
                string dir  = Path.GetDirectoryName(_filePath);
                string file = Path.GetFileName(_filePath);

                _watcher = new FileSystemWatcher(dir, file)
                {
                    NotifyFilter        = NotifyFilters.LastWrite | NotifyFilters.Size,
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = false,
                };

                _watcher.Changed += OnWatcherEvent;
                _watcher.Created += OnWatcherEvent;

                Debug.Log($"[JsonFileWatcher] Watching: {_filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[JsonFileWatcher] Could not start FileSystemWatcher ({ex.Message}). Polling only.");
                _watcher = null;
            }
        }

        private void OnDestroy()
        {
            StopWatching();
        }

        private void StopWatching()
        {
            if (_watcher == null) return;
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;
        }

        // ── Background thread callback ─────────────────────────────────────────

        private void OnWatcherEvent(object sender, FileSystemEventArgs e)
        {
            // Record event time on background thread; debounce happens on main thread.
            lock (_lock)
            {
                _lastEventTimeMs = CurrentTimeMs();
                _pendingNotify   = true;
            }
        }

        // ── Main thread Update ─────────────────────────────────────────────────

        private void Update()
        {
            double nowMs = CurrentTimeMs();

            // ── Polling fallback (500 ms) ──────────────────────────────────────
            if (nowMs - _lastPollCheckMs >= PollIntervalMs)
            {
                _lastPollCheckMs = nowMs;
                if (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath))
                {
                    DateTime wt = File.GetLastWriteTimeUtc(_filePath);
                    if (wt > _lastKnownWriteTime)
                    {
                        _lastKnownWriteTime = wt;
                        lock (_lock)
                        {
                            _lastEventTimeMs = nowMs;
                            _pendingNotify   = true;
                        }
                    }
                }
            }

            // ── Debounce check ─────────────────────────────────────────────────
            if (!_pendingNotify) return;

            double lastEvent;
            lock (_lock) { lastEvent = _lastEventTimeMs; }

            if (nowMs - lastEvent < DebounceMs) return; // still within debounce window

            // Clear flag before doing IO so we don't miss a concurrent event.
            lock (_lock) { _pendingNotify = false; }

            if (!File.Exists(_filePath))
            {
                Debug.LogWarning($"[JsonFileWatcher] File gone: {_filePath}");
                return;
            }

            // ── Lock file handling ─────────────────────────────────────────────
            // Wait up to LockTimeoutMs for writer to release the lock.
            double lockWaitStart = CurrentTimeMs();
            while (File.Exists(_lockFilePath))
            {
                if (CurrentTimeMs() - lockWaitStart > LockTimeoutMs)
                {
                    Debug.LogWarning("[JsonFileWatcher] Lock file timeout — forcing read (writer may have crashed).");
                    break;
                }
                Thread.Sleep(10);
            }

            // Update known write time so polling doesn't double-fire.
            _lastKnownWriteTime = File.GetLastWriteTimeUtc(_filePath);

            OnFileChanged?.Invoke();
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private static double CurrentTimeMs() =>
            (double)System.Diagnostics.Stopwatch.GetTimestamp()
            / System.Diagnostics.Stopwatch.Frequency * 1000.0;
    }
}

#endif // DEVELOPMENT_BUILD || UNITY_EDITOR
