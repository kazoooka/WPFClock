using WpfClock.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfClock.ViewModels
{

    public enum Status
    {
        Start,
        Stop
    }
    
    public class MainViewModel : NotifyBase
    {
        #region Fields
        #endregion

        #region Properties
        private DispatcherTimer timer;
        private bool _clockStarted;
        private string _currentTime, _changerTime;
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                RaisePropertyChanged(() => CurrentTime);
            }
        }
        public string ChangerTime
        {
            get { return _changerTime; }
            set
            {
                _changerTime = value;
                RaisePropertyChanged(() => ChangerTime);
                CommandManager.InvalidateRequerySuggested();
            }
        }
        public bool ClockStarted
        {
            get { return _clockStarted; }
            set
            {
                _clockStarted = value;
                RaisePropertyChanged(() => ClockStarted);
            }
        }
        #endregion

        #region Commands
        private RelayCommand _testCommand, _startstopCommand;
        public ICommand TestCommand
        {
            get
            {
                if (_testCommand != null) return _testCommand;
                _testCommand = new RelayCommand(TestExecute, CanTest);
                return _testCommand;
            }
        }
        public ICommand StartStopCommand
        {
            get
            {
                if (_startstopCommand != null) return _startstopCommand;
                _startstopCommand = new RelayCommand(StartStopExecute, CanStartStop);
                return _startstopCommand;
            }
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            Title = "Project Title";
            CurrentTime = "00:00";
            ClockStarted = false;

            timer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(1),
                IsEnabled = ClockStarted
            };
            timer.Tick+=(s,e) => {
                UpdateTime();
            };
        }
        #endregion

        #region Methods
        private bool CanStartStop(object obj)
        {
            return true;
        }

        private void StartStopExecute(object obj)
        {
          if(ClockStarted){
              timer.IsEnabled = false;
              ClockStarted = false;
          }else{
              timer.IsEnabled = true;
              ClockStarted = true;
              
          }
            
        }

        private bool CanTest(object obj)
        {
          
            
            if (string.IsNullOrEmpty(ChangerTime))
                return false;


            var sp = ChangerTime.Split(':');

            return sp.Length > 0;
        }

        private void TestExecute(object obj)
        {
            CurrentTime = ChangerTime;
        }
        #endregion
        
        private void UpdateTime()
        {
            var cct = ToSeconds();

            cct += 1;

            CurrentTime = ToReadableString(cct);
        }

        private int ToSeconds()
        {
            var sp = CurrentTime.Split(':');
            if(sp.Length <= 0)
                return 0;

            int h, s;
            var hc = int.TryParse(sp[0], out h);
            var sc = int.TryParse(sp[1], out s);

            if(!hc&&!sc)
                return 0;

            var hs = h * 60;
            return hs + s;
        }

        private string ToReadableString(int seconds) 
        {
            var rh = seconds / 60;
            var rs = seconds % 60;

            var mc = rh.ToString().Length>1;
            var cc = rs.ToString().Length>1;
            var m = rh.ToString();
            var s = rs.ToString();

            if(!mc)
                m="0"+rh;

            if(!cc)
                s="0"+rs;


            return string.Format("{0}:{1}",m,s);
        }
    }
}
