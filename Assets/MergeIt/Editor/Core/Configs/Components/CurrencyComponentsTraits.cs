// Copyright (c) 2024, Awessets

using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs
{
    public class CurrencyComponentsTraits : UxmlTraits
    {
        private readonly UxmlStringAttributeDescription _nameAttribute = new()
            {name = "name"};

        private readonly UxmlStringAttributeDescription _titleAttribute = new()
            {name = "title", defaultValue = "Currency parameters"};

        private readonly UxmlStringAttributeDescription _currencyTypeBindingAttribute = new()
            {name = "type-binding"};

        private readonly UxmlStringAttributeDescription _currencyAmountBindingAttribute = new()
            {name = "amount-binding"};

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            if (ve is CurrencyComponent currencyParameters)
            {
                currencyParameters.name = _nameAttribute.GetValueFromBag(bag, cc);
                currencyParameters.Title = _titleAttribute.GetValueFromBag(bag, cc);
                currencyParameters.TypeBinding = _currencyTypeBindingAttribute.GetValueFromBag(bag, cc);
                currencyParameters.AmountBinding = _currencyAmountBindingAttribute.GetValueFromBag(bag, cc);
            }
        }
    }
}