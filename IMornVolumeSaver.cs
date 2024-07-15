namespace MornVolume
{
    public interface IMornVolumeSaver
    {
        float Load(string volumeKey);
        void Save(string volumeKey, float volumeRate);
    }
}