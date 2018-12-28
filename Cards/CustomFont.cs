﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using System.IO;

namespace Cards
{
    public class CustomFont
    {
        private PrivateFontCollection Fonts = new PrivateFontCollection();

        public CustomFont()
        {
            if (File.Exists("5x3.ttf"))
                Fonts.AddFontFile("5x3.ttf");
        }

        public System.Drawing.Font GetFont(float fontSize)
        {
            return new System.Drawing.Font(Fonts.Families[0], fontSize);
        }
    }
}
