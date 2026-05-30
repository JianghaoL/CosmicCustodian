using UnityEngine;

public class PlatformManager : MonoBehaviour, IInitializable
{
    public static PlatformManager Instance;

    public enum Platform
    {
        Mobile,
        Desktop
    }
    private Platform _platform;

    public void InitializeOnAwake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _platform = Application.isMobilePlatform ? Platform.Mobile : Platform.Desktop;
    }

    public Platform GetPlatform()
    {
        return _platform;
    }
}
