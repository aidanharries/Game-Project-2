/*********************************************************************************************
 * MainMenu.cs
 * 
 * Created by: Aidan Harries
 * Date: 9/29/23
 * Project: Proj2
 * 
 * Description: Represents the main menu screen for the game. Handles input and drawing of the
 * main menu elements.
 * 
 ********************************************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Proj2
{
    public class MainMenu
    {
        // Textures and fonts
        private Texture2D _background;
        private SpriteFont _font;
        private SpriteFont _largeFont;

        // Text displayed on the main menu
        private string _titleText = "PROXIMA CENTAURI";
        private string _promptText = "Press Enter to Play";

        // Positioning for texts
        private Vector2 _titlePosition;
        private Vector2 _promptPosition;

        // Oscillation properties for the prompt text animation
        private float _promptBaselineY;
        private float _oscillationSpeed = 2.0f;

        // Tracking if Enter was pressed and its timestamp
        private bool _enterPressed = false;
        private TimeSpan _enterPressTime;

        // Properties for the fade transition
        private float _fadeValue = 0f; // The fade opacity, from 0 to 1
        private TimeSpan _fadeDuration = TimeSpan.FromSeconds(1); // Duration of the fade

        // Sound effect played when player selects to play
        private SoundEffect playSound;

        /// <summary>
        /// Initializes the main menu, loading graphics and positioning elements.
        /// </summary>
        /// <param name="content">The content manager to load resources.</param>
        public MainMenu(ContentManager content)
        {
            // Load graphics resources.
            _background = content.Load<Texture2D>("background");
            _font = content.Load<SpriteFont>("LanaPixel");
            _largeFont = content.Load<SpriteFont>("LanaPixelLarge");

            // Center text elements on the screen.
            _titlePosition = new Vector2(
                (Proj2.ScreenWidth - _largeFont.MeasureString(_titleText).X) / 2,
                Proj2.ScreenHeight * 4 / 16
            );
            _promptPosition = new Vector2(
                (Proj2.ScreenWidth - _font.MeasureString(_promptText).X) / 2,
                Proj2.ScreenHeight * 8 / 16
            );
            _promptBaselineY = _promptPosition.Y;

            // Load sound effect.
            playSound = content.Load<SoundEffect>("Play");
        }

        /// <summary>
        /// Updates the main menu elements. Handles user input and animations.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <returns>True if the transition to the game should occur, false otherwise.</returns>
        public bool Update(GameTime gameTime)
        {
            if (!_enterPressed)
            {
                // Oscillate the prompt position for animation.
                _promptPosition.Y = _promptBaselineY + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * _oscillationSpeed) * 20.0f;
            }

            // If Enter is pressed, set to start the game.
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_enterPressed)
            {
                playSound.Play(0.25f, 0.0f, 0.0f);
                _enterPressed = true;
                _enterPressTime = gameTime.TotalGameTime;
            }

            if (_enterPressed)
            {
                // Handle fading effect when transitioning.
                float fadeProgress = (float)(gameTime.TotalGameTime - _enterPressTime).TotalSeconds / (float)_fadeDuration.TotalSeconds;
                _fadeValue = MathHelper.Clamp(fadeProgress, 0f, 1f);

                // If fade is complete, start the game.
                if (_fadeValue >= 1f)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Draws the main menu elements.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background.
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            // Text color definitions.
            Color black = new Color(0, 0, 0);
            Color darkBlue = new Color(0, 150, 255); // Dark blue for the outline effect

            // Thickness of the text outline.
            float outlineThickness = 2.0f;

            // Draw outline of the title.
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(-outlineThickness, 0), darkBlue);  // Left
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(outlineThickness, 0), darkBlue);  // Right
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(0, -outlineThickness), darkBlue);  // Up
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(0, outlineThickness), darkBlue);  // Down
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(-outlineThickness, -outlineThickness), darkBlue);  // Up-Left
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(outlineThickness, -outlineThickness), darkBlue);  // Up-Right
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(-outlineThickness, outlineThickness), darkBlue);  // Down-Left
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(outlineThickness, outlineThickness), darkBlue);  // Down-Right

            // Draw the title and prompt texts.
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition, black);
            Color promptColor = _enterPressed ? Color.Gold : Color.White;
            spriteBatch.DrawString(_font, _promptText, _promptPosition, promptColor);

            // If Enter was pressed, draw fading effect.
            if (_enterPressed)
            {
                Color fadeColor = new Color(0, 0, 0, _fadeValue);
                spriteBatch.Draw(_background, new Rectangle(0, 0, Proj2.ScreenWidth, Proj2.ScreenHeight), fadeColor);
            }

        }
    }
}
