using System.Collections.Generic;
using MornGlobal;
using UnityEngine;
using UnityEngine.Audio;

namespace MornVolume
{
    [CreateAssetMenu(fileName = nameof(MornVolumeGlobal), menuName = "Morn/" + nameof(MornVolumeGlobal))]
    internal class MornVolumeGlobal : MornGlobalBase<MornVolumeGlobal>
    {
#if DISABLE_MORN_ASPECT_LOG
        protected override bool ShowLog => false;
#else
        protected override bool ShowLog => true;
#endif
        protected override string ModuleName => nameof(MornVolume);
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private List<string> _volumeKeys;
        internal AudioMixer Mixer => _mixer;
        internal IReadOnlyList<string> VolumeKeys => _volumeKeys;
    }
}