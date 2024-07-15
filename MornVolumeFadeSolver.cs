using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace MornVolume
{
    internal class MornVolumeFadeSolver : MonoBehaviour, IMornVolumeFade
    {
        [SerializeField] private float _defaultFadeInDuration = 0.3f;
        [SerializeField] private float _defaultFadeOutDuration = 0.6f;
        private readonly Subject<MornVolumePair> _fadeUpdateSubject = new();
        private readonly Dictionary<string, MornVolumePair> _fadeVolumeDict = new();
        private CancellationTokenSource _cts;
        private Action _onStart;
        internal IObservable<MornVolumePair> OnFadeUpdate => _fadeUpdateSubject;

        private void Start()
        {
            _onStart?.Invoke();
        }

        void IMornVolumeFade.FadeInImmediate(string key)
        {
            FadeFillAsync(key, 0).Forget();
        }

        void IMornVolumeFade.FadeIn(string key)
        {
            FadeFillAsync(key, _defaultFadeInDuration).Forget();
        }

        void IMornVolumeFade.FadeIn(string key, float duration)
        {
            FadeFillAsync(key, duration).Forget();
        }

        async UniTask IMornVolumeFade.FadeInAsync(string key, CancellationToken ct)
        {
            await FadeFillAsync(key, _defaultFadeInDuration, ct);
        }

        UniTask IMornVolumeFade.FadeInAsync(string key, float duration, CancellationToken ct)
        {
            return FadeFillAsync(key, duration, ct);
        }

        void IMornVolumeFade.FadeOutImmediate(string key)
        {
            FadeClearAsync(key, 0).Forget();
        }

        void IMornVolumeFade.FadeOut(string key)
        {
            FadeClearAsync(key, _defaultFadeOutDuration).Forget();
        }

        async UniTask IMornVolumeFade.FadeOutAsync(string key, CancellationToken ct)
        {
            await FadeClearAsync(key, _defaultFadeOutDuration, ct);
        }

        async UniTask IMornVolumeFade.FadeOutAsync(string key, float duration, CancellationToken ct)
        {
            await FadeClearAsync(key, duration, ct);
        }

        internal void Initialize(Action onStart)
        {
            _onStart = onStart;
        }

        internal MornVolumePair GetVolumePair(string key)
        {
            if (!_fadeVolumeDict.TryGetValue(key, out var pair))
            {
                pair = new MornVolumePair(key);
                _fadeVolumeDict.Add(key, pair);
            }

            return pair;
        }

        private async UniTask FadeClearAsync(string key, float duration, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var pair = GetVolumePair(key);
            await VolumeTweenTask(pair, pair.FadeVolume, 0, duration, _cts.Token);
        }

        private async UniTask FadeFillAsync(string key, float duration, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var pair = GetVolumePair(key);
            await VolumeTweenTask(pair, pair.FadeVolume, 1, duration, _cts.Token);
        }

        private async UniTask VolumeTweenTask(MornVolumePair pair, float startVolume, float endVolume, float duration,
            CancellationToken ct)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                pair.FadeVolume = Mathf.Lerp(startVolume, endVolume, elapsedTime / duration);
                _fadeUpdateSubject.OnNext(pair);
                await UniTask.Yield(ct);
            }

            pair.FadeVolume = endVolume;
            _fadeUpdateSubject.OnNext(pair);
        }
    }
}