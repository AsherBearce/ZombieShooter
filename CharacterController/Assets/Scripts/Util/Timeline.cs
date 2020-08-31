using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    public class Timeline
    {
        public Tuple<float, Action>[] keys;
        public float startTime = 0;
        private int currentKey = 0;
        public float finishTime = 0;
        public bool loop = false;
        public bool isPlaying = false;

        public void play(float time)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                startTime = time;
            }
        }

        public void stop()
        {
            isPlaying = false;
            currentKey = 0;
        }

        public void updateTime(float time)
        {
            if (isPlaying)
            {
                float currentTime = time - startTime;

                if (currentTime >= finishTime)
                {

                    stop();
                    if (loop)
                    {
                        play(time);
                    }
                }

                Tuple<float, Action> lastKey = keys[currentKey];

                if (currentTime - lastKey.Item1 >= 0)
                {
                    lastKey.Item2.Invoke();
                    currentKey++;
                }
            }
        }

        public Timeline(bool loops, Tuple<float, Action>[] keys)
        {
            this.keys = keys;
            this.loop = loops;
        }
    }
}
