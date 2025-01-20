using UnityEngine;

public class CharacterAnimation
{
    private readonly Animator Animator;

    private const string _locomotion = "Locomotion";
    private const string _attack = "isAttack";
    public CharacterAnimation(Animator animator) => Animator = animator;

    public bool IsActive { set { Animator.enabled = value; } }
    public bool OnLose { set {  } }
    public bool OnWin { set {  } }
    public bool OnGame { set {  } }

    public void MovementAnimations(float moveAmount)
    {
        Animator.SetFloat(_locomotion, moveAmount, 0.1f, Time.deltaTime);
    }

    public void Attack()
    {
        Animator.SetTrigger(_attack);
    }

    public void Reset()
    {
        OnLose = false;
        OnWin = false;
        IsActive = true;
    }
}