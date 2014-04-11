using System;
using System.Diagnostics;               
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace BossMode {

    public class WindowControl {

        static List<IntPtr> collection = new List<IntPtr>();

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        //-------------------------------------------------------------------------------------------------

        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);        

        //-------------------------------------------------------------------------------------------------

        EnumDelegate filter = delegate(IntPtr hWnd, int lParam) {

            Process[] processes = System.Diagnostics.Process.GetProcessesByName("pidgin");
            uint processId = 0;
            uint threadid = GetWindowThreadProcessId(hWnd, out processId);

            if (processes == null) {
                return false;
            }

            if (processes[0].Id == processId) {
                if (IsWindowVisible(hWnd)) {
                    collection.Add(hWnd);
                }
            }

            return true;
        };

        //-------------------------------------------------------------------------------------------------

        public void ToggleWindows() {
            if (collection.Count > 0) {
                ShowWindows();
            } else {
                HideWindows();
            }
        }

        //-------------------------------------------------------------------------------------------------

        public void HideWindows() {
            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero)) {
                foreach (var item in collection) {
                    ShowWindow((int)item, SW_HIDE);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------

        public void ShowWindows() {
            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero)) {
                foreach (var item in collection) {
                    ShowWindow((int)item, SW_SHOW);
                }
            }
            collection.Clear();
        }

        //-------------------------------------------------------------------------------------------------
    }
}
