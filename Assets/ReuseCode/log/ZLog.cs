using UnityEngine;
using System;
using System.Diagnostics;

namespace Zby
{
    class MyLogHandler : ILogHandler
    {
        private ILogHandler m_DefaultLogHandler = UnityEngine.Debug.unityLogger.logHandler;

        public MyLogHandler()
        {
            UnityEngine.Debug.unityLogger.logHandler = this;
        }

        /*
         * 添加时间戳
         */
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            string timeStr = string.Format("[{0}] ", DateTime.Now.ToString("yyyy-M-d HH:mm:ss.fff"));
            m_DefaultLogHandler.LogFormat(logType, context, timeStr + format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            m_DefaultLogHandler.LogException(exception, context);
        }
    }

    public static class ZLog
    {
        static MyLogHandler s_logHandler = new MyLogHandler();

        /*
         * 菜单 Edit->Project Setting->Player->Other Settins:Scriping Define Symbols
         * 如果宏没OUTPUT_D设置，则所有 ZLog.D 都不会被编译调用
         */
        [Conditional("OUTPUT_D")]
        public static void D(UnityEngine.Object context, string format, params object[] arg)
        {
            UnityEngine.Debug.LogFormat(context, format, arg);
        }

        public static void I(UnityEngine.Object context, string format, params object[] arg)
        {
            UnityEngine.Debug.LogFormat(context, format, arg);
        }

        public static void W(UnityEngine.Object context, string format, params object[] arg)
        {
            UnityEngine.Debug.LogWarningFormat(context, format, arg);
        }

        public static void E(UnityEngine.Object context, string format, params object[] arg)
        {
            UnityEngine.Debug.LogErrorFormat(context, format, arg);
        }
    }

}