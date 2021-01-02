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

        float unmatchedTimer = .6f;

        bool disableSelecting = false;

        int colorIndx;
        int matchCount;

        public static bool completed = false;

        Entity header, message;

        int tries;

        public override void Initialize()
        {
            base.Initialize();

            //Set size of screen and resolution policy for full screen
            SetDesignResolution(300, 500, SceneResolutionPolicy.BestFit);
            Screen.SetSize(300, 500);

            header = new Header();
            AddEntity(header); //Add header text to list of entities

            message = new Message();
            AddEntity(message); //Add message text to list of entities. 

            Entity field = CreateEntity("grid", new Vector2(Screen.Center.X - 120, (Screen.Center.Y * 1.3f) - 150)); //create the grid entity     
            AddEntity(selector); //Add selector entity
            selector.SetParent(field); //Set the grid area as the parent of the selector

            //Add the array of 8 colors to the list twice to make sure their are two of each to account for pairs
            for (int i = 0; i < colors.Length; i++) { colorList.Add(colors[i]); }
            for (int i = 0; i < colors.Length; i++) { colorList.Add(colors[i]); }

            grid = new Card[Width, Height]; //Set the number of cards 4x4 in dimensional array

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    grid[x, y] = new Card(); //Assign object to each card to the multi-dimensional card array to represent a grid
                    AddEntity(grid[x, y]); //Add the cards as Entities to the scene
                    grid[x, y].Name = $"card{x}x{y}"; //Assign a name to the entity of each card
                    grid[x, y].SetParent(field); //Set the grid as the parent to each card
                    grid[x, y].LocalPosition = new Vector2(x * 60, y * 75); //Set the position of each card
                }
            }
            Reset(); //See reset function below that stores default values
        }


        public override void Update()
        {
            base.Update();

            for (int x = 0; x < Width; x++) //Positioning Selector and update for it's position
            {
                for (int y = 0; y < Height; y++)
                { //Put the selector on a card on the grid
                    selector.LocalPosition = grid[selectorXIndex, selectorYIndex].LocalPosition; 
                }
            }

            UserInput(); //See method below
            CardComparingLogic(); //See method below
        }

        public void Reset()  //Reset the values and shuffle the color order after finishing the game 
        {
            colorList.Shuffle<Color>(); //Shuffle the order that the colored pairs are in for every game
            colorIndx = 0; //For interating through the list of colors in the for loop setting the grid
            matchCount = 0; //Reset to zero matches....
            tries = 0; //...and zero tries for a new game

            //Default text while game is being played before completion
            header.GetComponent<TextComponent>().Text = "Memory Match";
            message.GetComponent<TextComponent>().Text = "Instructions: \nUse arrow keys and spacebar \nto find matching pairs \nPress Esc to quit";

            for (int x = 0; x < Width; x++) 
            {
                for (int y = 0; y < Height; y++)
                {
                    //Make all the cards faced down and unmatched
                    grid[x, y].FacedUp = false;  
                    grid[x, y].Matched = false;

                    //When iterating through the list of colors
                    grid[x, y].CardColor = colorList[colorIndx];
                    colorIndx++;
                }
            }
        }

        public void UserInput()
        {
            if (Input.IsKeyPressed(Keys.Enter) && completed) //When the game is completeing then press enter to reset and play again
            {
                Reset();
                completed = false;
            }

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
        }

        public void CardComparingLogic()
        {
            if (cardsToCompare.Count == 2) //If two card's are being compared
            {
                if (cardsToCompare[0].CardColor == cardsToCompare[1].CardColor) //And if the card's color's match
                {
                    cardsToCompare[0].Matched = true; //Keep the paired cards faced up
                    cardsToCompare[1].Matched = true; //,,,,,,,,,,,,,,,,,,,,,
                    matchCount++; //Add to the number of matches
                    tries++; //Add one try
                    cardsToCompare.Clear(); //And clear those card's on the list of card's to be compared
                }
                else // If the card's colors don't match
                {
                    disableSelecting = true; //Bool temporarily disables spacebar input while timer is running
                    unmatchedTimer -= Time.DeltaTime; //run the timer that keeps the unmatched cards up for a time

                    if (unmatchedTimer <= 0) //When the time runs out
                    {
                        unmatchedTimer = .6f; //Reset the timer value
                        cardsToCompare[0].FacedUp = false; //turn the formerly unmatched cards faced down
                        cardsToCompare[1].FacedUp = false; //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
                        disableSelecting = false; //Activate selecting input since timer has stopped and reset
                        tries++; //Add one try
                        cardsToCompare.Clear(); //and clear them from the list of card's being compared
                    }
                }

                if (matchCount == 8) //When you find all the matches then display the text saying so and set the completed bool to true to activate reseting
                {
                    completed = true;
                    header.GetComponent<TextComponent>().Text = "Good Job!";
                    message.GetComponent<TextComponent>().Text = $"You found all the pairs\nin {tries} tries\nPress Enter to play again\nPress Esc to quit";
                }
            }
        }


    }
}
