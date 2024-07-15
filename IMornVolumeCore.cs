namespace MornVolume
{
    public interface IMornVolumeCore
    {
        void UpdateVolume(string volumeKey);
        void ApplyAndSaveVolume(string volumeKey, float volumeRate);
    }
}