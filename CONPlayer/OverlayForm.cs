using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public sealed class OverlayForm : Form
{
    // Win32 constants
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    private const int WM_NCHITTEST = 0x0084;
    private const int HTTRANSPARENT = -1;
    private const int WM_RBUTTONUP = 0x0205;
    private const int WM_CONTEXTMENU = 0x007B;
    private const int HTCLIENT = 1;
    private const int VK_RBUTTON = 0x02;

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    private const int ULW_ALPHA = 0x00000002;
    private const byte AC_SRC_OVER = 0x00;
    private const byte AC_SRC_ALPHA = 0x01;

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int x, y; public POINT(int x, int y) { this.x = x; this.y = y; } }

    [StructLayout(LayoutKind.Sequential)]
    private struct SIZE { public int cx, cy; public SIZE(int cx, int cy) { this.cx = cx; this.cy = cy; } }

    [StructLayout(LayoutKind.Sequential)]
    private struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
        ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc,
        int crKey, ref BLENDFUNCTION pblend, int dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern bool DeleteObject(IntPtr hObject);

    // IMPORTANT: do NOT use TransparencyKey or BackColor tricks
    public OverlayForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;

        // Avoid flicker / background erase
        SetStyle(ControlStyles.Opaque, true);
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
            return cp;
        }
    }

    public ContextMenuStrip HostMenu { get; set; }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_NCHITTEST)
        {
            bool rightDown = (GetAsyncKeyState(VK_RBUTTON) & 0x8000) != 0;

            // Only "catch" right-clicks; pass everything else through
            m.Result = (IntPtr)(rightDown ? HTCLIENT : HTTRANSPARENT);
            return;
        }

        base.WndProc(ref m);
    }

    public Action OnOverlayRightClick; // set this from main form

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);

        if (e.Button == MouseButtons.Right)
            OnOverlayRightClick?.Invoke();
    }

    private void ShowHostMenuAtCursor()
    {
        if (HostMenu == null) return;

        // Ensure menu shows even though overlay is NoActivate
        var pt = Cursor.Position;
        HostMenu.Show(pt);
    }

    protected override void OnPaint(PaintEventArgs e) { /* never used */ }
    protected override void OnPaintBackground(PaintEventArgs e) { /* never used */ }

    /// <summary>
    /// Push a 32bpp ARGB bitmap to the layered window. bmp must be Format32bppPArgb or Format32bppArgb.
    /// </summary>
    public void UpdateVisuals(Bitmap bmp)
    {
        if (bmp == null || IsDisposed) return;

        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => UpdateVisuals(bmp)));
            return;
        }

        // Ensure handle exists
        if (!IsHandleCreated) CreateControl();

        // Layered windows are happiest with premultiplied alpha
        Bitmap src = bmp;
        Bitmap converted = null;

        if (bmp.PixelFormat != PixelFormat.Format32bppPArgb)
        {
            converted = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppPArgb);
            using (var g = Graphics.FromImage(converted))
            {
                g.DrawImageUnscaled(bmp, 0, 0);
            }
            src = converted;
        }

        IntPtr screenDc = IntPtr.Zero;
        IntPtr memDc = IntPtr.Zero;
        IntPtr hBitmap = IntPtr.Zero;
        IntPtr oldBmp = IntPtr.Zero;

        try
        {
            screenDc = GetDC(IntPtr.Zero);
            memDc = CreateCompatibleDC(screenDc);

            // Get HBITMAP from managed bitmap
            hBitmap = src.GetHbitmap(Color.FromArgb(0)); // background ignored for 32bpp
            oldBmp = SelectObject(memDc, hBitmap);

            var topLeft = new POINT(Left, Top);
            var size = new SIZE(src.Width, src.Height);
            var srcPt = new POINT(0, 0);

            var blend = new BLENDFUNCTION
            {
                BlendOp = AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = 255, // overall opacity
                AlphaFormat = AC_SRC_ALPHA
            };

            bool ok = UpdateLayeredWindow(Handle, screenDc, ref topLeft, ref size,
                memDc, ref srcPt, 0, ref blend, ULW_ALPHA);

            if (!ok)
            {
                // Optional: debug the error
                // int err = Marshal.GetLastWin32Error();
            }
        }
        finally
        {
            if (oldBmp != IntPtr.Zero && memDc != IntPtr.Zero)
                SelectObject(memDc, oldBmp);

            if (hBitmap != IntPtr.Zero)
                DeleteObject(hBitmap);

            if (memDc != IntPtr.Zero)
                DeleteDC(memDc);

            if (screenDc != IntPtr.Zero)
                ReleaseDC(IntPtr.Zero, screenDc);

            converted?.Dispose();
        }
    }
}
