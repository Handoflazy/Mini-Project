using System.Collections.Generic;
using UnityEngine;
using VTabs.Libs;

namespace ImprovedTimers
{
    public static class TimerManager
    {
        static readonly List<Timer> timers = new();
        static readonly List<Timer> sweep = new();
        public static void RegisterTimer(Timer timer) => timers.Add(timer);
        public static void DeregisterTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateTimer()
        {
            if (timers.Count == 0) return;
            sweep.RefreshWith(timers);
            foreach (var timer in sweep)
            {
                timer.Tick();
            }
        }

        public static void Clear()
        {
            sweep.RefreshWith(timers);
            foreach (var timer in sweep) {
                timer.Dispose();
            }
            
            timers.Clear();
            sweep.Clear();
        }
    }
    public static class ListExtensions 
    {
        public static void RefreshWith<T>(this List<T> list, List<T> other)
        {
            list.Clear();
            list.AddRange(other);
        }
    }
}