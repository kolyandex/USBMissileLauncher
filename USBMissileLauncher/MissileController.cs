using System;
using System.Windows.Threading;
using UsbLibrary;

namespace USBMissileLauncher
{
    internal class MissileController
    {
        private struct Command
        {
            public static readonly byte[] Stop = { 0, 0x20 };
            public static readonly byte[] Up = { 0, 0x02 };
            public static readonly byte[] Down = { 0, 0x01 };
            public static readonly byte[] Left = { 0, 0x04 };
            public static readonly byte[] Right = { 0, 0x08 };
            public static readonly byte[] Fire = { 0, 0x10 };
            public static readonly byte[] RequestStatus = { 0, 0x40 };
        }
        public struct TurretState
        {
            public const byte Up = 0x02;
            public const byte Down = 0x01;
            public const byte Left = 0x04;
            public const byte Right = 0x08;
            public const byte ShotComplete = 0x10;
        }

        private readonly SpecifiedDevice _missileLauncherDevice;
        private readonly DispatcherTimer _updateStateTimer = new DispatcherTimer();
        private byte _state;
        public bool Exist;
        /// <summary>
        /// Обычный конструктор
        /// </summary>
        /// <param name="vendorId">Vendor ID</param>
        /// <param name="productId">Product ID</param>
        public MissileController(int vendorId, int productId)
        {
            _updateStateTimer.Tick += UpdateStateTimerTick;
            _updateStateTimer.Interval = new TimeSpan(TimeSpan.TicksPerSecond / 8); 
            _updateStateTimer.IsEnabled = true;
            _updateStateTimer.Stop();
            _missileLauncherDevice = SpecifiedDevice.FindSpecifiedDevice(vendorId, productId);
            _missileLauncherDevice.DataRecieved += DataReceived;
            Exist = _missileLauncherDevice != null;
        }
        
        private void UpdateStateTimerTick(object sender, EventArgs e)
        {
            RequestStatus();
        }

        /// <summary>
        /// Запрашивает состояние турели в данный момент. Подробности в структуре TurretState
        /// </summary>
        /// <returns>Байт состояния</returns>
        public byte GetState()
        {
            RequestStatus();
            return _state;
        }

        private void DataReceived(object sender, DataRecievedEventArgs args)
        {
            if (args.data[1] == TurretState.ShotComplete)
            {
                //Тут произошел выстрел
                Stop();
                return;
            }
            _state = args.data[1];
        }

        private void RequestStatus()
        {
            _missileLauncherDevice.SendData(Command.RequestStatus);
        }

        public void Left()
        {
            _missileLauncherDevice.SendData(Command.Left);
            _updateStateTimer.Start();
        }
        public void Right()
        {
            _missileLauncherDevice.SendData(Command.Right);
            _updateStateTimer.Start();
        }
        public void Up()
        {
            _missileLauncherDevice.SendData(Command.Up);
            _updateStateTimer.Start();
        }
        public void Down()
        {
            _missileLauncherDevice.SendData(Command.Down);
            _updateStateTimer.Start();
        }
        public void Fire()
        {
            _missileLauncherDevice.SendData(Command.Fire);
            _updateStateTimer.Start();
        }
        /// <summary>
        /// Останавливает любое действие
        /// </summary>
        public void Stop()
        {
            _updateStateTimer.Stop();
            _missileLauncherDevice.SendData(Command.Stop);
        }
    }
}
