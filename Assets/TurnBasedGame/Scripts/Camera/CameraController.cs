using TurnBasedGame.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [field: Header("Battle grid")]
    [SerializeField] private BattleGrid _battleGrid;
    
    [field: Header("Zooming and Rotation values")]
    
    [SerializeField] private float _minZoomDistance;
    [SerializeField] private float _maxZoomDistance;
    [SerializeField] private float _baseZoomDistance;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _yMinLimit;
    [SerializeField] private float _yMaxLimit;
    
    [field: Header("Position values")]
    [field: SerializeField] public float DistanceFromCenterOfGrid { get; set; }
    [field: SerializeField] public float Angle { get; set; }
    
    
    private float _xRotation;
    private float _yRotation;
    
    private void LateUpdate()
    {
        RotateCamera();
        ZoomCamera();
    }

    private void Start()
    {
        SetDistanceToTheCenter();
    }

    private void SetDistanceToTheCenter()
    {
        DistanceFromCenterOfGrid = _baseZoomDistance;
        GetBaseRotationAndPosition();
        
        var angles = transform.eulerAngles;
        _xRotation = angles.y;
        _yRotation = angles.x;
    }

    private void GetBaseRotationAndPosition()
    {
        Vector3 center = new Vector3(_battleGrid.Columns / 2f - 0.5f, 0f, _battleGrid.Rows / 2f - 0.5f);
        Quaternion targetRotation = Quaternion.Euler(Angle, 0f, 0f);
        Vector3 position = targetRotation * new Vector3(0f, 0f, -DistanceFromCenterOfGrid) + center;
        
        transform.position = position;
        transform.rotation = targetRotation;
    }

    private void GetCurrentRotationAndPosition()
    {
        Vector3 center = new Vector3(_battleGrid.Columns / 2f - 0.5f, 0f, _battleGrid.Rows / 2f - 0.5f);
        Quaternion rotation = Quaternion.Euler(_yRotation, _xRotation, 0);
        Vector3 position = rotation * new Vector3(0f, 0f, -DistanceFromCenterOfGrid) + center;
            
        transform.position = position;
        transform.rotation = rotation;
    }

    private void RotateCamera()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (Input.GetMouseButton(1))
        {
            _xRotation += Input.GetAxis("Mouse X") * _rotationSpeed;
            _yRotation -= Input.GetAxis("Mouse Y") * _rotationSpeed;
            
            _yRotation = ClampAngle(_yRotation, _yMinLimit, _yMaxLimit);
        
            GetCurrentRotationAndPosition();
        }
    }

    private void ZoomCamera()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            DistanceFromCenterOfGrid = Mathf.Clamp(DistanceFromCenterOfGrid - scroll * _zoomSpeed,
                _minZoomDistance, _maxZoomDistance);
            
            GetCurrentRotationAndPosition();
        }
    }


    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
