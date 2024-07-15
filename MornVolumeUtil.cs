using UnityEngine;

namespace MornVolume
{
    internal static class MornVolumeUtil
    {
#if DISABLE_MORN_VOLUME_LOG
        private const bool ShowLOG = false;
#else
        private const bool ShowLOG = true;
#endif
        private const string Prefix = "[<color=green>MornVolume</color>] ";

        internal static void Log(string message)
        {
            if (ShowLOG) Debug.Log(Prefix + message);
        }

        internal static void LogError(string message)
        {
            if (ShowLOG) Debug.LogError(Prefix + message);
        }

        internal static void LogWarning(string message)
        {
            if (ShowLOG) Debug.LogWarning(Prefix + message);
        }
    }
}