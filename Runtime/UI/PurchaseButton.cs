using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AWP
{
    public class PurchaseButton<T> : ButtonVariant where T : IComparable
    {
        [SerializeField]
        private CurrencyVariable<T> _currency;
        [SerializeField]
        protected T _cost;
        [SerializeField]
        protected UnityEvent _onPurchaseSucceed;
        [SerializeField]
        protected UnityEvent _onPurchaseFail;

        public T Cost { get { return _cost; } set { _cost = value; } }
        public bool CanAfford => _currency.CanAfford(Cost);

        protected override void OnEnable()
        {
            base.OnEnable();

            SyncVisuals();
            _currency.OnValueChanged += SyncVisuals;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _currency.OnValueChanged -= SyncVisuals;
        }

        protected override void OnPress()
        {
            CheckToPurchase();
        }

        protected void CheckToPurchase()
        {
            if (_currency.AttemptPurchase(Cost))
            {
                OnPurchase();
            }
            else
            {
                _onPurchaseFail?.Invoke();
            }
        }

        protected virtual void OnPurchase()
        {
            _onPurchaseSucceed?.Invoke();
            SyncVisuals();
        }

        protected virtual void SyncVisuals()
        {
            _button.interactable = CanAfford;
        }
    }
}
