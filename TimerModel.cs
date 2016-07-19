using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SkillMeter.ViewModels
{
    /// <summary>
    /// Trivial enum to describe the timer state.
    /// </summary>
    public enum TimerState
    {
        Running,
        Paused,
        Complete
    }

    internal class TimerModel : ITimerModel
    {


        #region Fields

        /// <summary>
        /// The underlying timer
        /// </summary>
        readonly DispatcherTimer timer = new DispatcherTimer();
        

        #endregion

        #region Constructors
       

        /// <summary>
        /// Create a timer of the specified duration.
        /// </summary>
        /// <param name="duration"></param>
        public TimerModel(TimeSpan duration)
        {
            //  use a coarse grained interval, as this timer isn't meant
            //  to be completely accurate.
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += (sender, e) => OnDispatcherTimerTick();

           
            Duration = duration;
            Reset();

        }

        public TimerModel(): this(new TimeSpan())
        {
            // TODO: Complete member initialization
        }
        #endregion

        #region Properties

        private TimeSpan maxDuration;

        public TimeSpan MaxDuration
        {
            get { return new TimeSpan(0,2,0); }
           
        }
        

        /// <summary>
        /// Return the timer interval that we're using.
        /// </summary>
        public TimeSpan Interval { get { return timer.Interval; } }

        /// <summary>
        /// Return the time remaining.
        /// </summary>
        public TimeSpan Remaining { get; private set; }

        /// <summary>
        /// Has the time completed?
        /// </summary>
        public bool Complete
        {
            get
            {
                return Remaining <= TimeSpan.Zero;
            }
        }

        private TimeSpan duration;

        /// <summary>
        /// Set or get the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (duration != value)
                {
                    duration = value;
                    Reset();
                }
            }
        }

        public TimerState Status { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Start the countdown.
        /// </summary>
        public void Start()
        {
            timer.Start();
            OnStarted();
        }

        /// <summary>
        /// Stop the countdown.
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            OnStopped();
        }

        /// <summary>
        /// Stop the current countdown and reset.
        /// </summary>
        public void Reset()
        {
            Stop();
            Remaining = Duration;
            OnReset();
        }


        #region Event Handlers
        /// <summary>
        /// Handle the ticking of the system timer.
        /// </summary>
        private void OnDispatcherTimerTick()
        {
            Remaining = Remaining - Interval;
            OnTick();
            if (Complete)
            {
                Stop();
                Remaining = TimeSpan.Zero;
                OnCompleted();
            }
        }
        #endregion
        #endregion

        #region Events

        public event EventHandler<TimerModelEventArgs> Tick;
        public event EventHandler<TimerModelEventArgs> Started;
        public event EventHandler<TimerModelEventArgs> Stopped;
        public event EventHandler<TimerModelEventArgs> TimerReset;
        public event EventHandler<TimerModelEventArgs> Completed;


        #region OnReset
        /// <summary>
        /// Triggers the TimerReset event
        /// </summary>
        private void OnReset()
        {
            Status = TimerState.Paused;

            if (TimerReset != null)
            {
                TimerReset(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Reset));
            }
        }
        #endregion

        #region OnCompleted
        /// <summary>
        /// Triggers the Completed event.
        /// </summary>
        private void OnCompleted()
        {
            Status = TimerState.Complete;

            if (Completed != null)
            {
                Completed(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Completed));
            }
        }
        #endregion

        #region OnStopped
        /// <summary>
        /// Triggers the Stopped event.
        /// </summary>
        private void OnStopped()
        {
            Status = TimerState.Paused;

            if (Stopped != null)
            {
                Stopped(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Stopped));
            }
        }
        #endregion

        #region OnStarted
        /// <summary>
        /// Triggers the Started event.
        /// </summary>
        private void OnStarted()
        {
            Status = TimerState.Running;

            if (Started != null)
            {
                Started(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Started));
            }
        }
        #endregion

        #region OnTick
        /// <summary>
        /// Triggers the Tick event.
        /// </summary>
        private void OnTick()
        {
            Status = TimerState.Running;

            if (Tick != null)
            {
                Tick(this, new TimerModelEventArgs(Duration, Remaining, TimerModelEventArgs.Status.Running));
            }
        }
        #endregion

        #endregion
    }


    /// <summary>
    /// Simple EventArgs class for the timer model.
    /// </summary>
    /// 
    public class TimerModelEventArgs : EventArgs
    {
        public enum Status
        {
            NotSpecified,
            Stopped,
            Started,
            Running,
            Completed,
            Reset
        }

        private TimeSpan duration;
        private TimeSpan remaining;
        private Status state = Status.NotSpecified;

        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (duration == value)
                    return;
                duration = value;
            }
        }

        public TimeSpan Remaining
        {
            get
            {
                return remaining;
            }
            set
            {
                if (remaining == value)
                    return;
                remaining = value;
            }
        }

        public Status State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public TimerModelEventArgs(TimeSpan duration, TimeSpan remaining, Status state)
        {
            Duration = duration;
            Remaining = remaining;
            State = state;
        }

    }
}
