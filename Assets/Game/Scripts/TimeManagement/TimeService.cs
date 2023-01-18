﻿using UnityEngine;
using VContainer;

namespace Game.TimeManagement
{
    public class TimeService
    {
        [Inject]
        public TimeService()
        {
        }

        public void Pause() => Time.timeScale = 0;
        public void Play() => Time.timeScale = 1;
    }
}