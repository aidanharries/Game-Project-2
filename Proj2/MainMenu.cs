using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Proj2
{
    internal class MainMenu
    {
        private Texture2D _background;
        private SpriteFont _font;
        private SpriteFont _largeFont;

        private string _titleText = "ASTEROID BLAST";
        private string _promptText = "Press Enter to Play";

        private Vector2 _titlePosition;
        private Vector2 _promptPosition;

        private float _promptBaselineY;
        private float _oscillationSpeed = 2.0f;

        private bool _enterPressed = false;
        private TimeSpan _enterPressTime;
        private TimeSpan _delayDuration = TimeSpan.FromSeconds(1);

        public MainMenu(ContentManager content)
        {
            _background = content.Load<Texture2D>("background");
            _font = content.Load<SpriteFont>("LanaPixel");
            _largeFont = content.Load<SpriteFont>("LanaPixelLarge");

            // Calculate positions so text is centered on the screen
            _titlePosition = new Vector2(
                (Proj2.ScreenWidth - _largeFont.MeasureString(_titleText).X) / 2,
                Proj2.ScreenHeight * 4 / 16
            );
            _promptPosition = new Vector2(
                (Proj2.ScreenWidth - _font.MeasureString(_promptText).X) / 2,
                Proj2.ScreenHeight * 8 / 16
            );
            _promptBaselineY = _promptPosition.Y;
        }

        public bool Update(GameTime gameTime)
        {
            if (!_enterPressed)
            {
                // Update position only if Enter has not been pressed
                _promptPosition.Y = _promptBaselineY + (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * _oscillationSpeed) * 20.0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_enterPressed)
            {
                _enterPressed = true;
                _enterPressTime = gameTime.TotalGameTime;
            }

            if (_enterPressed && gameTime.TotalGameTime - _enterPressTime > _delayDuration)
            {
                return true; // proceed to the next screen after a delay
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);

            Color black = new Color(0, 0, 0);
            Color darkBlue = new Color(0, 150, 255); // Dark blue for the outline effect
            float outlineThickness = 2.0f;

            // Draw outline (8 surrounding positions)
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(-outlineThickness, 0), darkBlue);  // Left
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(outlineThickness, 0), darkBlue);  // Right
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(0, -outlineThickness), darkBlue);  // Up
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(0, outlineThickness), darkBlue);  // Down
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(-outlineThickness, -outlineThickness), darkBlue);  // Up-Left
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(outlineThickness, -outlineThickness), darkBlue);  // Up-Right
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(-outlineThickness, outlineThickness), darkBlue);  // Down-Left
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition + new Vector2(outlineThickness, outlineThickness), darkBlue);  // Down-Right

            // Draw main text on top with black
            spriteBatch.DrawString(_largeFont, _titleText, _titlePosition, black);

            Color promptColor = _enterPressed ? Color.Gold : Color.White;

            spriteBatch.DrawString(_font, _promptText, _promptPosition, promptColor);

        }
    }
}
