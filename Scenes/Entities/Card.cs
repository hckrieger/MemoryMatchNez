using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;

namespace MemoryMatch.Scenes.Entities
{
    class Card : Entity
    {

        public Color CardColor { get; set; }

        public bool FacedUp { get; set; } = false;

        public bool Matched { get; set; } = false;
             
        public Card()
        {
            AddComponent(new SpriteRenderer(Content.Load<Texture2D>("card")));
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.Origin = new Vector2(-7.5f, -7.5f);
        }

        public override void Update()
        {
            base.Update();

            if (FacedUp)
            {
                GetComponent<SpriteRenderer>().Color = CardColor;
            } else
            {
                GetComponent<SpriteRenderer>().Color = Color.White;
            }

            if (Matched)
            {
                FacedUp = true;
            }
        }
    }
}
