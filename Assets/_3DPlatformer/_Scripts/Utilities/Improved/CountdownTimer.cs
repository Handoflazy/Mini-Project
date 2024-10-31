using UnityEngine;

namespace ImprovedTimers
{
    public class CountdownTimer : Timer
    {
        public CountdownTimer(float initialTime) : base(initialTime)
        {
        }

        public override void Tick()
        {
            if (IsRunning && CurrentTime > 0)
            {
                CurrentTime-=Time.deltaTime;
            }

            if (IsRunning && CurrentTime <= 0)
            {
                Stop();
            }
        }

        public override bool IsFinished => CurrentTime <= 0;
    }
}