using UnityEngine;

public class HeartController : MonoBehaviour
{
    private Animator _animator;
    private int _propertyHash;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _propertyHash = Animator.StringToHash("State");
    }

    public void SetState(int state)
    {
        // 0 = Empty, 1 = Half, 2 = Full
        _animator.SetInteger(_propertyHash, state);
    }
}
