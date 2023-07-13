﻿using CalibrationProvider;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Point = VisionGuided.Point;
using RelayCommand = X_Guide.MVVM.Command.RelayCommand;

namespace X_Guide.MVVM.ViewModel
{
    [SupportedOSPlatform("windows")]
    internal class NinePointCalibrationViewModel : ViewModelBase, IRecipient<NinePointData>
    {
        private readonly ICalibrationService _calibrationService;
        private readonly IMessenger _messenger;

        public ObservableCollection<bool> NinePointState { get; set; } = new ObservableCollection<bool>(new bool[9]);
        private SemaphoreSlim _semaphore;

        public bool CanNext { get; set; } = true;
        public RelayCommand NextCommand { get; set; }

        public NinePointCalibrationViewModel(ICalibrationService calibrationService, IMessenger messenger)
        {
            _calibrationService = calibrationService;
            _messenger = messenger;
            _messenger.Register(this);
            NextCommand = new RelayCommand(Continue9Point, (o) => CanNext);
        }

        public async Task<Point[]> LookingDownward9PointVision()
        {
            return await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Vision);
        }

        public async Task<Point[]> LookingDownward9PointManipulator()
        {
            return await _calibrationService.LookingDownward9Point(BlockingCall, Provider.Manipulator);
        }

        private void Continue9Point(object arg)
        {
            try
            {
                _semaphore.Release();
            }
            catch
            {
            }
        }

        public void Receive(NinePointData message)
        {
            Task<Point[]> task = default;
            switch (message.Type)
            {
                case DataType.Vision: task = _calibrationService.LookingDownward9PointVision(BlockingCall); break;
                case DataType.Manipulator: task = _calibrationService.LookingDownward9PointManipulator(BlockingCall); break;
            }
            message.Reply(task);
        }

        private async Task BlockingCall(int arg)
        {
            CanNext = true;
            if (arg != 0) NinePointState[arg - 1] = true;
            NextCommand.OnCanExecuteChanged();

            using (_semaphore = new SemaphoreSlim(0))
            {
                await _semaphore.WaitAsync();
            }

            CanNext = false;
            NextCommand.OnCanExecuteChanged();
        }

        public override void Dispose()
        {
            _messenger.UnregisterAll(this);
            base.Dispose();
        }
    }

    public enum DataType
    {
        Manipulator,
        Vision
    }

    public class NinePointData : AsyncRequestMessage<Point[]>
    {
        public DataType Type { get; set; }

        public NinePointData(DataType type = default)
        {
            Type = type;
        }
    }
}