using System;
using System.Windows;
using System.Windows.Input;

namespace USBMissileLauncher
{
    public partial class MainWindow
    {
        private readonly MissileController _missileController = new MissileController(0x0A81, 0x0701);
        public MainWindow()
        {
            InitializeComponent();
            if (_missileController.Exist) return;
            MessageBox.Show("Device is not connected or VID/PID not match 0x0A81/0x0701");
            Environment.Exit(-1);
        }


        private void FireButton_Click(object sender, RoutedEventArgs e)
        {
            _missileController.Fire();
        }

       

        private void UpButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _missileController.Up();
        }

        private void LeftButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _missileController.Left();
        }

        private void RightButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _missileController.Right();
        }

        private void DownButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _missileController.Down();
        }

        private void All_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _missileController.Stop();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _missileController.Up();
                    break;
                case Key.Down:
                    _missileController.Down();
                    break;
                case Key.Left:
                    _missileController.Left();
                    break;
                case Key.Right:
                    _missileController.Right();
                    break;
                case Key.Space:
                    _missileController.Fire();
                    break;
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) return;
            _missileController.Stop();
        }

        protected override void OnClosed(EventArgs e)
        {
            _missileController.Stop();
        }
    }
}
