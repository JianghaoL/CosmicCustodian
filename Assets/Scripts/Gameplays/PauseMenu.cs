using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    private void Start()
    {
        GameEventsManager.OnPauseRequested.AddListener(ShowPauseMenu);
        GameEventsManager.OnResumeRequested.AddListener(ClosePauseMenu);
        
        ClosePauseMenu();
    }

    private void OnDestroy()
    {
        GameEventsManager.OnPauseRequested.RemoveListener(ShowPauseMenu);
        GameEventsManager.OnResumeRequested.RemoveListener(ClosePauseMenu);
    }

    private void ShowPauseMenu()
    {
        pauseMenuUI.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        pauseMenuUI.SetActive(false);
    }

    public void Resume()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        GameEventsManager.OnResumeRequested.Invoke();
    }
    
    public void ResetToDefault()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        FindAnyObjectByType<SaveLoadManager>().ResetToDefault();
    }

    public void ToMainMenu()
    {
        GameEventsManager.OnGameQuit.Invoke();
    }
    
}
