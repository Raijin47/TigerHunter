using UnityEngine;

public class MusicSpriteSwapView : BaseSpriteSwapView
{
    protected override Sprite Off()
    {
        return _audioSpriteSwap.MusicOff;
    }

    protected override Sprite On()
    {
        return _audioSpriteSwap.MusicOn;
    }

    protected override bool Get()
    {
        return _audioSpriteSwap.MusicValue;
    }

    protected override void Swap()
    {
        _audioSpriteSwap.MusicValue = !_audioSpriteSwap.MusicValue;
        UpdateState();
    }
}