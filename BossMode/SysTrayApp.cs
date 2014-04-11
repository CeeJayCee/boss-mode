using System.Windows.Forms;
using System;
using System.Drawing;


namespace BossMode {
    public class SysTrayApp : Form {

        private NotifyIcon trayIcon;
        private ContextMenu contextMenu;

        private KeyboardHook hook = new KeyboardHook();
        private WindowControl window_control = new WindowControl();


        //-------------------------------------------------------------------------------------------------

        public SysTrayApp() {
            contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Exit", OnExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Boss Mode";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            trayIcon.ContextMenu = contextMenu;
            trayIcon.Visible = true;

            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(OnKeyPressed);
            hook.RegisterHotKey(BossMode.ModifierKeys.Control, Keys.F12);
        }

        //-------------------------------------------------------------------------------------------------

        protected override void OnLoad(EventArgs e) {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        //-------------------------------------------------------------------------------------------------

        private void OnExit(object sender, EventArgs e) {
            Application.Exit();
        }

        //-------------------------------------------------------------------------------------------------

        private void OnKeyPressed(object sender, EventArgs e) {
            window_control.ToggleWindows();
        }

        //-------------------------------------------------------------------------------------------------

        protected override void Dispose(bool isDisposing) {
            if (isDisposing) {
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        //-------------------------------------------------------------------------------------------------
    }
}
