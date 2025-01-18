using UnityEngine;

public class CharacterAnimation
{
    private readonly Animator Animator;

    private const string _locomotion = "Locomotion";
    private const string _loseKey = "OnLose";
    private const string _winKey = "OnWin";
    private const string _isGameKey = "OnGame";

    public CharacterAnimation(Animator animator) => Animator = animator;

    public bool IsActive { set { Animator.enabled = value; } }
    public bool OnLose { set { Animator.SetBool(_loseKey, value); } }
    public bool OnWin { set { Animator.SetBool(_winKey, value); } }
    public bool OnGame { set { Animator.SetBool(_isGameKey, value); } }

    public void MovementAnimations(float moveAmount)
    {
        Animator.SetFloat(_locomotion, moveAmount, 0.1f, Time.deltaTime);
    }

    public void Reset()
    {
        OnLose = false;
        OnWin = false;
        IsActive = true;
    }
}