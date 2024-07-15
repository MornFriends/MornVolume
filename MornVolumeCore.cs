using UniRx;
using UnityEngine;

namespace MornVolume
{
    public sealed class MornVolumeCore : IMornVolumeCore
    {
        private readonly MornVolumeFadeSolver _fadeSolver;
        private readonly IMornVolumeSaver _volumeSaver;

        public MornVolumeCore(IMornVolumeSaver volumeSaver)
        {
            _volumeSaver = volumeSaver;
            _fadeSolver = new GameObject(nameof(MornVolumeFadeSolver)).AddComponent<MornVolumeFadeSolver>();
            _fadeSolver.Initialize(LoadAndApplyVolumes);
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
            MornVolumeGlobalSettings.Instance.Mixer.SetFloat(volumeKey, volumeDecibel);
            _volumeSaver.Save(volumeKey, volumeRate);
        }

        private void UpdateVolume(string volumeKey)
        {
            var volumeRate = _volumeSaver.Load(volumeKey);
            volumeRate *= _fadeSolver.GetVolumePair(volumeKey).FadeVolume;
            var volumeDecibel = VolumeRateToDecibel(volumeRate);
            MornVolumeGlobalSettings.Instance.Mixer.SetFloat(volumeKey, volumeDecibel);
        }

        private void LoadAndApplyVolumes()
        {
            foreach (var volumeKey in MornVolumeGlobalSettings.Instance.VolumeKeys) UpdateVolume(volumeKey);
        }

        private static float VolumeRateToDecibel(float rate)
        {
            return rate <= 0 ? -5000 : (1 - rate) * -30;
        }
    }
}