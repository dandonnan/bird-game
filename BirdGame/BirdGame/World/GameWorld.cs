namespace BirdGame.World
{
    using BirdGame.Characters;
    using BirdGame.Data;
    using BirdGame.Enums;
    using BirdGame.Events;
    using Microsoft.Xna.Framework;

    internal class GameWorld
    {
        private readonly SaveData saveData;

        private readonly IntroText introText;

        private readonly Bird bird;

        private ScoreScreen scoreScreen;

        private WorldState state;

        public GameWorld()
        {
            state = WorldState.Title;

            saveData = SaveData.Load();

            introText = new IntroText();

            bird = new Bird();
        }

        public void Update(GameTime gameTime)
        {
            if (state == WorldState.Title)
            {
                UpdateIntro();
            }

            bird.Update(gameTime);
        }

        public void Draw()
        {
            bird.Draw();

            if (state == WorldState.Score)
            {
                scoreScreen.Draw();
            }

            if (state == WorldState.Title)
            {
                introText.Draw();
            }
        }

        private void UpdateIntro()
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
        }
    }
}
