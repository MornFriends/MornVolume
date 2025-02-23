using System.Collections.Generic;
using MornGlobal;
using UnityEngine;
using UnityEngine.Audio;

namespace MornVolume
{
    [CreateAssetMenu(fileName = nameof(MornVolumeGlobal), menuName = "Morn/" + nameof(MornVolumeGlobal))]
    internal class MornVolumeGlobal : MornGlobalBase<MornVolumeGlobal>
    {
        protected override string ModuleName => nameof(MornVolume);
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private List<string> _volumeKeys;
        internal AudioMixer Mixer => _mixer;
        internal IReadOnlyList<string> VolumeKeys => _volumeKeys;
    }
}