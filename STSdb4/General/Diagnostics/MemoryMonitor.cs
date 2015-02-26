using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STSdb4.General.Diagnostics
{
    public class MemoryMonitor
    {
        private Process process;
        private Task worker;
        private bool shutDown;

        private long peakPagedMemorySize64;
        private long peakWorkingSet64;
        private long peakVirtualMemorySize64;

        public bool MonitorPagedMemorySize64;
        public bool MonitorWorkingSet64;
        public bool MonitorVirtualMemorySize64;
        public int MonitorPeriodInMilliseconds;

        public MemoryMonitor(bool monitorPagedMemorySize64, bool monitorWorkingSet64, bool monitorVirtualMemorySize64, int monitorPeriodInMilliseconds = 500)
        {
            if (!monitorPagedMemorySize64 && !monitorWorkingSet64 && !monitorVirtualMemorySize64)
                throw new ArgumentException("At least one flag has to be true.");

            process = Process.GetCurrentProcess();

            MonitorPagedMemorySize64 = monitorPagedMemorySize64;
            MonitorWorkingSet64 = monitorWorkingSet64;
            MonitorVirtualMemorySize64 = monitorVirtualMemorySize64;
            MonitorPeriodInMilliseconds = monitorPeriodInMilliseconds;
        }

        public MemoryMonitor(int monitorPeriodInMilliseconds = 500)
            : this(true, true, true, monitorPeriodInMilliseconds)
        {
        }

        ~MemoryMonitor()
        {
            Stop();
        }

        private void DoUpate()
        {
            process.Refresh();

            if (MonitorPagedMemorySize64)
            {
                var pagedMemorySize64 = process.PagedMemorySize64;
                if (pagedMemorySize64 > PeakPagedMemorySize64)
                    PeakPagedMemorySize64 = pagedMemorySize64;
            }

            if (MonitorWorkingSet64)
            {
                var workingSet64 = process.WorkingSet64;
                if (workingSet64 > PeakWorkingSet64)
                    PeakWorkingSet64 = workingSet64;
            }

            if (MonitorVirtualMemorySize64)
            {
                var virtualMemorySize64 = process.VirtualMemorySize64;
                if (virtualMemorySize64 > PeakVirtualMemorySize64)
                    PeakVirtualMemorySize64 = virtualMemorySize64;
            }
        }

        private void DoMonitor()
        {
            while (!shutDown)
            {
                DoUpate();

                SpinWait.SpinUntil(() => shutDown, MonitorPeriodInMilliseconds);
            }
        }

        public void Start()
        {
            if (worker != null)
                Stop();

            DoUpate();

            worker = Task.Factory.StartNew(DoMonitor, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            if (worker == null)
                return;

            try
            {
                shutDown = true;
                worker.Wait();
            }
            finally
            {
                shutDown = false;
                worker = null;
            }
        }

        public void Reset()
        {
            PeakPagedMemorySize64 = 0;
            PeakWorkingSet64 = 0;
            PeakVirtualMemorySize64 = 0;
        }

        public long PeakPagedMemorySize64
        {
            get { return Interlocked.Read(ref peakPagedMemorySize64); }
            private set { Interlocked.Exchange(ref peakPagedMemorySize64, value); }
        }

        public long PeakWorkingSet64
        {
            get { return Interlocked.Read(ref peakWorkingSet64); }
            private set { Interlocked.Exchange(ref peakWorkingSet64, value); }
        }

        public long PeakVirtualMemorySize64
        {
            get { return Interlocked.Read(ref peakVirtualMemorySize64); }
            private set { Interlocked.Exchange(ref peakVirtualMemorySize64, value); }
        }
    }
}
