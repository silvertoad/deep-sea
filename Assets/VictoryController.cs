using System.Collections;
using System.Linq;
using DG.Tweening;
using PixelCrew.Components;
using TMPro;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [SerializeField] private GameObject _vic;
    [SerializeField] private Vector3 _target;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private GameObject _whereText;
    [SerializeField] private AudioSource _sceam;

    [SerializeField] private float _matchTime;

    private bool _isTicked = true;

    private void Update()
    {
        if (!_isTicked) return;
        _matchTime -= Time.deltaTime;
        _matchTime = Mathf.Max(0, _matchTime);
        _timer.text = $"{ToTime(_matchTime / 60)}:{ToTime(_matchTime % 60)}";

        if (_matchTime == 0)
        {
            _isTicked = false;
            _sceam.Play();
            _vic.transform.DOMove(_target, 2f)
                .OnComplete(() =>
                {
                    _whereText.SetActive(true);
                    StartCoroutine(StartDestroy());
                });
        }
    }

    private IEnumerator StartDestroy()
    {
        var objects = FindObjectsOfType<HealthComponent>().Where(x => !x.IsDead).ToArray();
        while (objects.Length > 0)
        {
            foreach (var healthComponent in objects)
            {
                healthComponent.Modify(-2);
            }

            _sceam.Play();
            objects = FindObjectsOfType<HealthComponent>().Where(x => !x.IsDead).ToArray();
            yield return new WaitForSeconds(2f);
        }
    }

    private string ToTime(float value)
    {
        var ret = ((int) value).ToString();
        if (ret.Length == 1)
            ret.Insert(0, "0");
        return ret;
    }
}