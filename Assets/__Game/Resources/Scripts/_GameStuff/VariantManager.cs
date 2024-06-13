using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System.Linq;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class VariantManager : MonoBehaviour
  {
    [SerializeField] private Variant[] _variants;

    private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
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

    private void CheckVariants(bool correct)
    {
      if (correct == false)
      {
        _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));

        return;
      }

      if (_variants.All(variant => variant.CanReceive == false))
        _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));
      else
        Debug.Log("Correct answer received, but not all variants are completed yet.");
    }
  }
}