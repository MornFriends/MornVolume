using System;
using UnityEngine;

namespace MornVolume
{
    internal class MornVolumeTrigger : MonoBehaviour
    {
        private Action _onStart;

        private void Start()
        {
            _onStart?.Invoke();
            Destroy(gameObject);
        }

        internal void Initialize(Action onStart)
        {
            _onStart = onStart;
        }
    }
}