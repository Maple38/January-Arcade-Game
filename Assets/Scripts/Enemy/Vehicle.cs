using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float _linearAccel;
    [SerializeField] private float _speedMax;
    [SerializeField] private float _wheelAngleMax;
    [SerializeField] private float _wheelOffsetFront;
    [SerializeField] private float _wheelOffsetBack;
    private Vector2 _targetPos;
    private Vector2 _desiredDir;
    private float _wheelAngle;
    private float _wheelSpacing;
    private float _turnRadius;
    private float _turnRadiusMax;

    void Start()
    {
        _turnRadiusMax = CalcTurnRadius(_wheelSpacing, _wheelAngleMax);
        _targetPos = new Vector2(0, 0);
        _wheelSpacing = Mathf.Abs(_wheelOffsetFront) + Mathf.Abs(_wheelOffsetBack);
    }

    void Update()
    {
        _turnRadius = CalcTurnRadius(_wheelSpacing, _wheelAngle);
        
        if (Input.GetMouseButtonDown(0))
        {
            _targetPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    float CalcTurnRadius(float wheelBase, float x)
    {
        var xRad = x * Mathf.Deg2Rad;
        var sinx = Mathf.Sin(xRad); 
        var scale = wheelBase / sinx;
        var cosDistance = Mathf.Cos(xRad) * scale;
        return cosDistance;
    }

    void OnDrawGizmosSelected()
    {
        _wheelSpacing = Mathf.Abs(_wheelOffsetFront) + Mathf.Abs(_wheelOffsetBack);
        _turnRadiusMax = CalcTurnRadius(_wheelSpacing, _wheelAngleMax);
        _turnRadius = CalcTurnRadius(_wheelSpacing, _wheelAngle);
        
        Vector2 wheelPosFront = transform.position + transform.up * _wheelOffsetFront;
        Vector2 wheelPosBack = transform.position + transform.up * _wheelOffsetBack;
        Vector2 centerOfTurn = wheelPosFront + transform.right;
        Vector2 centerOfTurnMax = wheelPosBack + ((Vector2)transform.right * _turnRadiusMax);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_targetPos, 0.1f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(wheelPosFront, wheelPosBack);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(wheelPosFront, centerOfTurn);
        Gizmos.DrawLine(wheelPosBack, centerOfTurn);
        Gizmos.DrawWireSphere(centerOfTurn, _turnRadius);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wheelPosFront, centerOfTurnMax);
        Gizmos.DrawLine(wheelPosBack, centerOfTurnMax);
        Gizmos.DrawWireSphere(centerOfTurnMax, _turnRadiusMax);

    }
}