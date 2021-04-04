using System;
using System.Collections;
using DG.Tweening;
using PixelCrew.Town;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private PlayerInput[] _input;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [SerializeField] private Color _targetColor;
    private bool _complete;

    public void Complete(int loosedTeam)
    {
        gameObject.SetActive(true);
        var team = (TeamType) loosedTeam;
        Keyboard.current.onTextInput += OnTextInput;

        foreach (var playerInput in _input)
        {
            playerInput.enabled = false;
        }

        _text.text = $"Team {team} loose!!! Kekeke...";
        StartCoroutine(WaitAndFinish());
    }

    private IEnumerator WaitAndFinish()
    {
        _image.DOColor(_targetColor, 0.5f);
        yield return new WaitForSeconds(1);
        _complete = true;
    }

    private void OnDestroy()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char obj)
    {
        if (_complete)
        {
            SceneManager.LoadScene("Intro");
        }
    }
}