// Copyright (c) 2024, Awessets

using MergeIt.Core.Configs.Elements;
using UnityEngine.UIElements;

namespace MergeIt.Editor.Configs.ItemsDrawers
{
    public class GeneratorSettingsInspectorDrawer : GeneratorSettingsBaseDrawer
    {
        public GeneratorSettingsInspectorDrawer(VisualElement parent, ElementConfig config) : base(parent, config)
        {
        }

        public override void Draw()
        {
            base.Draw();

            var cantGenerateSection = Root.Q<VisualElement>("CantGenerateSection");
            var helpBox = new HelpBox("Editing this section is available only from the 'Evolutions editor', " +
                                      "that you can find in the menu 'Window/Merge Toolkit/Evolutions editor'", HelpBoxMessageType.Info);
            
            cantGenerateSection.Add(helpBox);
        }
    }
}