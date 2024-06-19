using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class Win32Help {

    private delegate bool Wndenumproc(IntPtr hwnd, uint IParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EnumWindows(Wndenumproc lpEnumFunc, uint lParam);//遍历顶层窗口，将窗口句柄传递给应用程序定义的回调函数

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetParent(IntPtr hWnd);//获得子窗口的父窗口句柄

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

    [DllImport("kernel32.dll")]
    private static extern void SetLastError(uint dwErrCode);


    public static IntPtr GetProcessWnd()
    {
        var ptrWnd = IntPtr.Zero;
        var pid = (uint)Process.GetCurrentProcess().Id;
        var bResult = EnumWindows(delegate(IntPtr hwnd, uint lParam)
        {
            uint id = 0;
            if (GetParent(hwnd) != IntPtr.Zero)
                return true;
            GetWindowThreadProcessId(hwnd, ref id);
            if (id != lParam)
                return true;
            ptrWnd = hwnd;
            SetLastError(0);
            return false;//返回false终止窗口
        }, pid);
        return (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWnd : IntPtr.Zero;
    }

    [DllImport("imm32.dll")]
    private static extern IntPtr ImmGetContext(IntPtr hwnd);//获得输入法窗口的句柄
    [DllImport("imm32.dll")]
    private static extern bool ImmGetOpenStatus(IntPtr himc);
    [DllImport("imm32.dll")]
    private static extern bool ImmSetOpenStatus(IntPtr himc, bool b);

    //使用系统输入法
    public static void SetImeEnable(bool tf)
    {
        var handle = GetProcessWnd();
        var hIme = ImmGetContext(handle);
        ImmSetOpenStatus(hIme, tf);
    }

    public bool GetImeStatus()
    {
        var handle = GetProcessWnd();
        var hIme = ImmGetContext(handle);
        return ImmGetOpenStatus(hIme);
    }

}
