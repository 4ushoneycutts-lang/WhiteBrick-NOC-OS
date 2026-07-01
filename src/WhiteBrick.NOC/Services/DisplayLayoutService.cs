using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia;

namespace WhiteBrick.NOC.Services
{
    public class MonitorLayout
    {
        public int Index { get; set; }
        public Rect Bounds { get; set; }
        public Rect WorkArea { get; set; }
        public double ScaleFactor { get; set; }
    }

    public static class DisplayLayoutService
    {
        public static IReadOnlyList<MonitorLayout> GetMonitors()
        {
            var monitors = new List<MonitorLayout>();
            int index = 0;

            EnumMonitors((hMonitor, rcMonitor, rcWork) =>
            {
                var bounds = new Rect(rcMonitor.left, rcMonitor.top, rcMonitor.right - rcMonitor.left, rcMonitor.bottom - rcMonitor.top);
                var work = new Rect(rcWork.left, rcWork.top, rcWork.right - rcWork.left, rcWork.bottom - rcWork.top);
                double scale = 1.0;
                try
                {
                    if (GetDpiForMonitor(hMonitor, DpiType.Effective, out uint dpiX, out uint dpiY) == 0)
                    {
                        scale = dpiX / 96.0;
                    }
                }
                catch { }

                monitors.Add(new MonitorLayout
                {
                    Index = index++,
                    Bounds = bounds,
                    WorkArea = work,
                    ScaleFactor = scale
                });
            });

            return monitors;
        }

        private delegate void MonitorCallback(IntPtr hMonitor, RECT rcMonitor, RECT rcWork);

        private static void EnumMonitors(MonitorCallback callback)
        {
            bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
            {
                var mi = new MONITORINFOEX();
                mi.cbSize = Marshal.SizeOf<MONITORINFOEX>();
                if (GetMonitorInfo(hMonitor, ref mi))
                {
                    callback(hMonitor, mi.rcMonitor, mi.rcWork);
                }
                return true;
            }

            MonitorEnumProc proc = MonitorEnum;
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, proc, IntPtr.Zero);
        }

        #region Win32
        private delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int left, top, right, bottom; }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITORINFOEX
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szDevice;
        }

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        private enum DpiType { Effective = 0, Angular = 1, Raw = 2 }

        [DllImport("Shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, DpiType dpiType, out uint dpiX, out uint dpiY);
        #endregion
    }
}
