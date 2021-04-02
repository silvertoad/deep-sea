using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.State
{
    [Serializable]
    public class ObservableProperty<TType>
    {
        [SerializeField] private TType _value;

        public TType Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        public PropertyEvent<TType> OnChanged = new PropertyEvent<TType>();
    }

    public static class PropertyExtension
    {
        public static Subscription SubscribeAndFire<TType>(this ObservableProperty<TType> prop,
            UnityAction<TType> callback)
        {
            var subscition = prop.Subscribe(callback);
            callback(prop.Value);
            return subscition;
        }

        public static Subscription Subscribe<TType>(this ObservableProperty<TType> prop,
            UnityAction<TType> callback)
        {
            prop.OnChanged.AddListener(callback);
            return new Subscription(() => prop.OnChanged.RemoveListener(callback));
        }
    }

    public class Disposables : IDisposable
    {
        private List<IDisposable> _disposables = new List<IDisposable>();

        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }
    }

    public class Subscription : IDisposable
    {
        private readonly Action _call;

        public Subscription(Action call)
        {
            _call = call;
        }

        public void Dispose()
        {
            _call();
        }
    }

    [Serializable]
    public class PropertyEvent<TType> : UnityEvent<TType>
    {
    }

    [Serializable]
    public class IntProperty : ObservableProperty<int>
    {
    }
}