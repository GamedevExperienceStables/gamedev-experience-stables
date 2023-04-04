namespace Game.Persistence
{
    public interface IPlayerPrefs
    {
        void SetFloat(string key, float value);
        float GetFloat(string key, float defaultValue);
        
        void SetString(string key, string value);
        string GetString(string key, string defaultValue);
    }
}