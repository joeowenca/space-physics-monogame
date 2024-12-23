using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SpacePhysics.Debugging
{
    internal class SystemUsage
    {
        private PerformanceCounter cpuCounter;
        private float lastCpuUsage;
        private DateTime lastCpuCheck;

        public SystemUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }
            else
            {
                throw new PlatformNotSupportedException("This functionality is only supported on Windows.");
            }
        }

        public float GetCpuUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if ((DateTime.Now - lastCpuCheck).TotalMilliseconds >= 1000)
                {
                    lastCpuUsage = cpuCounter.NextValue();
                    lastCpuCheck = DateTime.Now;
                }
                return (float)Math.Round(lastCpuUsage, 2);
            }
            else
            {
                throw new PlatformNotSupportedException("This functionality is only supported on Windows.");
            }
        }

        public long GetRamUsage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    return process.WorkingSet64;
                }
            }
            else
            {
                throw new PlatformNotSupportedException("This functionality is only supported on Windows.");
            }
        }
    }
}

