using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Variant : MonoBehaviour
  {
    public event Action<bool> Received;

    [SerializeField] private Sprite _sprite;
    [Space]
    [SerializeField] private Image _image;
    [Header("")]
    [SerializeField] private string _value;

    public bool CanReceive { get; private set; } = true;

    private void Start()
    {
      _image.sprite = _sprite;
    }

    public void Receive(Answer answer)
    {
      if (CanReceive == false) return;

      CanReceive = false;

      answer.transform.DOMove(transform.position, 0.2f);

      if (answer.GetValue() == _value)
        Received?.Invoke(true);
      else
        Received?.Invoke(false);
    }
  }
}