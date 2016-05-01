﻿using Microsoft.Xna.Framework;
using Rampastring.Tools;
using System;

namespace Rampastring.XNAUI.DXControls
{
    /// <summary>
    /// A static label control.
    /// </summary>
    public class DXLabel : DXControl
    {
        public DXLabel(WindowManager windowManager) : base(windowManager)
        {
            RemapColor = UISettings.TextColor;
        }

        public int FontIndex { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            if (!String.IsNullOrEmpty(Text))
            {
                Vector2 textSize = Renderer.GetTextDimensions(Text, FontIndex);
                ClientRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, (int)textSize.X, (int)textSize.Y);
            }
        }

        protected override void ParseAttributeFromINI(IniFile iniFile, string key, string value)
        {
            switch (key)
            {
                case "FontIndex":
                    FontIndex = Utilities.IntFromString(value, 0);
                    return;
            }

            base.ParseAttributeFromINI(iniFile, key, value);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!String.IsNullOrEmpty(Text))
                Renderer.DrawStringWithShadow(Text, FontIndex, new Vector2(GetLocationX(), GetLocationY()), GetRemapColor());
        }
    }
}
