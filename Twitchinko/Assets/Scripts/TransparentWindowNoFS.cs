using System;
using System.Runtime.InteropServices;
using UnityEngine;
using TwitchIntegration;
using UnityEngine.XR;

public class TransparentWindowNoFS : MonoBehaviour
{
    //Variables for turning window transparent.

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

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

    const int GWL_STYLE = -16; //for windowed mode.
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);


    //Variables for removing transparency
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOACTIVATE = 0x0010;

    static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    //gets the default style of the window so it can be reverted back when needed.
    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_FRAMECHANGED = 0x0020;
    private int originalStyle = 0;
    private bool styleSaved = false;

    private bool isTransparent = false;
    

    void Start()
    {
        

//        //Debug.Log("FUCK");
//        bool isTwitchAuthenticated = TwitchManager.IsAuthenticated;
//        //Debug.Log("Shit");

//        //MessageBox(new IntPtr(0), "Hello World", "Hello Dialog!", 0);

//        //Makes the app transparent and click through in the build (if made to run in the editor, it will break the whole Unity Editor)
//#if !UNITY_EDITOR

//        //Allow player to change settings when not yet authenticated to Twitch.
//        //if(isTwitchAuthenticated)
//        {
//            IntPtr hWnd = GetActiveWindow();

//            //Allows transparency using the negative margin value.
//            MARGINS margins = new MARGINS { cxLeftWidth = -1 };
//            DwmExtendFrameIntoClientArea(hWnd, ref margins);

//            //Allows the window to be click through and transparent
//            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);

//            //Sets the window to be on the topmost.
//            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
//        }
//#endif

    }

    public void TurnScreenTransparent(bool transparentModeOn)
    {
        Debug.Log($"Turn screen transparent {transparentModeOn}");

#if !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();

        //saves the current window style to allow reverting back to it. 
        if (!styleSaved)
        {
            originalStyle = GetWindowLong(hWnd, GWL_STYLE);
            styleSaved = true;
        }

        if (transparentModeOn && !isTransparent)
        {
            //Allows transparency using the negative margin value.
            MARGINS margins = new MARGINS { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hWnd, ref margins);

            //Allows the window to be click through and transparent
            //SetWindowLong(hWnd, GWL_EXSTYLE, (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT));
            SetWindowLong(hWnd, GWL_STYLE, unchecked((int)(WS_POPUP | WS_VISIBLE)));


            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            exStyle |= (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT);
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            //Sets the window to be on the topmost.
            //SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);

            isTransparent = true;
        }
        else if(!transparentModeOn && isTransparent)
        {
            if (styleSaved)
            {
                SetWindowLong(hWnd, GWL_STYLE, originalStyle);

                // Remove WS_EX_LAYERED and WS_EX_TRANSPARENT
                int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                exStyle &= ~(int)(WS_EX_LAYERED | WS_EX_TRANSPARENT);
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

                //forces Windows to redraw in case it doesn't work properly. 
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
                    SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

            }
            else
            {
                // Revert to normal window
                MARGINS margins = new MARGINS { cxLeftWidth = 0, cxRightWidth = 0, cyTopHeight = 0, cyBottomHeight = 0 };
                DwmExtendFrameIntoClientArea(hWnd, ref margins);

                // Remove WS_EX_LAYERED and WS_EX_TRANSPARENT
                int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                exStyle &= ~(int)(WS_EX_LAYERED | WS_EX_TRANSPARENT);
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

                // Optional: remove topmost
                //SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }


            isTransparent = false;

        }
#endif

    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IntPtr hWnd = GetActiveWindow();

            //saves the current window style to allow reverting back to it. 
            if (!styleSaved)
            {
                originalStyle = GetWindowLong(hWnd, GWL_STYLE);
                styleSaved = true;
            }

            if (!isTransparent)
            {
                //Allows transparency using the negative margin value.
                MARGINS margins = new MARGINS { cxLeftWidth = -1 };
                DwmExtendFrameIntoClientArea(hWnd, ref margins);

                //Allows the window to be click through and transparent
                //SetWindowLong(hWnd, GWL_EXSTYLE, (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT));
                SetWindowLong(hWnd, GWL_STYLE, unchecked((int)(WS_POPUP | WS_VISIBLE)));


                int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                exStyle |= (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT);
                SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

                //Sets the window to be on the topmost.
                //SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);

                isTransparent = true;
            }
            else
            {
                if (styleSaved)
                {
                    SetWindowLong(hWnd, GWL_STYLE, originalStyle);

                    // Remove WS_EX_LAYERED and WS_EX_TRANSPARENT
                    int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                    exStyle &= ~(int)(WS_EX_LAYERED | WS_EX_TRANSPARENT);
                    SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

                    //forces Windows to redraw in case it doesn't work properly. 
                    SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
                     SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

                }
                else
                {
                    // Revert to normal window
                    MARGINS margins = new MARGINS { cxLeftWidth = 0, cxRightWidth = 0, cyTopHeight = 0, cyBottomHeight = 0 };
                    DwmExtendFrameIntoClientArea(hWnd, ref margins);

                    // Remove WS_EX_LAYERED and WS_EX_TRANSPARENT
                    int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
                    exStyle &= ~(int)(WS_EX_LAYERED | WS_EX_TRANSPARENT);
                    SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

                    // Optional: remove topmost
                    //SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                }


                isTransparent = false;

            }

        }
#endif
    }


}
