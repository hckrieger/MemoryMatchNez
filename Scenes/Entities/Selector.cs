using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;

namespace MemoryMatch.Scenes.Entities
{
    class Selector : Entity
    {
        public Selector()
        {
            Name = "selector";
            UpdateOrder = 1;
          
            AddComponent(new SpriteRenderer(Content.Load<Texture2D>("selector")));
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.Origin = new Vector2(-7.5f, -7.5f);
        }
    }
}
