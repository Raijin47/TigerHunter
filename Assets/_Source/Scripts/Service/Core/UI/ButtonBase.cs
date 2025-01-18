using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    #region Sub-Classes
    [System.Serializable]
    public class UIButtonEvent : UnityEvent<PointerEventData.InputButton> { }
    #endregion

    [SerializeField] private bool _interactable = true;

    public bool Interactable 
    {
        get => _interactable;
        set
        {
            _interactable = value;
            _canvas.alpha = value ? 1 : 0;
            _canvas.blocksRaycasts = value;
        }
    }

    [Space(10)]
    public UnityEvent OnClick;

    private RectTransform _rectTransform;
    private readonly Vector3 PressedSize = new(0.9f, 0.9f, 1);
    private const float ResizeDuration = 0.2f;
    private CanvasGroup _canvas;
    private Coroutine _resizeCoroutine; 

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        Interactable = Interactable;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!Interactable) return;
        
        if (_resizeCoroutine != null)      
            StopCoroutine(_resizeCoroutine);

        _resizeCoroutine = StartCoroutine(ResizeButton(PressedSize));
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!Interactable) return;

        if (_resizeCoroutine != null)
            StopCoroutine(_resizeCoroutine);

        _resizeCoroutine = StartCoroutine(ResizeButton(Vector3.one));
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable) return;

        OnClick?.Invoke();
        Game.Audio.OnClick();
    }

    private void OnEnable()
    {
        _rectTransform.localScale = Vector3.one;
    }

    private IEnumerator ResizeButton(Vector3 targetSize)
    {
        Vector3 initialSize = _rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < ResizeDuration)
        {
            _rectTransform.localScale = Vector3.Lerp(initialSize, targetSize, elapsedTime / ResizeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rectTransform.localScale = targetSize;
    }
}