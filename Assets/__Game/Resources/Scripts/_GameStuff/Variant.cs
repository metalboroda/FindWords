using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Variant : MonoBehaviour
  {
    public event Action<bool> Received;

    [SerializeField] private string _value;

    public bool CanReceive { get; private set; } = true;

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