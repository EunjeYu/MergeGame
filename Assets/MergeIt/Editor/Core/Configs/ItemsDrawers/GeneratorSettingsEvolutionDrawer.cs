// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using MergeIt.Editor.EvolutionsEditor;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs.ItemsDrawers
{
    public class GeneratorSettingsEvolutionDrawer : GeneratorSettingsBaseDrawer
    {
        private readonly EvolutionsGraphView _graphView;
        
        private GeneratedItemsCreator _creator;

        public GeneratorSettingsEvolutionDrawer(VisualElement parent, ElementConfig config, EvolutionsGraphView evolutionsGraphView) 
            : base(parent, config)
        {
            _graphView = evolutionsGraphView;
        }

        public override void Draw()
        {
            base.Draw();
            
            var genItemsContainer = Root.Q<VisualElement>("GenItemsContainer");
            var genItemsControls = Root.Q<VisualElement>("GeneratedItemsControls");
            _creator = new GeneratedItemsCreator(genItemsControls, genItemsContainer, Config, _graphView.EvolutionsData, _graphView.Configs);
            _creator.Generate();
        }

        protected override void OnClear()
        {
            base.OnClear();
            
            _creator.Dispose();
        }
    }
}