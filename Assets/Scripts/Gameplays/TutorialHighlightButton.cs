using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHighlightButton : MonoBehaviour
{
    private Image _image;
    
    [SerializeField] private float highlightDuration = 0.5f;

    private float _timer;
    private bool _fadeIn;

    private void Start()
    {
        _image = GetComponent<Image>();
        _timer = 0f;
        _fadeIn = true;
        _image.DOFade(0f, 0f);
    }

    private void Update()
    {
        if (!_image.enabled) return;
        
        _timer += Time.deltaTime;
        if (_timer > highlightDuration && _fadeIn)
        {
            _fadeIn = false;
            _image.DOFade(1f, highlightDuration);
            return;
        }
        
        if (_timer > highlightDuration * 2 && !_fadeIn)
        {
            _fadeIn = true;
            _image.DOFade(0f, highlightDuration);
            _timer = 0f;
        }
    }
}
