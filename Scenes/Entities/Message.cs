using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace MemoryMatch.Scenes.Entities
{
    class Message : Entity
    {
        public Message() 
        {
            Name = "header";
            Position = new Vector2(160, 75);
            TextComponent r = AddComponent<TextComponent>();
            r.SetFont(new NezSpriteFont(Content.Load<SpriteFont>("message")));
            r.SetHorizontalAlign(HorizontalAlign.Center);
            r.Color = Color.Black;
            r.Text = "Instructions:";
            r.Text += "\nUse arrow keys and spacebar\nto find matching pairs\nPress Esc to quit";

        }

        
    }
}
