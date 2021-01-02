using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace MemoryMatch.Scenes.Entities
{
    class Header : Entity
    {
        public TextComponent l;
        public Header() 
        {
            Name = "header";
            Position = new Vector2(Screen.Center.X, 35);
            
            l = AddComponent<TextComponent>();
            l.SetFont(new NezSpriteFont(Content.Load<SpriteFont>("title")));
            l.HorizontalOrigin = HorizontalAlign.Center;
            l.VerticalOrigin = VerticalAlign.Center;
            l.Color = Color.Black;
            l.Text = "Memory Match";

        }


    }
}
