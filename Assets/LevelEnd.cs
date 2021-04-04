using System;
using System.Collections;
using DG.Tweening;
using PixelCrew.Town;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [SerializeField] private Color _targetColor;
    private bool _complete;

    public void Complete(int loosedTeam)
    {
        gameObject.SetActive(true);
        var team = (TeamType) loosedTeam;

        _text.text = $"Team {team} loose!!! Kekeke...";
        StartCoroutine(WaitAndFinish());
    }

    private IEnumerator WaitAndFinish()
    {
        _image.DOColor(_targetColor, 0.5f);
        yield return new WaitForSeconds(1);
        _complete = true;
    }

    private void Update()
    {
        if (_complete)
        {
            SceneManager.LoadScene("Intro");
        }
    }
}