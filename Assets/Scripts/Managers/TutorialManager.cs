using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour, IInitializable
{
    public static TutorialManager Instance;
    [SerializeField] private List<TutorialSO> tutorials;
    
    private HashSet<int> _showedTutorials;
    private int order;
    private TutorialSO _tutorialOnDisplay;
    
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
        
        SortEntries();
        _showedTutorials = new HashSet<int>();
        order = 0;
        
        GameEventsManager.OnMapConstructed.AddListener(() =>
        {
            StartCoroutine(WaitForCamera());
            
            IEnumerator WaitForCamera()
            {
                yield return new WaitForSecondsRealtime(GameDataManager.Instance.GetConfig().cameraRotationDuration);
                GameEventsManager.OnTutorialLoaded.Invoke(true);
            }
        });
        
        GameEventsManager.OnMapConstructedTutorial.AddListener(ShowNextTutorial);
        GameEventsManager.OnMoveTutorial.AddListener(ShowNextTutorial);
        GameEventsManager.OnGameLoseTutorial.AddListener(ShowNextTutorial);
        GameEventsManager.OnUndoTutorial.AddListener(ShowNextTutorial);
        GameEventsManager.OnGameWinTutorial.AddListener(ShowNextTutorial);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnMapConstructedTutorial.RemoveListener(ShowNextTutorial);
        GameEventsManager.OnMoveTutorial.RemoveListener(ShowNextTutorial);
        GameEventsManager.OnGameLoseTutorial.RemoveListener(ShowNextTutorial);
        GameEventsManager.OnUndoTutorial.RemoveListener(ShowNextTutorial);
        GameEventsManager.OnGameWinTutorial.RemoveListener(ShowNextTutorial);
    }

    private void ShowNextTutorial(TutorialState s)
    {
        int i;
        switch (s)
        {
            case TutorialState.MapConstructed: i = 0; break;
            case TutorialState.FirstMove: i = 1; break;
            case TutorialState.GameLose: i = 2; break;
            case TutorialState.Undo: i = 3; break;
            case TutorialState.GameWin: i = 4; break;
            default: i = -1; break;
        }

        if (i == -1) return;
        if (i != order) return;
        
        order = i;
        
        if (!_showedTutorials.Add(order)) return; // if the requested tutorial is already shown, skip it.
        
        if (_tutorialOnDisplay != null) _tutorialOnDisplay.EndSpecialEffect();
        
        var tutorial = tutorials[order];
        var text = PlatformManager.Instance.GetPlatform() == PlatformManager.Platform.Mobile ? tutorial.mobileTutorialText : tutorial.desktopTutorialText;
        UIManager.Instance.SetPromptText(text);

        if (tutorial.HasSpecialEffect())
        {
            tutorial.StartSpecialEffect();
            _tutorialOnDisplay = tutorial;
        }
        
        order ++;
    }
    
    private void SortEntries()
    {
        tutorials.Sort(CompareOrder);

        int CompareOrder(TutorialSO x, TutorialSO y)
        {
            if (x.order < y.order)
                return -1;
            if (x.order > y.order)
                return 1;
            return 0;
        }
    }
}

public enum TutorialState
{
    MapConstructed,
    FirstMove,
    GameLose,
    Undo,
    GameWin
}
