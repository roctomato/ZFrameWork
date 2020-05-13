using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Zby
{
    /// <summary>
    /// 日志数据类
    /// </summary>
    public class LogData
    {
        public string Log { get; set; }
        public string Track { get; set; }
        public LogType Level { get; set; }
    }

    public class LogFile
    {
        private string _logFileName;
        private StreamWriter _LogWriter = null;

        private Queue<LogData> _WritingLogQueue = null;
        private Queue<LogData> _WaitingLogQueue = null;

        private object _LogLock = null;
        private bool _IsRunning = false;

        private Thread _FileLogThread = null;
        private int _mainThreadID = -1;


        void InitLogFile()
        {
            string logPath = OSTools.GetUserDocumentPath();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            _logFileName = string.Format("{0}/{1}.log", logPath, DateTime.Now.ToString("yyyy-M-d"));

            this._LogWriter = new StreamWriter(_logFileName, true);
            this._LogWriter.AutoFlush = true;

            this._WritingLogQueue = new Queue<LogData>();
            this._WaitingLogQueue = new Queue<LogData>();
        }

        void InitWriteThread()
        {
            this._LogLock = new object();
            this._IsRunning = true;
            _mainThreadID = Thread.CurrentThread.ManagedThreadId;
            this._FileLogThread = new Thread(new ThreadStart(WriteLog));
            this._FileLogThread.Start();
        }

        void WriteLog()
        {
            while (this._IsRunning)
            {
                if (this._WritingLogQueue.Count == 0)
                {
                    lock (this._LogLock)
                    {
                        while (this._WaitingLogQueue.Count == 0)
                            Monitor.Wait(this._LogLock);
                        Queue<LogData> tmpQueue = this._WritingLogQueue;
                        this._WritingLogQueue = this._WaitingLogQueue;
                        this._WaitingLogQueue = tmpQueue;
                    }
                }
                else
                {
                    while (this._WritingLogQueue.Count > 0)
                    {
                        LogData log = this._WritingLogQueue.Dequeue();
                        if (log.Level == LogType.Exception)
                        {
                            string timeStr = string.Format("[{0}] ", DateTime.Now.ToString("yyyy-M-d HH:mm:ss.fff"));
                            this._LogWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                            this._LogWriter.WriteLine(timeStr + log.Log);
                            this._LogWriter.WriteLine(log.Track);
                            this._LogWriter.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            this._LogWriter.WriteLine(log.Log);
                        }
                    }
                }
            }

        }
        void EnableRecvLogMsg(bool enabled)
        {
            if (enabled)
            {
                Application.logMessageReceived += LogCallback;
                Application.logMessageReceivedThreaded += LogMultiThreadCallback;
            }
            else
            {
                Application.logMessageReceived += LogCallback;
                Application.logMessageReceivedThreaded += LogMultiThreadCallback;
            }
        }
        public LogFile()
        {
            InitLogFile();
            InitWriteThread();
            EnableRecvLogMsg(true);
        }

        public void Close()
        {
            EnableRecvLogMsg(false);
            this._IsRunning = false;
            this._LogWriter.Close();
        }


        /// <summary>
        /// 日志调用回调，主线程和其他线程都会回调这个函数，在其中根据配置输出日志
        /// </summary>
        /// <param name="log">日志</param>
        /// <param name="track">堆栈追踪</param>
        /// <param name="type">日志类型</param>
        void LogCallback(string log, string track, LogType type)
        {
            if (this._mainThreadID == Thread.CurrentThread.ManagedThreadId) {
                Output(log, track, type);
            }
        }

        void LogMultiThreadCallback(string log, string track, LogType type)
        {
            if (this._mainThreadID != Thread.CurrentThread.ManagedThreadId) {
                Output(log, track, type);
            }
        }

        void Output(string log, string track, LogType type)
        {
            LogData logData = new LogData
            {
                Log = log,
                Track = track,
                Level = type,
            };
            lock (this._LogLock)
            {
                this._WaitingLogQueue.Enqueue(logData);
                Monitor.Pulse(this._LogLock);
            }
        }
    }
}