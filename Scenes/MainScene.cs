using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MemoryMatch.Scenes.Entities;
using Nez.Sprites;
using Nez.Textures;
using Nez;

namespace MemoryMatch.Scenes
{
    class MainScene : Scene
    {
        Selector selector = new Selector();
        Card[,] grid;

        Color[] colors = { Color.Red, Color.Orange, Color.Yellow, Color.Green,
                               Color.Blue, Color.LightGreen, Color.Indigo, Color.Violet };

        List<Color> colorList = new List<Color>();

        int Width = 4;
        int Height = 4;

        int selectorYIndex = 0;
        int selectorXIndex = 0;


        List<Card> cardsToCompare = new List<Card>();

        float timer = .6f;

        bool disableSelecting = false;

        int colorIndx = 0;
        int matchCount = 0;

        bool completed = false;

       

        public override void Initialize()
        {

            base.Initialize();


            SetDesignResolution(300, 500, SceneResolutionPolicy.BestFit);
            Screen.SetSize(300, 500);

            Entity title = CreateEntity("title", new Vector2(Screen.Center.X, 35));
            TextComponent l = title.AddComponent<TextComponent>();
            l.SetFont(new NezSpriteFont(Content.Load<SpriteFont>("title")));
            l.HorizontalOrigin = HorizontalAlign.Center;
            l.VerticalOrigin = VerticalAlign.Center;
            l.Color = Color.Black;
            l.Text = "Memory Match";

            Entity message = CreateEntity("message", new Vector2(160, 75));
            TextComponent r = message.AddComponent<TextComponent>();
            r.SetFont(new NezSpriteFont(Content.Load<SpriteFont>("message")));
            r.SetHorizontalAlign(HorizontalAlign.Center);
            r.Color = Color.Black;
            r.Text = "Instructions:";
            r.Text += "\nUse arrow keys and spacebar\nto find matching pairs\nPress Esc to quit";



            Entity field = CreateEntity("grid", new Vector2(Screen.Center.X - 120, (Screen.Center.Y * 1.3f) - 150)); //create the grid entity     
            AddEntity(selector); //Add selector entity
            selector.SetParent(field);


            //Add the array of 8 colors to the list twice to make sure their are two of each to account for pairs
            for (int i = 0; i < colors.Length; i++) { colorList.Add(colors[i]); }
            for (int i = 0; i < colors.Length; i++) { colorList.Add(colors[i]); }

            //shuffle the list every time so the cards have different colors every time
            colorList.Shuffle<Color>();

            grid = new Card[Width, Height];
        
            for (int x = 0; x < Width; x++)
            {
                
                for (int y = 0; y < Height; y++)
                {
                    grid[x, y] = new Card(); //Assign object to each card to the multi-dimensional card array to represent a grid
                    AddEntity(grid[x, y]); //Add the cards as Entities to the scene

                    grid[x, y].Name = $"card{x}x{y}"; //Assign a name to the entity of each card
                    grid[x, y].SetParent(field); //Set the grid as the parent to each card
                    grid[x, y].LocalPosition = new Vector2(x * 60, y * 75); //Set the position of each card
                    
                    grid[x, y].CardColor = colorList[colorIndx];
                    colorIndx++;

                }
                
            }


        }


        public override void Update()
        {
            base.Update();
        
        //Positioning Selector
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    //Put the selector on a card on the grid
                    selector.LocalPosition = grid[selectorXIndex, selectorYIndex].LocalPosition; 

                    if (matchCount == 8)
                    {
                        completed = true;
                    }
                }
            }
        //

            if (Input.IsKeyPressed(Keys.Enter) && completed)
            {
                
                Reset();
                completed = false;

            }

        //Input Logic
            // Move the position of the selector using the arrow keys and keep them in range
            if (Input.IsKeyPressed(Keys.Down) && selectorYIndex < 3) { selectorYIndex++; } 
            if (Input.IsKeyPressed(Keys.Up) && selectorYIndex > 0) { selectorYIndex--; }
            if (Input.IsKeyPressed(Keys.Left) && selectorXIndex > 0) { selectorXIndex--; }
            if (Input.IsKeyPressed(Keys.Right) && selectorXIndex < 3) { selectorXIndex++; }

            //If space button is pressed while the timer that keeps unmatched card's faced up is not running
            if (Input.IsKeyPressed(Keys.Space) && !disableSelecting)
            {                
                var selectedCard = grid[selectorXIndex, selectorYIndex]; //set the selected card
                
                if (selectedCard.FacedUp == false) //If the selected card is faced down
                {                    
                    selectedCard.FacedUp = true; //Show the card's color
                    cardsToCompare.Add(selectedCard); //and add the card to the list of card's being compared
                }
            }
        //
        


        //Card Comparing Logic
            if (cardsToCompare.Count == 2) //If two card's are being compared
            {              
                if (cardsToCompare[0].CardColor == cardsToCompare[1].CardColor) //And if the card's color's match
                {                   
                    cardsToCompare[0].Matched = true; //Keep the paired cards faced up
                    cardsToCompare[1].Matched = true; //,,,,,,,,,,,,,,,,,,,,,
                    matchCount++;
                    cardsToCompare.Clear(); //And clear those card's on the list of card's to be compared
                }
                else // If the card's colors don't match
                {
                    disableSelecting  = true; //Bool temporarily disables spacebar input while timer is running
                    timer -= Time.DeltaTime; //run the timer that keeps the unmatched cards up for a time

                    if (timer <= 0) //When the time runs out
                    {
                        timer = .6f; //Reset the timer value
                        cardsToCompare[0].FacedUp = false; //turn the formerly unmatched cards faced down
                        cardsToCompare[1].FacedUp = false; //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
                        disableSelecting = false; //Activate selecting input since timer has stopped and reset

                        cardsToCompare.Clear(); //and clear them from the list of card's being compared
                    }

                }
           
            } 
        }

        public void Reset()  //Reset the values and shuffle the color order after finishing the game 
        {
            colorList.Shuffle<Color>();
            colorIndx = 0;
            matchCount = 0;
            for (int x = 0; x < Width; x++) 
            {

                for (int y = 0; y < Height; y++)
                {
                    grid[x, y].FacedUp = false;
                    grid[x, y].Matched = false;

                    grid[x, y].CardColor = colorList[colorIndx];
                    colorIndx++;

                }

            }
            
        }


    }
}
