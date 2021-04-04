using Common.UI.DataGroup;
using PixelCrew.Data;
using PixelCrew.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Mobs
{
    public class MobControl : MonoBehaviour, IItemRenderer<MobItem>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _count;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private GameObject _selection;
        [SerializeField] private GameObject _container;
        [SerializeField] private Image _upgradeProgressBar;

        [Space] [SerializeField] private Events.StringEvent _onSelect;

        private MobState _state;
        private int _index;
        private readonly Disposables _trash = new Disposables();
        private IntProperty _selectedIndex;

        public bool IsUpgrading { get; set; }
        private float _upgradeProgress;
        private float _timer;
        private MobData _def;

        public void SetData(MobItem itemData)
        {
            _index = itemData.Index;
            _state = itemData.State;
            _def = Defs.I.Mobs.Get(_state.Id);
            _icon.sprite = _def.Icon;

            _trash.Add(_state.Level.SubscribeAndFire(UpdateLevel));
            _trash.Add(_state.Count.SubscribeAndFire(count => _count.text = $"x{count}"));

            UpdateSelection(_selectedIndex.Value);
        }

        private void UpdateLevel(int level)
        {
            _level.text = $"lvl. {level + 1}";
            _price.text = _def.GetLevelData(_state.Level.Value).Price.ToString();
        }

        private float UpgradeTime => _def.GetLevelData(_state.Level.Value).UpgradeTime;

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public void SetSelection(IntProperty selection)
        {
            _selectedIndex = selection;
            _trash.Add(selection.SubscribeAndFire(UpdateSelection));
        }

        private void UpdateSelection(int index)
        {
            _container.transform.localScale = Vector3.one * (index == _index ? 1.2f : 1f);
            _selection.SetActive(index == _index);
        }

        private void Update()
        {
            if (IsUpgrading)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _timer -= Time.deltaTime;
                _timer = Mathf.Max(0, _timer);
            }

            _upgradeProgress = Mathf.Clamp(_timer / UpgradeTime, 0, 1);
            _upgradeProgressBar.fillAmount = _upgradeProgress;

            if (_upgradeProgress >= 1)
            {
                _timer = 0;
                _state.Level.Value++;
                Debug.Log(UpgradeTime);
            }
        }

        public void BuyMob()
        {
            _onSelect.Invoke(_state.Id);
        }
    }

    public class MobItem
    {
        public int Index;
        public MobState State;
    }
}