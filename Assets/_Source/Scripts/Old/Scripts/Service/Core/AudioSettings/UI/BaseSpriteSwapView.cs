using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonBase))]
public abstract class BaseSpriteSwapView : MonoBehaviour
{
    [SerializeField] private Image _targetImage;

    private ButtonBase _button;
    protected AudioSpriteSwap _audioSpriteSwap;

    private void Awake() => _button = GetComponent<ButtonBase>();

    private void Start()
    {
        _audioSpriteSwap = Game.Instance.AudioSettings;
        _button.OnClick.AddListener(Swap);
        UpdateState();
    }

    private void OnEnable()
    {
        if (_audioSpriteSwap == null) return;
        UpdateState();
    }

    protected void UpdateState()
    {
        _targetImage.sprite = Get() ? On() : Off();
    }

    protected abstract Sprite On();
    protected abstract Sprite Off();

    protected abstract bool Get();
    protected abstract void Swap();
}