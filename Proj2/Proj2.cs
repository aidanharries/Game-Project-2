using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Proj2
{
    public class Proj2 : Game
    {
        public static int ScreenWidth => 1920; 
        public static int ScreenHeight => 1080;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MainMenu _mainMenu;
        private GameState _currentState;

        public Proj2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsFixedTimeStep = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            _currentState = GameState.MainMenu; // Start at main menu
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mainMenu = new MainMenu(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    if (_mainMenu.Update(gameTime))
                    {
                        _currentState = GameState.Gameplay;
                    }
                    break;

                case GameState.Gameplay:
                    // Gameplay update logic (currently empty)
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    _mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.Gameplay:
                    // Gameplay draw logic (currently empty)
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}