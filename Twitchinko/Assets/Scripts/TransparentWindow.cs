using System;
using System.Runtime.InteropServices;
using UnityEngine;
using TwitchIntegration;

public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint cFlags);

    public struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    public static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);


    void Start()
    {
        Debug.Log("FUCK");
        bool isTwitchAuthenticated = TwitchManager.IsAuthenticated;
        Debug.Log("Shit");

        //MessageBox(new IntPtr(0), "Hello World", "Hello Dialog!", 0);

        //Makes the app transparent and click through in the build (if made to run in the editor, it will break the whole Unity Editor)
#if !UNITY_EDITOR

        //Allow player to change settings when not yet authenticated to Twitch.
        //if(isTwitchAuthenticated)
        {
            IntPtr hWnd = GetActiveWindow();

            //Allows transparency using the negative margin value.
            MARGINS margins = new MARGINS { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hWnd, ref margins);

            //Allows the window to be click through and transparent
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);

            //Sets the window to be on the topmost.
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
        }
#endif

    }

}
