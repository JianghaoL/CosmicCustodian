using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandManager : MonoBehaviour, IInitializable
{
    public void InitializeOnAwake()
    {
        GameEventsManager.OnRestartRequested.AddListener(OnRestartRequested);
    }

    public void OnDestroy()
    {
        GameEventsManager.OnRestartRequested.RemoveListener(OnRestartRequested);
    }

    private void OnRestartRequested()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
