using System.Threading;
using Cysharp.Threading.Tasks;

namespace MornVolume
{
    public interface IMornVolumeFade
    {
        void FadeInImmediate(string key);
        void FadeIn(string key);
        void FadeIn(string key, float duration);
        UniTask FadeInAsync(string key, CancellationToken ct);
        UniTask FadeInAsync(string key, float duration, CancellationToken ct);
        void FadeOutImmediate(string key);
        void FadeOut(string key);
        UniTask FadeOutAsync(string key, CancellationToken ct);
        UniTask FadeOutAsync(string key, float duration, CancellationToken ct);
    }
}