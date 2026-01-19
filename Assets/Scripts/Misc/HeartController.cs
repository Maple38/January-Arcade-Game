using UnityEngine;

public class HeartController : MonoBehaviour
{
    private Animator _animator;
    private int _propertyHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _propertyHash = Animator.StringToHash("State"); // Hash this for efficiency
    }

    // Updates the "state" property to an integer, determining which sprite should show
    public void SetState(int state)
    {
        // 0 = Empty, 1 = Half, 2 = Full
        _animator.SetInteger(_propertyHash, state);
    }

    // Overrides the current playback timestamp. This is because after changing the animation, it'll restart,
    // making the glint effect not be in sync. So we use this function to sync all the hearts.
    public void SyncTime(float time)
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.Play(stateInfo.fullPathHash, 0, time);
    }

    // Gets the current progress into playing the animation clip
    public float GetTime()
    {
        // Fetch the current animation status
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }
}