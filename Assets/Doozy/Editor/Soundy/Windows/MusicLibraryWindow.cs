﻿// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Doozy.Editor.EditorUI.Components;
using Doozy.Editor.EditorUI.Components.Internal;
using Doozy.Editor.EditorUI.Windows.Internal;
using Doozy.Editor.Soundy.Layouts;
using Doozy.Editor.UIElements;
using Doozy.Runtime.UIElements.Extensions;
using UnityEngine;

namespace Doozy.Editor.Soundy.Windows
{
    public class MusicLibraryWindow : FluidWindow<MusicLibraryWindow>
    {
        private const string k_WindowTitle = "Music Libraries";
        
        public static void Open() => InternalOpenWindow(k_WindowTitle);
        
        protected override void CreateGUI()
        {
            windowLayout = new MusicLibraryRegistryWindowLayout();
            ((FluidWindowLayout)windowLayout)?.OnEnable();
            root
                .RecycleAndClear()
                .AddChild(windowLayout);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            minSize = new Vector2(500, 500);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ((FluidWindowLayout)windowLayout).OnDisable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var layout = (FluidWindowLayout)windowLayout;
            if (layout == null) return;
            layout?.OnDestroy();
            layout?.Dispose();
        }

        public MusicLibraryWindow SelectLibrary(string libraryName)
        {
            MusicLibraryRegistryWindowLayout layout = (MusicLibraryRegistryWindowLayout)windowLayout;
            layout.schedule.Execute(() =>
            {
                foreach (FluidToggleButtonTab button in layout.sideMenu.buttons)
                {
                    if (!button.buttonLabel.text.Equals(libraryName))
                        continue;
                    button.SetIsOn(true);
                    return;
                }
            }).ExecuteLater(100);
            return this;
        }
    }
}
