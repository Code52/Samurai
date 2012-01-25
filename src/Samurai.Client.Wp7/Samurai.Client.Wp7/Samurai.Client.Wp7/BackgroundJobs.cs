using System;
using System.Collections.Generic;
using System.Threading;

namespace Samurai.Client.Wp7
{
    public class BackgroundJob
    {
        public bool IsComplete = false;
        public Action Job;

        public BackgroundJob(Action job)
        {
            Job = job;
            IsComplete = false;
        }
    }

    public class BackgroundJobs
    {
        private Thread _t;
        private bool _keepRunning = false;
        private object sync = new object();
        private readonly Queue<BackgroundJob> _jobs = new Queue<BackgroundJob>();

        private void PerformJobs()
        {
            bool running = _keepRunning;
            while (running)
            {
                if (_jobs.Count == 0)
                {
                    Thread.Sleep(17);
                }
                else
                {
                    var job = _jobs.Dequeue();
                    job.Job();
                    job.IsComplete = true;
                }
                lock (sync)
                    running = _keepRunning;
            }
        }

        public void SpinUp()
        {
            if (_t == null)
            {
                _t = new Thread(PerformJobs);
                _t.Name = "Loader Thread";
                _t.IsBackground = true;
                _keepRunning = true;
                _t.Start();
            }
        }

        public void StopThread()
        {
            if (_t != null)
            {
                lock (sync)
                    _keepRunning = false;
                _t = null;
            }
        }

        public BackgroundJob CreateJob(Action job)
        {
            if (job == null)
                return null;

            BackgroundJob j = new BackgroundJob(job);
            _jobs.Enqueue(j);
            return j;
        }
    }
}
