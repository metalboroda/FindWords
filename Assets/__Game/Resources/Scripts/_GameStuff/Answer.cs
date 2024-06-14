using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Answer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
  {
    [SerializeField] private string _value;
    [Header("Audio")]
    [SerializeField] private AudioClip _clip;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial = false;
    [SerializeField] private GameObject _tutorialFinger;
    [SerializeField] private Transform _tutorialTarget;

    private bool _placed = true;
    private bool _returning = false;
    private Vector3 _startPosition;
    private Vector3 _offset;
    private TextMeshProUGUI _text;
    private GameObject _spawnedFinger;

    private Camera _mainCamera;
    private CameraAnchor _cameraAnchor;

    private void Awake()
    {
      _text = GetComponentInChildren<TextMeshProUGUI>();

      _mainCamera = Camera.main;
      _cameraAnchor = GetComponent<CameraAnchor>();
    }

    private void Start()
    {
      _startPosition = transform.position;
      _placed = false;

      _text.text = _value;

      SpawnTutorialFinger();
    }

    public void OnTriggerEnter(Collider other)
    {
      if (_placed == true) return;
      if (_returning == true) return;

      if (other.TryGetComponent(out Variant variant))
      {
        if (variant.CanReceive == true)
        {
          variant.Receive(this);

          _placed = true;

          if (_tutorial == true)
          {
            DOTween.Kill(_spawnedFinger.transform);

            Destroy(_spawnedFinger);
          }
        }
      }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _cameraAnchor.enabled = false;

      Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, _mainCamera.nearClipPlane));

      _offset = transform.position - new Vector3(worldPosition.x, worldPosition.y, 0f);

      EventBus<EventStructs.VariantAudioClickedEvent>.Raise(new EventStructs.VariantAudioClickedEvent { AudioClip = _clip });
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

    private void SpawnTutorialFinger()
    {
      if (_tutorial == false) return;

      _spawnedFinger = Instantiate(_tutorialFinger, transform.position, Quaternion.identity);

      _spawnedFinger.transform.DOMove(_tutorialTarget.position, 1.5f)
        .SetLoops(-1);
    }
  }
}