using System;

namespace LTF.Timers
{
    public interface ITimer
    {
        public Action TimeEvent { get; set; }

        public float TimeToDo { get; }
        public float ElapsedTime { get; }
        public bool CanTick { get; }

        public float T => ElapsedTime / TimeToDo;

        public void Tick();

        /// <summary>
        /// Set how fast elasedtime gets ticked 
        /// </summary>
        public void SetDeltaMultiplier(float multipler);

        /// <summary>
        /// Change Time to do Event
        /// </summary>
        public void ChangeTime(float time);

        /// <summary>
        /// Set Time to do Event
        /// </summary>
        public void SetTime(float time);

        /// <summary>
        /// Stops ticking
        /// </summary>
        public void Stop();

        /// <summary>
        /// Starts ticking
        /// </summary>
        public void Continue();

        //not unity reset zz
        /// <summary>
        /// Resets elapsed time to 0
        /// </summary>
        public void Reset_();

        /// <summary>
        /// Stops ticking and Resets elapsed time to 0
        /// </summary>
        public void StopAndReset()
        {
            Stop();
            Reset_();
        }

        /// <summary>
        /// Resets elapsed time to 0 and starts ticking 
        /// </summary>
        public void Restart()
        {
            Reset_();
            Continue();
        }
    }
 }