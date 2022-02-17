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

    internal class GameWorld
    {
        public static Rectangle WorldBounds = new Rectangle(96, 96, 928, 928);

        private const int MaximumCharacters = 60;

        private readonly IntroText introText;

        private readonly ScoreCounter scoreCounter;

        private readonly Bird bird;

        private readonly List<AbstractCharacter> characters;

        private readonly List<Poop> poops;

        private readonly PauseScreen pauseScreen;

        private readonly Camera camera;

        private readonly Texture2D town;

        private readonly Vector2 townPosition;

        private ScoreScreen scoreScreen;

        private WorldState state;

        private int dronesActive;

        private int maxDronesAllowed;

        public GameWorld()
        {
            state = WorldState.Title;

            scoreCounter = ScoreCounter.Initialise();
            NodeNetwork.Initialise();

            introText = new IntroText();

            characters = new List<AbstractCharacter>();

            poops = new List<Poop>();

            bird = new Bird();

            camera = new Camera(bird);

            pauseScreen = new PauseScreen();

            town = WorldManager.ContentManager.Load<Texture2D>("Sprites\\town");

            townPosition = new Vector2(-128, -128);

            AudioManager.PlayLoopingSoundEffect("Ambience");

            PopulateWorldWithCharacters();

            maxDronesAllowed = 0;
            dronesActive = 0;
        }

        public Camera Camera => camera;

        public Bird Bird => bird;

        public void Update(GameTime gameTime)
        {
            switch (state)
            {
                case WorldState.Title:
                    UpdateIntro(gameTime);
                    break;

                case WorldState.Playing:
                    UpdatePlaying(gameTime);
                    break;

                case WorldState.Paused:
                    UpdatePaused();
                    break;

                case WorldState.Score:
                    UpdateScore(gameTime);
                    break;
            }
        }

        public void Draw()
        {
            WorldManager.SpriteBatch.Begin(SpriteSortMode.Deferred, transformMatrix: camera.Transform);

            DrawWorld();
            bird.Draw();

            WorldManager.SpriteBatch.End();

            WorldManager.SpriteBatch.Begin();

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

            WorldManager.SpriteBatch.End();
        }

        private void Reset()
        {
            camera.Reset();
            bird.Reset();
            characters.ForEach(c => c.Kill());
            characters.Clear();
            poops.Clear();
            scoreCounter.Reset();
            AudioManager.StopLoopingSoundEffect("DroneFly");
            PopulateWorldWithCharacters();
            state = WorldState.Playing;
            maxDronesAllowed = 0;
            dronesActive = 0;
        }

        private void UpdateIntro(GameTime gameTime)
        {
            introText.Update();

            if (EventManager.EventFired(KnownEvents.SpawnBird))
            {
                bird.AllowMovement(true);
            }

            if (EventManager.EventFired(KnownEvents.IntroFinished))
            {
                bird.AllowControl();
                state = WorldState.Playing;
            }

            UpdateWorld(gameTime);

            bird.Update(gameTime);
            camera.Update(gameTime);
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            if (InputManager.IsBindingPressed(DefaultBindings.Pause))
            {
                pauseScreen.Reset();
                state = WorldState.Paused;
            }

            if (EventManager.EventFired(KnownEvents.BirdDead))
            {
                scoreScreen = new ScoreScreen(ScoreCounter.CurrentScore, SaveManager.SaveData.HighScore);
                SaveManager.SaveData.HighScore = ScoreCounter.CurrentScore;
                SaveManager.Save();
                state = WorldState.Score;
            }

            if (EventManager.EventFiredThenKill(KnownEvents.PoopSpawned))
            {
                poops.Add(new Poop(bird.Position));
            }

            scoreCounter.Update();

            UpdateWorld(gameTime);

            bird.Update(gameTime);
            camera.Update(gameTime);
        }

        private void UpdatePaused()
        {
            pauseScreen.Update();

            if (InputManager.IsBindingPressed(DefaultBindings.Pause))
            {
                SaveManager.Save();
                state = WorldState.Playing;
            }
        }

        private void UpdateScore(GameTime gameTime)
        {
            if (InputManager.IsBindingPressed(DefaultBindings.Poop)
                || InputManager.IsBindingPressed(DefaultBindings.Dive))
            {
                Reset();
            }

            UpdateWorld(gameTime);
        }

        private void UpdateWorld(GameTime gameTime)
        {
            List<AbstractCharacter> charactersToRemove = new List<AbstractCharacter>();
            List<Poop> poopsToRemove = new List<Poop>();

            SpawnCharacters();

            foreach (AbstractCharacter character in characters)
            {
                character.Update(gameTime);

                if (character.MinLifetime > 0 && character.Lifetime > character.MinLifetime
                    && camera.CharacterInCameraBounds(character) == false
                    && Vector2.Distance(character.Position, bird.Position) > 300)
                {
                    charactersToRemove.Add(character);
                }
            }

            foreach (Poop poop in poops)
            {
                poop.Update(gameTime);

                if (camera.PoopInCameraBounds(poop) == false)
                {
                    poopsToRemove.Add(poop);
                }
            }

            if (state != WorldState.Score)
            {
                foreach (AbstractCharacter character in charactersToRemove)
                {
                    character.Kill();
                    characters.Remove(character);
                }

                foreach (Poop poop in poopsToRemove)
                {
                    poops.Remove(poop);
                }
            }
        }

        private void SpawnCharacters()
        {
            if (state != WorldState.Score)
            {
                if (characters.Count < MaximumCharacters)
                {
                    int characterType = new Random().Next(0, 10);

                    if (characterType <= 2)
                    {
                        SpawnStatic();
                    }
                    else
                    {
                        SpawnRoaming();
                    }
                }

                SpawnDrone();
            }
        }

        private void SpawnStatic()
        {
            List<SpawnPoint> availableSpawns = SpawnPoint.SpawnPoints.Where(s => s.Static
                                                                            && s.Occupied == false)
                                                                     .ToList();

            if (availableSpawns.Any())
            {
                int index = new Random().Next(0, availableSpawns.Count);

                if (camera.SpawnPointInCameraBounds(availableSpawns[index]) == false)
                {
                    availableSpawns[index].Occupy();
                    characters.Add(new StaticCharacter(availableSpawns[index]));
                }
            }
        }

        private void SpawnRoaming()
        {
            List<SpawnPoint> availableSpawns = SpawnPoint.SpawnPoints.Where(s => s.Static == false
                                                                            && s.Occupied == false)
                                                                     .ToList();

            if (availableSpawns.Any())
            {
                int index = new Random().Next(0, availableSpawns.Count);

                if (camera.SpawnPointInCameraBounds(availableSpawns[index]) == false)
                {
                    availableSpawns[index].Occupy();
                    characters.Add(new RoamingCharacter(availableSpawns[index], NodeNetwork.GetRouteFromSpawnPoint(availableSpawns[index])));
                }
            }
        }

        private void SpawnDrone()
        {
            if (dronesActive < maxDronesAllowed)
            {
                if (ScoreCounter.CurrentScore > 100 && new Random().Next(0, 100) >= 99)
                {
                    List<SpawnPoint> availableSpawns = SpawnPoint.SpawnPoints.Where(s => s.Occupied == false)
                                                                             .ToList();

                    int index = new Random().Next(0, availableSpawns.Count);

                    if (camera.SpawnPointInCameraBounds(availableSpawns[index]) == false)
                    {
                        availableSpawns[index].Occupy();
                        characters.Add(new Drone(availableSpawns[index]));
                        dronesActive++;
                    }
                }
            }

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

        private void PopulateWorldWithCharacters()
        {
            for (int i=0; i<MaximumCharacters / 2; i++)
            {
                SpawnCharacters();
            }
        }

        private void DrawWorld()
        {
            WorldManager.SpriteBatch.Draw(town, townPosition, Color.White);

            foreach (AbstractCharacter character in characters)
            {
                character.Draw();
            }

            foreach (Poop poop in poops)
            {
                poop.Draw();
            }
        }
    }
}
