using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UI.DataGroup
{
    public class ComponentDataGroup<TDataType, TItemRenderer> : MonoBehaviour, IDataGroup<TDataType>
        where TItemRenderer : MonoBehaviour, IItemRenderer<TDataType>
    {
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _prefab;

        private readonly List<TItemRenderer> _views = new List<TItemRenderer>();
        private Action<TItemRenderer> _setup = delegate { };

        public void Setup(Action<TItemRenderer> setup)
        {
            _setup += setup;
        }

        public void SetData(IList<TDataType> itemData)
        {
            for (var i = _views.Count; i < itemData.Count; i++)
            {
                var instance = Instantiate(_prefab, _content).GetComponent<TItemRenderer>();
                _setup(instance);
                _views.Add(instance);
            }

            for (var i = 0; i < itemData.Count; i++)
            {
                _views[i].SetData(itemData[i]);
            }

            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].gameObject.SetActive(i < itemData.Count);
            }
        }
    }

    public class ComponentRenderer<TDataType> : MonoBehaviour, IItemRenderer<TDataType>
    {
        public void SetData(TDataType itemData)
        {
        }
    }
}