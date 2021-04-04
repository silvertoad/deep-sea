using System.Linq;
using PixelCrew.Chars;
using PixelCrew.State;
using PixelCrew.Town;
using PixelCrew.UI.Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private TownController _town;
        [Space] [SerializeField] private MobList _mobList;

        [SerializeField] private TextMeshProUGUI _coins;


        [SerializeField] private Cooldown _selectCooldown;
        [SerializeField] private IntProperty _selection = new IntProperty();
        private int _selected;
        private MobControl[] _controls;
        private bool _lockSelection;

        private void Start()
        {
            _town._state.Coins.SubscribeAndFire(coins => _coins.text = $"x{coins}");

            _mobList.Setup(x => x.SetSelection(_selection));
            _mobList.SetData(_town.State.Mobs.Select((x, i) => new MobItem {State = x, Index = i}).ToArray());
            var controls = _mobList.GetComponentsInChildren<MobControl>();
            _controls = controls.Where(x => x.gameObject.activeSelf).ToArray();
        }

        private void Update()
        {
            _selectCooldown.Tick();
        }

        private void Select(int delta)
        {
            var newIndex = (int) Mathf.Repeat(_selection.Value + delta, _controls.Length);

            _selectCooldown.Reset();
            _selection.Value = newIndex;
        }

        public void OnSelection(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<float>();
            if (!_selectCooldown.IsReady) return;
            if (direction == 0) return;
            if (_lockSelection) return;
            Select(direction > 0 ? 1 : -1);
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _controls[_selection.Value].BuyMob();
                Debug.Log("OnUse");
            }
        }

        public void OnUpgrade(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _controls[_selection.Value].IsUpgrading = true;
                _lockSelection = true;
                Debug.Log("upgrade stared");
            }

            if (context.canceled)
            {
                _lockSelection = false;
                _controls[_selection.Value].IsUpgrading = false;
                Debug.Log("upgrade cancelled");
            }
        }
    }
}