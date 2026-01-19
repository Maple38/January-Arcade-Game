using Unity.VisualScripting;
using UnityEngine;

public class ScrollingSprite : MonoBehaviour
{
    private float _scrollSpeed;
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;
    [SerializeField] private float spriteWidth;
    
    void Start()
    {
        _scrollSpeed = GameManager.Instance.globalScrollSpeed;
        bg1.localPosition = Vector3.zero;
        bg2.localPosition = new Vector3(0, spriteWidth, 0);
    }
    
    void Update()
    {
        bg2.Translate(Vector3.down * (_scrollSpeed * Time.deltaTime));
        bg1.Translate(Vector3.down * (_scrollSpeed * Time.deltaTime));

        if (bg1.localPosition.y <= -spriteWidth)
        {
            bg1.localPosition = new Vector3(0, bg2.localPosition.y + spriteWidth, 0);
        }  
        if (bg2.localPosition.y <= -spriteWidth)
        {
            bg2.localPosition = new Vector3(0, bg1.localPosition.y + spriteWidth, 0);
        }
    }
}
