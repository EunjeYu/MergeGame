// Copyright (c) 2024, Awessets

using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace MergeIt.Editor.EvolutionsEditor
{
    public class EvolutionsGraphViewTraits : GraphView.UxmlTraits
    {
        private readonly UxmlStringAttributeDescription _nameAttribute = new()
            {name = "name"};
        
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            if (ve is EvolutionsGraphView evolutionsGraphView)
            {
                evolutionsGraphView.name = _nameAttribute.GetValueFromBag(bag, cc);
            }
        }
    }
}