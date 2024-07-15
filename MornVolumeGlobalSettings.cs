using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornVolume
{
    [CreateAssetMenu(fileName = nameof(MornVolumeGlobalSettings),
        menuName = "Morn/" + nameof(MornVolumeGlobalSettings))]
    internal class MornVolumeGlobalSettings : ScriptableObject
    {
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private List<string> _volumeKeys;
        internal static MornVolumeGlobalSettings Instance { get; private set; }
        internal AudioMixer Mixer => _mixer;
        internal IReadOnlyList<string> VolumeKeys => _volumeKeys;

        private void OnEnable()
        {
            Instance = this;
            MornVolumeUtil.Log("Global Settings Loaded");
#if UNITY_EDITOR
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Contains(this) &&
                preloadedAssets.Count(x => x is MornVolumeGlobalSettings) == 1) return;
            preloadedAssets.RemoveAll(x => x is MornVolumeGlobalSettings);
            preloadedAssets.Add(this);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            MornVolumeUtil.Log("Global Settings Added to Preloaded Assets!");
#endif
        }

        private void OnDisable()
        {
            Instance = null;
            MornVolumeUtil.Log("Global Settings Unloaded");
        }
    }
}