using Common.UI.DataGroup;
using PixelCrew.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Mobs
{
    public class MobControl : MonoBehaviour, IItemRenderer<MobState>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _count;
        
        [Space] [SerializeField] private Events.StringEvent _onSelect;

        private MobState _state;
        private readonly Disposables _trash = new Disposables();

        public void SetData(MobState itemData)
        {
            _state = itemData;
            var def = Defs.I.Mobs.Get(itemData.Id);
            _icon.sprite = def.Icon;

            _trash.Add(_state.Level.SubscribeAndFire(level => _level.text = $"lvl. {level + 1}"));
            _trash.Add(_state.Count.SubscribeAndFire(count => _count.text = $"x{count}"));
        }

        public void OnClick()
        {
            _onSelect.Invoke(_state.Id);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}