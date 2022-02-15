﻿namespace BirdGame.World
{
    using BirdGame.Audio;
    using BirdGame.Characters;
    using BirdGame.Data;
    using BirdGame.Enums;
    using BirdGame.Events;
    using BirdGame.Input;
    using BirdGame.UI;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    internal class GameWorld
    {
        private readonly IntroText introText;

        private readonly ScoreCounter scoreCounter;

        private readonly Bird bird;

        private readonly List<AbstractCharacter> characters;

        private readonly PauseScreen pauseScreen;

        private readonly Camera camera;

        private ScoreScreen scoreScreen;

        private WorldState state;

        public GameWorld()
        {
            state = WorldState.Title;

            scoreCounter = ScoreCounter.Initialise();

            introText = new IntroText();

            characters = new List<AbstractCharacter>();

            bird = new Bird();

            camera = new Camera(bird);

            pauseScreen = new PauseScreen();

            AudioManager.PlayLoopingSoundEffect("Ambience");
        }

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

                default:
                    break;
            }
        }

        public void Draw()
        {
            WorldManager.SpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: camera.Transform);

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

        private void UpdateWorld(GameTime gameTime)
        {
            foreach (AbstractCharacter character in characters)
            {
                character.Update(gameTime);
            }
        }

        private void DrawWorld()
        {
            // TODO: tiles

            foreach (AbstractCharacter character in characters)
            {
                character.Draw();
            }
        }
    }
}
