using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System.Linq;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class VariantManager : MonoBehaviour
  {
    [SerializeField] private Variant[] _variants;
    [Header("Audio")]
    [SerializeField] private AudioClip _correctAudioClip;
    [SerializeField] private AudioClip _incorrectAudioClip;

    private AudioSource _audioSource;

    private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();

      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      foreach (var variant in _variants)
      {
        variant.Received += CheckVariants;
      }
    }

    private void OnDisable()
    {
      foreach (var variant in _variants)
      {
        variant.Received -= CheckVariants;
      }
    }

    private void Start()
    {
      EventBus<EventStructs.VariantsAssignedEvent>.Raise(new EventStructs.VariantsAssignedEvent());
    }

    private void CheckVariants(bool correct)
    {
      if (correct == false)
      {
        _audioSource.PlayOneShot(_incorrectAudioClip);

        _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));

        return;
      }

      if (_variants.All(variant => variant.CanReceive == false))
      {
        _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));

        _audioSource.PlayOneShot(_correctAudioClip);
      }
      else
      {
        _audioSource.PlayOneShot(_correctAudioClip);
      }
    }
  }
}