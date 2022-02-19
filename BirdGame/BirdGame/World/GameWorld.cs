namespace BirdGame.World
{
    using BirdGame.AI;
    using BirdGame.Audio;
    using BirdGame.Characters;
    using BirdGame.Data;
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Input;
    using BirdGame.UI;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The game world.
    /// </summary>
    internal class GameWorld
    {
        /// <summary>
        /// The bounds the bird can move within.
        /// </summary>
        public static Rectangle WorldBounds = new Rectangle(96, 96, 928, 928);

        /// <summary>
        /// The maximum number of characters.
        /// </summary>
        private const int MaximumCharacters = 60;

        /// <summary>
        /// The intro text UI.
        /// </summary>
        private readonly IntroText introText;

        /// <summary>
        /// The score counter.
        /// </summary>
        private readonly ScoreCounter scoreCounter;

        /// <summary>
        /// The bird.
        /// </summary>
        private readonly Bird bird;

        /// <summary>
        /// The list of characters.
        /// </summary>
        private readonly List<AbstractCharacter> characters;

        /// <summary>
        /// The list of drones.
        /// </summary>
        private readonly List<Drone> drones;

        /// <summary>
        /// The list of poops.
        /// </summary>
        private readonly List<Poop> poops;

        /// <summary>
        /// The pause screen.
        /// </summary>
        private readonly PauseScreen pauseScreen;

        /// <summary>
        /// The camera.
        /// </summary>
        private readonly Camera camera;

        /// <summary>
        /// The texture for the town.
        /// </summary>
        private readonly Texture2D town;

        /// <summary>
        /// The position to draw the town at.
        /// </summary>
        private readonly Vector2 townPosition;

        /// <summary>
        /// The score screen.
        /// </summary>
        private ScoreScreen scoreScreen;

        /// <summary>
        /// The state of the world.
        /// </summary>
        private WorldState state;

        /// <summary>
        /// The number of drones that are active.
        /// </summary>
        private int dronesActive;

        /// <summary>
        /// The maximum number of drones that are allowed.
        /// </summary>
        private int maxDronesAllowed;

        /// <summary>
        /// The matrix to scale the UI by based on the resolution.
        /// </summary>
        private Matrix uiMatrix;

        /// <summary>
        /// The constructor.
        /// </summary>
        public GameWorld()
        {
            // Set the state
            state = WorldState.Title;

            // Initialise the score counter and node network
            scoreCounter = ScoreCounter.Initialise();
            NodeNetwork.Initialise();

            introText = new IntroText();

            characters = new List<AbstractCharacter>();

            drones = new List<Drone>();

            poops = new List<Poop>();

            bird = new Bird();

            camera = new Camera(bird);

            pauseScreen = new PauseScreen();

            // Load the town texture
            town = WorldManager.ContentManager.Load<Texture2D>("Sprites\\town");

            // Move the town texture to start off screen because it contains non-playable areas
            townPosition = new Vector2(-512, -512);

            // Play the ambience and make it loop
            AudioManager.PlayLoopingSoundEffect("Ambience");

            // Populate the world
            PopulateWorldWithCharacters();

            maxDronesAllowed = 0;
            dronesActive = 0;

            // Update the matrix for drawing the UI
            UpdateUiMatrix();
        }

        /// <summary>
        /// A publically accessible property to get the camera.
        /// </summary>
        public Camera Camera => camera;

        /// <summary>
        /// A publically accessible property to get the bird.
        /// </summary>
        public Bird Bird => bird;

        /// <summary>
        /// Update the world.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        public void Update(GameTime gameTime)
        {
            // If the resolution has changed
            if (EventManager.EventFired(KnownEvents.ResolutionChanged))
            {
                // Update the UI matrix
                UpdateUiMatrix();
            }

            // Update based on the current state
            switch (state)
            {
                case WorldState.Title:
                    UpdateIntro(gameTime);
                    break;

                case WorldState.Playing:
                    UpdatePlaying(gameTime);
                    break;

                case WorldState.Paused:
                    UpdatePaused(gameTime);
                    break;

                case WorldState.Score:
                    UpdateScore(gameTime);
                    break;
            }
        }

        /// <summary>
        /// Draw the world.
        /// </summary>
        public void Draw()
        {
            // Create a sprite batch and position / scale it based on the camera
            WorldManager.SpriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: camera.Transform);

            // Draw the world
            DrawWorld();

            // Draw the world
            bird.Draw();

            // Stop the sprite batch
            WorldManager.SpriteBatch.End();

            // Start a new sprite batch for the UI
            WorldManager.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: uiMatrix);

            // Draw the UI based on the state
            switch (state)
            {
                case WorldState.Title:
                    introText.Draw();
                    break;

                case WorldState.Playing:
                    scoreCounter.Draw();
                    break;

                case WorldState.Paused:
                    pauseScreen.Draw();
                    break;

                case WorldState.Score:
                    scoreScreen.Draw();
                    break;
            }

            // Stop the sprite batch
            WorldManager.SpriteBatch.End();
        }

        /// <summary>
        /// Reset the world.
        /// </summary>
        private void Reset()
        {
            // Clear the lists
            characters.Clear();
            drones.Clear();
            poops.Clear();

            // Reset the camera and bird
            camera.Reset();
            bird.Reset(true);

            // Kill all the characters
            characters.ForEach(c => c.Kill());

            // Stop the looping sound for drones
            AudioManager.StopLoopingSoundEffect("DroneFly");

            // Populate the world with characters
            PopulateWorldWithCharacters();

            // Set the state
            state = WorldState.Playing;

            // Reset the number of drones
            maxDronesAllowed = 0;
            dronesActive = 0;

            scoreCounter.Reset();
        }

        /// <summary>
        /// Update the intro.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void UpdateIntro(GameTime gameTime)
        {
            // Update the intro text
            introText.Update();

            // If the spawn bird event has been fired
            if (EventManager.EventFired(KnownEvents.SpawnBird))
            {
                // Allow the bird to move
                bird.AllowMovement(true);
            }

            // If the intro has finished
            if (EventManager.EventFired(KnownEvents.IntroFinished))
            {
                // Allow the bird to be controlled
                bird.AllowControl();
                state = WorldState.Playing;
            }

            // Update the world
            UpdateWorld(gameTime);

            // Update the bird and camera
            bird.Update(gameTime);
            camera.Update(gameTime);
        }

        /// <summary>
        /// Update when the world is playable.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void UpdatePlaying(GameTime gameTime)
        {
            // If the pause button is pressed
            if (InputManager.IsBindingPressed(DefaultBindings.Pause))
            {
                // Reset the pause screen
                pauseScreen.Reset();

                // Set the state to be paused
                state = WorldState.Paused;
            }

            // If the bird is dead
            if (EventManager.EventFired(KnownEvents.BirdDead))
            {
                // Create a new score screen
                scoreScreen = new ScoreScreen(ScoreCounter.CurrentScore, SaveManager.SaveData.HighScore);

                // Update the high score and save it
                SaveManager.SaveData.HighScore = ScoreCounter.CurrentScore;
                SaveManager.Save();
                
                // Set the state to the score screen
                state = WorldState.Score;
            }

            // If a poop should be spawned
            if (EventManager.EventFiredThenKill(KnownEvents.PoopSpawned))
            {
                // Create a poop at the bird's position
                poops.Add(new Poop(bird.Position));
            }

            // Update the score counter
            scoreCounter.Update();

            // Update the world
            UpdateWorld(gameTime);

            // Update the bird and camera
            bird.Update(gameTime);
            camera.Update(gameTime);
        }

        /// <summary>
        /// Update the pause screen.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void UpdatePaused(GameTime gameTime)
        {
            // Update the pause screen
            pauseScreen.Update();

            // Update the camera
            camera.Update(gameTime);

            // If the pause button is pressed
            if (InputManager.IsBindingPressed(DefaultBindings.Pause))
            {
                // Save the options
                SaveManager.Save();

                // Return to playing
                state = WorldState.Playing;
            }
        }

        /// <summary>
        /// Update the score screen.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void UpdateScore(GameTime gameTime)
        {
            // If the player has pressed a button
            if (InputManager.IsBindingPressed(DefaultBindings.Poop)
                || InputManager.IsBindingPressed(DefaultBindings.Dive))
            {
                // Reset the world
                Reset();
            }

            // Update the world
            UpdateWorld(gameTime);
        }

        /// <summary>
        /// Update the world.
        /// </summary>
        /// <param name="gameTime">The time the game has been active.</param>
        private void UpdateWorld(GameTime gameTime)
        {
            // Create lists of characters and poops to remove
            List<AbstractCharacter> charactersToRemove = new List<AbstractCharacter>();
            List<Poop> poopsToRemove = new List<Poop>();

            // Spawn characters
            SpawnCharacters();

            // For each character
            foreach (AbstractCharacter character in characters)
            {
                // Update the character
                character.Update(gameTime);

                // If the character has been alive long enough, and is not in the camera's
                // bounds and is 100 units from the bird
                if (character.MinLifetime > 0 && character.Lifetime > character.MinLifetime
                    && camera.CharacterInCameraBounds(character) == false
                    && Vector2.Distance(character.Position, bird.Position) > 100)
                {
                    // Add the character to the remove list
                    charactersToRemove.Add(character);
                }
            }

            // For each drone
            foreach (Drone drone in drones)
            {
                // Update the drone
                drone.Update(gameTime);
            }

            // For each poop
            foreach (Poop poop in poops)
            {
                // Update the poop
                poop.Update(gameTime);

                // If the poop is not in the camera bounds
                if (camera.PoopInCameraBounds(poop) == false)
                {
                    // Add the poop to the remove list
                    poopsToRemove.Add(poop);
                }
            }

            // If the state is not the score screen
            if (state != WorldState.Score)
            {
                // Go through the list of characters to remove
                foreach (AbstractCharacter character in charactersToRemove)
                {
                    // Kill the character
                    character.Kill();
                    
                    // Remove it from the main character list
                    characters.Remove(character);
                }

                // Go through the list of poops to remove
                foreach (Poop poop in poopsToRemove)
                {
                    // Remove it from the main poop list
                    poops.Remove(poop);
                }
            }
        }

        /// <summary>
        /// Spawn some characters.
        /// </summary>
        private void SpawnCharacters()
        {
            // If the score screen is not showing
            if (state != WorldState.Score)
            {
                // If the number of characters is not at the maximum
                if (characters.Count < MaximumCharacters)
                {
                    // Get a random number to determine the character type
                    int characterType = new Random().Next(0, 10);

                    // 3% chance of spawning a static character
                    if (characterType <= 2)
                    {
                        SpawnStatic();
                    }
                    // 7% chance of spawning a roaming character
                    else
                    {
                        SpawnRoaming();
                    }
                }

                // Spawn a drone
                SpawnDrone();
            }
        }

        /// <summary>
        /// Spawn a static character.
        /// </summary>
        private void SpawnStatic()
        {
            // Get a list of available spawn points that are not occupied and allow static characters
            List<SpawnPoint> availableSpawns = SpawnPoint.SpawnPoints.Where(s => s.Static
                                                                            && s.Occupied == false)
                                                                     .ToList();

            // If there are available spawn points
            if (availableSpawns.Any())
            {
                // Get a random spawn point
                int index = new Random().Next(0, availableSpawns.Count);

                // If the spawn point is not in the camera's bounds
                if (camera.SpawnPointInCameraBounds(availableSpawns[index]) == false)
                {
                    // Occupy the spawn point
                    availableSpawns[index].Occupy();

                    // Add a character at the spawn point
                    characters.Add(new StaticCharacter(availableSpawns[index]));
                }
            }
        }

        /// <summary>
        /// Spawn a roaming character.
        /// </summary>
        private void SpawnRoaming()
        {
            // Get a list of available spawn points that are not occupied and do not allow static characters
            List<SpawnPoint> availableSpawns = SpawnPoint.SpawnPoints.Where(s => s.Static == false
                                                                            && s.Occupied == false)
                                                                     .ToList();

            // If there are available spawn points
            if (availableSpawns.Any())
            {
                // Get a random spawn point
                int index = new Random().Next(0, availableSpawns.Count);

                // If the spawn point is not in the camera bounds
                if (camera.SpawnPointInCameraBounds(availableSpawns[index]) == false)
                {
                    // Occupy the spawn point
                    availableSpawns[index].Occupy();

                    // Add a character at the spawn point, and get a route from the current spawn point
                    characters.Add(new RoamingCharacter(availableSpawns[index], NodeNetwork.GetRouteFromSpawnPoint(availableSpawns[index])));
                }
            }
        }

        /// <summary>
        /// Spawn a drone.
        /// </summary>
        private void SpawnDrone()
        {
            // If the number of drones active is less than the maximum drones allowed
            if (dronesActive < maxDronesAllowed)
            {
                // If the current score is greater than 100 and a 1% chance of spawning has been reached
                if (ScoreCounter.CurrentScore > 100 && new Random().Next(0, 100) >= 99)
                {
                    // Get a list of available spawn points that are not occupied
                    List<SpawnPoint> availableSpawns = SpawnPoint.SpawnPoints.Where(s => s.Occupied == false)
                                                                             .ToList();

                    // Get a random spawn point from the list
                    int index = new Random().Next(0, availableSpawns.Count);

                    // If the spawn point is not in the camera's bounds
                    if (camera.SpawnPointInCameraBounds(availableSpawns[index]) == false)
                    {
                        // Occupy the spawn point
                        availableSpawns[index].Occupy();

                        // Add a drone at the spawn point, and increase the number active
                        drones.Add(new Drone(availableSpawns[index]));
                        dronesActive++;
                    }
                }
            }

            // Go through the maximum number of drones allowed and if the score is 
            // greater than the set value, increase the maximum number allowed
            switch (maxDronesAllowed)
            {
                case 0:
                    maxDronesAllowed = ScoreCounter.CurrentScore > 100 ? 1 : 0;
                    break;

                case 1:
                    maxDronesAllowed = ScoreCounter.CurrentScore > 500 ? 2 : 1;
                    break;

                case 2:
                    maxDronesAllowed = ScoreCounter.CurrentScore > 1000 ? 3 : 2;
                    break;

                case 3:
                    maxDronesAllowed = ScoreCounter.CurrentScore > 2000 ? 4 : 3;
                    break;

                case 4:
                    maxDronesAllowed = ScoreCounter.CurrentScore > 2500 ? 5 : 4;
                    break;

                case 5:
                    maxDronesAllowed = ScoreCounter.CurrentScore > 2750 ? 6 : 5;
                    break;

                default:
                    maxDronesAllowed = 0;
                    break;
            }
        }

        /// <summary>
        /// Populate the world with characters.
        /// </summary>
        private void PopulateWorldWithCharacters()
        {
            // Starting at 0 and moving up to half the maximum number of characters
            for (int i=0; i<MaximumCharacters / 2; i++)
            {
                // Spawn a character
                SpawnCharacters();
            }
        }

        /// <summary>
        /// Draw the world.
        /// </summary>
        private void DrawWorld()
        {
            // Draw the town
            WorldManager.SpriteBatch.Draw(town, townPosition, Color.White);

            // For each of the characters, draw the character
            foreach (AbstractCharacter character in characters)
            {
                character.Draw();
            }

            // For each of the poops, draw the poop
            foreach (Poop poop in poops)
            {
                poop.Draw();
            }

            // For each of the drones, draw the drone
            foreach (Drone drone in drones)
            {
                drone.Draw();
            }
        }

        /// <summary>
        /// Update the matrix for the UI to scale it by the resolution.
        /// </summary>
        private void UpdateUiMatrix()
        {
            // If the game is not fullscreen
            if (SaveManager.SaveData.Fullscreen == false)
            {
                // Scale the matrix by dividing the window's resolution by the base resolution
                uiMatrix = Matrix.CreateScale(SaveManager.SaveData.ResolutionWidth / MainGame.DefaultWidth,
                                          SaveManager.SaveData.ResolutionHeight / MainGame.DefaultHeight,
                                          1);
            }
            // If the game is fullscreen
            else
            {
                // Scale the matrix by dividing the screen's resolution by the base resolution
                uiMatrix = Matrix.CreateScale(WorldManager.SpriteBatch.GraphicsDevice.DisplayMode.Width / MainGame.DefaultWidth,
                                          WorldManager.SpriteBatch.GraphicsDevice.DisplayMode.Height / MainGame.DefaultHeight,
                                          1);
            }
        }
    }
}
