using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SkillMeter.ViewModels
{
   public class TimerViewModel :ViewModelBase
    {
        readonly ITimerModel _timer = new TimerModel();
        private DataAccessLayer.DataAccessADO dataObj;
        public readonly TimeSpan maxDuration;

       public TimerViewModel()
        {
            dataObj = new DataAccessLayer.DataAccessADO();
            int timerValue = Convert.ToInt32(dataObj.getUserTestsData().Where(r=>r.UserName==this.CurrentSession.UserName && r.Attempted==0).Select(r=>r.TestDuration).FirstOrDefault());
            maxDuration = new TimeSpan(0,timerValue,0);
            AddEventHandlers();
           
        }

       /// <summary>
       /// Add the event handlers.
       /// </summary>
       private void AddEventHandlers()
       {
           _timer.Tick += (sender, e) => OnTick(sender, e);
           _timer.Completed += (sender, e) => OnCompleted(sender, e);
           _timer.Started += (sender, e) => OnStarted(sender, e);
           _timer.Stopped += (sender, e) => OnStopped(sender, e);
           _timer.TimerReset += (sender, e) => OnReset(sender, e);
       }

       public void Start()
       {

           _timer.Start();
       }

       #region Event handlers

       /// <summary>
       /// Update the timer view model properties based on the time span passed in.
       /// </summary>
       /// <param name="t"></param>
       private void UpdateTimer(TimerModelEventArgs e)
       {
           UpdateTimerValues();

       }

       private void UpdateTimerValues()
       {
           TimeSpan t = _timer.Remaining;
           TimerValue = string.Format("{0}:{1}:{2}", t.Hours.ToString("D2"),
               t.Minutes.ToString("D2"), t.Seconds.ToString("D2"));

       }
       /// <summary>
       /// Fires where the timer completes.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void OnCompleted(object sender, TimerModelEventArgs e)
       {
           UpdateTimer(e);
           this.CurrentSession.VMBInstance = "TimeUpViewModel";
           //CompletedCount++;

       }

       /// <summary>
       /// Fires when the timer ticks.  Ticks out to be of the order of 
       /// tenths of a second or so to prevent excessive spamming of this method.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void OnTick(object sender, TimerModelEventArgs e)
       {
           UpdateTimer(e);

       }

       /// <summary>
       /// Fires when the timer starts.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       void OnStarted(object sender, TimerModelEventArgs e)
       {
           UpdateTimer(e);


       }

       /// <summary>
       /// Fires when the timer stops.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void OnStopped(object sender, TimerModelEventArgs e)
       {
           UpdateTimer(e);
        
       }

       /// <summary>
       /// Fires when the timer resets.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       private void OnReset(object sender, TimerModelEventArgs e)
       {
           var timer = sender as TimerModel;
           UpdateTimer(e);
          

       }

       void t_Elapsed(object sender, ElapsedEventArgs e)
       {
           MessageBox.Show("Time Up");
       }
       #endregion

       private string timerValue;
       public string TimerValue 
       {
           get
           {
               return timerValue;
           }
           set
           {
               timerValue = value;
               this.CurrentSession.TimeStampValue = timerValue;
               OnPropertyChanged(()=>this.TimerValue
                   );
           }
       }

       /// <summary>
       /// The timer duration.
       /// </summary>
       public TimeSpan Duration
       {
           get
           {
               return _timer.Duration;
           }

           set
           {
               if (_timer.Duration == value)
                   return;
               _timer.Duration = value;
               OnPropertyChanged("Duration");
           }
       }
    }
}
