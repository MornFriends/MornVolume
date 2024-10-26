using UniRx;
using UnityEngine;

namespace MornVolume
{
    public sealed class MornVolumeCore : IMornVolumeCore
    {
        private readonly MornVolumeFadeSolver _fadeSolver;
        private readonly IMornVolumeSaver _volumeSaver;

        public MornVolumeCore(IMornVolumeSaver volumeSaver, MornVolumeFadeSolver fadeSolver)
        {
            _volumeSaver = volumeSaver;
            var trigger = new GameObject(nameof(MornVolumeTrigger)).AddComponent<MornVolumeTrigger>();
            trigger.Initialize(LoadAndApplyVolumes);
            _fadeSolver = fadeSolver;
            _fadeSolver.OnFadeUpdate.Subscribe(pair => UpdateVolume(pair.Key));
        }

        void IMornVolumeCore.UpdateVolume(string volumeKey)
        {
            UpdateVolume(volumeKey);
        }

        void IMornVolumeCore.ApplyAndSaveVolume(string volumeKey, float volumeRate)
        {
            volumeRate *= _fadeSolver.GetVolumePair(volumeKey).FadeVolume;
            var volumeDecibel = VolumeRateToDecibel(volumeRate);
            MornVolumeGlobal.I.Mixer.SetFloat(volumeKey, volumeDecibel);
            _volumeSaver.Save(volumeKey, volumeRate);
        }

        private void UpdateVolume(string volumeKey)
        {
            var volumeRate = _volumeSaver.Load(volumeKey);
            volumeRate *= _fadeSolver.GetVolumePair(volumeKey).FadeVolume;
            var volumeDecibel = VolumeRateToDecibel(volumeRate);
            MornVolumeGlobal.I.Mixer.SetFloat(volumeKey, volumeDecibel);
        }

        private void LoadAndApplyVolumes()
        {
            foreach (var volumeKey in MornVolumeGlobal.I.VolumeKeys) UpdateVolume(volumeKey);
        }

        public static float VolumeRateToDecibel(float rate)
        {
            return rate <= 0 ? -5000 : (1 - rate) * -30;
        }
    }
}