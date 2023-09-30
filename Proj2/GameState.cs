/*********************************************************************************************
 * GameState.cs
 * 
 * Created by: Aidan Harries
 * Date: 9/29/23
 * Project: Proj2
 * 
 * Description: Enumeration that represents the different states the game can be in. Used to 
 * manage transitions between game screens and handle logic based on the current game state.
 * 
 ********************************************************************************************/

namespace Proj2
{
    /// <summary>
    /// Enum defining possible states of the game.
    /// </summary>
    public enum GameState
    {
        MainMenu,   // Represents the main menu screen of the game.
        Gameplay    // Represents the gameplay screen where the main game actions take place.
    }
}
