using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPageMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    
    private static readonly string levelToLoad = "Level 1";

    private void Start()
    {
        settings.SetActive(false);
        GameEventsManager.OnGameStart.AddListener(LoadLevel);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnGameStart.RemoveListener(LoadLevel);
    }

    public void StartGame()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        GameEventsManager.OnGameStart.Invoke();
    }

    private void LoadLevel()
    {
        StartCoroutine(WaitThenLoad());
        
        IEnumerator WaitThenLoad()
        {
            yield return new WaitForSeconds(GameDataManager.Instance.GetConfig().onWinLoadDelay);
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void OpenSettings()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        settings.SetActive(false);
    }
    
    public void Quit()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        Application.Quit();
    }
    
    public void ResetToDefault()
    {
        GameEventsManager.OnUIButtonClicked.Invoke();
        FindAnyObjectByType<SaveLoadManager>().ResetToDefault();
    }
}
