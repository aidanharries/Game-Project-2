/*********************************************************************************************
 * Gameplay.cs
 * 
 * Created by: Aidan Harries
 * Date: 9/29/23
 * Project: Proj2
 * 
 * Description: Manages the core gameplay elements including player's ship, asteroids, 
 * and their interactions. It also handles background rendering and the fading effect 
 * when transitioning into the gameplay screen.
 * 
 ********************************************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Proj2
{
    public class Gameplay
    {
        // Background texture for gameplay screen
        private Texture2D _background;

        // Fade effect properties for transitioning into the gameplay screen
        private float _fadeAlpha = 1.0f; 
        private float _fadeSpeed = 0.02f;

        // Game elements
        private PlayerShip _playerShip;
        private Asteroid _asteroid;

        private Game gameInstance;
        private ContentManager contentManager;

        /// <summary>
        /// Initializes gameplay elements and loads necessary content.
        /// </summary>
        /// <param name="content">The content manager to load game assets.</param>
        /// <param name="game">Reference to the main game instance.</param>
        public Gameplay(ContentManager content, Game game)
        {
            _background = content.Load<Texture2D>("background");

            _playerShip = new PlayerShip(game);
            _playerShip.LoadContent(content);

            _asteroid = new Asteroid(game);
            _asteroid.LoadContent(content);

            this.gameInstance = game;
            this.contentManager = content;
        }

        /// <summary>
        /// Updates game elements such as player ship, asteroids, and checks for collisions.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // Manage the fade-in effect
            if (_fadeAlpha > 0)
            {
                _fadeAlpha -= _fadeSpeed;
                _fadeAlpha = MathHelper.Clamp(_fadeAlpha, 0f, 1f);
            }

            // Update individual game elements
            _playerShip.Update(gameTime);
            _asteroid.Update(gameTime);

            // Check for laser-asteroid collisions
            Laser collidedLaser = null;
            foreach (var laser in _playerShip.GetLasers())
            {
                if (_asteroid.CheckCollision(laser))
                {
                    collidedLaser = laser;
                    break;
                }
            }

            if (collidedLaser != null)
            {
                _playerShip.RemoveLaser(collidedLaser);

                // Check if already exploding or invulnerable
                if (!_asteroid.isExploding && !_asteroid.isInvulnerable)
                {
                    _asteroid.isInvulnerable = true; // Make the asteroid invulnerable to further hits
                    _asteroid.isExploding = true;  // Start the explosion animation
                    _asteroid.currentFrame = 0;  // Reset the current frame
                    _asteroid.timeSinceLastFrame = 0;  // Reset the time counter

                    // Stop the asteroid from moving
                    _asteroid.SetVelocity(Vector2.Zero);
                }
            }

            // Check for player-asteroid collisions
            if (_asteroid.CheckCollision(_playerShip))
            {
                // Check if already exploding or invulnerable
                if (!_asteroid.isExploding && !_asteroid.isInvulnerable)
                {
                    // Set the ship to be red
                    _playerShip.SetToBeRed();

                    _asteroid.isInvulnerable = true; // Make the asteroid invulnerable to further hits
                    _asteroid.isExploding = true;    // Start the explosion animation
                    _asteroid.currentFrame = 0;      // Reset the current frame
                    _asteroid.timeSinceLastFrame = 0;  // Reset the time counter

                    // Stop the asteroid from moving
                    _asteroid.SetVelocity(Vector2.Zero);
                }
            }

            // Reset the asteroid if marked for removal
            if (_asteroid.toBeRemoved)
            {
                _asteroid = new Asteroid(gameInstance);
                _asteroid.LoadContent(contentManager);
            }
        }

        /// <summary>
        /// Renders game elements on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw background and elements in the correct order
            spriteBatch.Draw(_background, new Rectangle(0, 0, Proj2.ScreenWidth, Proj2.ScreenHeight), Color.White);

            if (_fadeAlpha > 0)
            {
                Color fadeColor = new Color(0, 0, 0, _fadeAlpha);
                spriteBatch.Draw(_background, new Rectangle(0, 0, Proj2.ScreenWidth, Proj2.ScreenHeight), fadeColor);
            }

            _playerShip.Draw(gameTime, spriteBatch);
            _asteroid.Draw(spriteBatch);
        }
    }
}
