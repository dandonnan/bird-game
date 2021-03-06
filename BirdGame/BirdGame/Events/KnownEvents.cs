namespace BirdGame.Events
{
    /// <summary>
    /// Known events that can be fired.
    /// </summary>
    internal class KnownEvents
    {
        public static string SpawnBird = "SpawnBird";

        public static string IntroFinished = "IntroFinished";

        public static string PoopSpawned = "PoopSpawned";

        public static string PoopLanded = "PoopLanded";

        public static string DiveBottom = "DiveBottom";

        public static string BirdDead = "BirdDead";

        public static string PointsAwarded = "PointsAwarded";

        public static string HidePoints = "HidePoints";

        public static string ResolutionChanged = "ResolutionChanged";
    }
}
