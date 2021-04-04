using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI
{
    public class IntroFlow : MonoBehaviour
    {
        private bool _waitForAction = true;
        [SerializeField] private Transform _camera;
        [SerializeField] private float _moveTime = 2f;
        [SerializeField] private Transform _tutorialPosition;
        [SerializeField] private Transform _gamePosition;

        private void Awake()
        {
            _currentAction = MoveToTutorial;
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                if (_waitForAction)
                {
                    _waitForAction = false;
                    _currentAction();
                }
            }
        }

        private void MoveToTutorial()
        {
            _camera.DOMoveY(_tutorialPosition.position.y, _moveTime)
                .SetEase(Ease.InOutBack)
                .OnComplete(() =>
                {
                    _currentAction = MoveToGame;
                    _waitForAction = true;
                });
        }

        private void MoveToGame()
        {
            SceneManager.LoadScene("Level", LoadSceneMode.Additive);
            _camera.DOMoveY(_gamePosition.position.y, _moveTime)
                .SetEase(Ease.InOutBack)
                .OnComplete(() => { Destroy(gameObject); });
        }

        private Action _currentAction;
    }
}