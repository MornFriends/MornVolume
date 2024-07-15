namespace MornVolume
{
    internal class MornVolumePair
    {
        internal readonly string Key;
        internal float FadeVolume;

        internal MornVolumePair(string key)
        {
            Key = key;
            FadeVolume = 1;
        }
    }
}