using UnityEngine;

public class SoundSpriteSwapView : BaseSpriteSwapView
{
    protected override Sprite Off()
    {
        return _audioSpriteSwap.SoundOff;
    }

    protected override Sprite On()
    {
        return _audioSpriteSwap.SoundOn;
    }

    protected override bool Get()
    {
        return _audioSpriteSwap.SoundValue;
    }

    protected override void Swap()
    {
        _audioSpriteSwap.SoundValue = !_audioSpriteSwap.SoundValue;
        UpdateState();
    }
}