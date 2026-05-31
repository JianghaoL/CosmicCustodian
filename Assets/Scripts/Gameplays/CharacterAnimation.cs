using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private static readonly int Walking = Animator.StringToHash("Walk");
    private static readonly int WithBox = Animator.StringToHash("With Box");
    private static readonly int Win = Animator.StringToHash("Win");
    private static readonly int Lose = Animator.StringToHash("Lose");
    
    private Animator _animator;
    private Transform _transform;
    private bool _isWithBox;
    private bool _isTutorial;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        
        GameEventsManager.OnPlayerMoving.AddListener(OnMove);
        GameEventsManager.OnMoveCompleted.AddListener(OnMoveCompleted);
        GameEventsManager.OnBoxMoveRequested.AddListener(SetIfWithBox);
        
        GameEventsManager.OnTutorialLoaded.AddListener(OnTutorial);
        
        GameEventsManager.OnGameWin.AddListener(OnGameWin);
        GameEventsManager.OnGameLose.AddListener(OnGameLose);
    }

    private void Start()
    {
        _transform.rotation = new Quaternion(0f, 90f, 0f, 0f);
        _isTutorial = false;
    }

    private void OnDestroy()
    {
        GameEventsManager.OnPlayerMoving.RemoveListener(OnMove);
        GameEventsManager.OnMoveCompleted.RemoveListener(OnMoveCompleted);
        GameEventsManager.OnBoxMoveRequested.RemoveListener(SetIfWithBox);
        
        GameEventsManager.OnTutorialLoaded.RemoveListener(OnTutorial);
        
        GameEventsManager.OnGameWin.RemoveListener(OnGameWin);
        GameEventsManager.OnGameLose.RemoveListener(OnGameLose);
    }

    private void OnMove(Vector2Int dir)
    {
        _animator.SetBool(Walking, true);

        Vector3 rotation;
        if (dir.x == 0)
        {
            rotation = dir.y > 0 ? new Vector3(0f, 0f, 0f) : new Vector3(0f, 180f, 0f);
        }
        else
        {
            rotation = dir.x > 0 ? new Vector3(0f, 90f, 0f) : new Vector3(0f, -90f, 0f);
        }
        
        var newRot = Quaternion.Euler(rotation);
        _transform.DORotateQuaternion(newRot, GameDataManager.Instance.GetConfig().turnDuration);
    }
    
    private void OnMoveCompleted()
    {
        _animator.SetBool(Walking, false);
        _animator.SetBool(WithBox, false);
    }

    private void SetIfWithBox(Vector2Int dir)
    {
        _animator.SetBool(WithBox, true);
    }

    private void OnGameWin()
    {
        _animator.SetTrigger(Win);
    }

    private void OnGameLose()
    {
        Debug.Log($"Is tutorial {_isTutorial}, Game lose animation");
        if (_isTutorial) return;
        
        _animator.SetTrigger(Lose);
    }
    
    private void OnTutorial(bool isTutorial)
    {
        _isTutorial = isTutorial;
    }
}
