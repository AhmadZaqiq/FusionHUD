using FusionHUD.Performance.Interfaces;
using FusionHUD.Performance.Models;
using FusionHUD.Performance.Native;
using System.IO.MemoryMappedFiles;
using System.Runtime.Versioning;

namespace FusionHUD.Performance.Services
{
    [SupportedOSPlatform("windows")]
    public sealed class FPSDataProvider : IFPSDataProvider
    {
        private const string SHARED_MEMORY_NAME = "RTSSSharedMemoryV2";

        private const int FPS_OFFSET = 276; // Offset of the FPS value within each RTSS application entry.

        private const int SMOOTH_SAMPLES = 5; // Number of recent samples used to smooth FPS readings.

        private const int ENTRY_SIZE_OFFSET = 8; // Offset of the application entry size in the RTSS shared memory header.

        private const int APPLICATION_OFFSET_OFFSET = 12; // Offset of the application entries in the RTSS shared memory header.

        private const int APPLICATION_COUNT_OFFSET = 16;// Offset of the application count in the RTSS shared memory header.

        private readonly Queue<float> _FPSHistory = new();

        private readonly object _Lock = new();

        public FPSData GetFPSData()
        {
            try
            {
                uint ForegroundProcessID = WindowsProcessInterop.GetForegroundProcessID();

                if (ForegroundProcessID == 0)
                {
                    ClearFPSHistory();

                    return new FPSData();
                }

                using MemoryMappedFile Memory = MemoryMappedFile.OpenExisting(SHARED_MEMORY_NAME);

                using MemoryMappedViewAccessor Accessor = Memory.CreateViewAccessor();

                uint EntrySize = Accessor.ReadUInt32(ENTRY_SIZE_OFFSET);

                uint ApplicationOffset = Accessor.ReadUInt32(APPLICATION_OFFSET_OFFSET);

                uint ApplicationCount = Accessor.ReadUInt32(APPLICATION_COUNT_OFFSET);

                if (EntrySize == 0 || ApplicationCount == 0)
                {
                    ClearFPSHistory();

                    return new FPSData();
                }

                for (uint Index = 0; Index < ApplicationCount; Index++)
                {
                    long EntryOffset = ApplicationOffset + (Index * EntrySize);

                    uint ProcessID = Accessor.ReadUInt32(EntryOffset);

                    if (ProcessID != ForegroundProcessID)
                    {
                        continue;
                    }

                    int FPS = Accessor.ReadInt32(EntryOffset + FPS_OFFSET) - 1;

                    if (FPS <= 0)
                    {
                        ClearFPSHistory();

                        return new FPSData();
                    }

                    return new FPSData
                    {
                        FPS = AddFPSReading(FPS),
                        GameName = WindowsProcessInterop.GetProcessName(ForegroundProcessID)
                    };
                }

                ClearFPSHistory();

                return new FPSData();
            }
            catch
            {
                ClearFPSHistory();

                return new FPSData();
            }
        }

        private float AddFPSReading(float FPS)
        {
            lock (_Lock)
            {
                _FPSHistory.Enqueue(FPS);

                if (_FPSHistory.Count > SMOOTH_SAMPLES)
                {
                    _FPSHistory.Dequeue();
                }

                return _FPSHistory.Average();
            }
        }

        private void ClearFPSHistory()
        {
            lock (_Lock)
            {
                _FPSHistory.Clear();
            }
        }
    }
}