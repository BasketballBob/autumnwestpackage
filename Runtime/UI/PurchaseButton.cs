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
            bool canAfford = _currency.CanAfford(Cost);
            _button.interactable = canAfford;
        }
    }
}
