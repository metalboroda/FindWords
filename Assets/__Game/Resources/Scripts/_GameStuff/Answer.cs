using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Answer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
  {
    [SerializeField] private string _value;

    private bool _placed = true;
    private bool _returning = false;
    private Vector3 _startPosition;
    private Vector3 _offset;

    private Camera _mainCamera;
    private CameraAnchor _cameraAnchor;

    private void Awake()
    {
      _mainCamera = Camera.main;
      _cameraAnchor = GetComponent<CameraAnchor>();
    }

    private void Start()
    {
      _startPosition = transform.position;
      _placed = false;
    }

    public void OnTriggerEnter(Collider other)
    {
      if (_placed == true) return;
      if (_returning == true) return;

      if (other.TryGetComponent(out Variant variant))
      {
        variant.Receive(this);

        _placed = true;
      }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _cameraAnchor.enabled = false;

      Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, _mainCamera.nearClipPlane));

      _offset = transform.position - new Vector3(worldPosition.x, worldPosition.y, 0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (_placed == true) return;

      _returning = true;

      transform.DOMove(_startPosition, 0.2f).OnComplete(() =>
      {
        _returning = false;
        _cameraAnchor.enabled = true;
      });
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (_placed == true) return;
      if (_returning == true) return;

      Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0f));
      Vector3 targetPosition = new Vector3(worldPosition.x, worldPosition.y, 0f) + _offset;

      transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
    }

    public string GetValue()
    {
      return _value;
    }
  }
}