namespace BirdGame.Graphics
{
    using BirdGame.World;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    /// <summary>
    /// A library of sprites.
    /// </summary>
    internal class SpriteLibrary
    {
        /// <summary>
        /// A singleton for the sprite library, so there is only ever one.
        /// </summary>
        private static SpriteLibrary spriteLibrary;

        /// <summary>
        /// The texture for objects in the town.
        /// </summary>
        private readonly Texture2D objectsTexture;

        /// <summary>
        /// The texture for the bird.
        /// </summary>
        private readonly Texture2D birdTexture;

        /// <summary>
        /// The dictionary of sprites.
        /// </summary>
        private readonly Dictionary<string, AbstractSprite> spriteDictionary;

        /// <summary>
        /// A private constructor for the sprite library that can only be created
        /// through the Initialise method.
        /// </summary>
        private SpriteLibrary()
        {
            // Load the textures
            objectsTexture = WorldManager.ContentManager.Load<Texture2D>("Sprites\\objects");
            birdTexture = WorldManager.ContentManager.Load<Texture2D>("Sprites\\bird");

            // Populate the dictionary
            spriteDictionary = PopulateDictionary();

            spriteLibrary = this;
        }

        /// <summary>
        /// Initialise the sprite library.
        /// </summary>
        public static void Initialise()
        {
            if (spriteLibrary == null)
            {
                new SpriteLibrary();
            }
        }

        /// <summary>
        /// Get a non-animated sprite by the id.
        /// </summary>
        /// <param name="id">The id of the sprite.</param>
        /// <returns>A sprite with the id, or null if not found.</returns>
        public static Sprite GetSprite(string id)
        {
            Sprite sprite = null;
            
            // If the sprite is in the sprite dictionary
            if (spriteLibrary.spriteDictionary.TryGetValue(id, out AbstractSprite abstractSprite))
            {
                // Convert from an abstract sprite to a non-animated one
                sprite = new Sprite((Sprite)abstractSprite);
            }

            return sprite;
        }

        /// <summary>
        /// Get an animated sprite by the id.
        /// </summary>
        /// <param name="id">The id of the sprite.</param>
        /// <returns>A sprite with the id, or null if not found.</returns>
        public static AnimatedSprite GetAnimatedSprite(string id)
        {
            AnimatedSprite sprite = null;

            // If the sprite is in the sprite dictionary
            if (spriteLibrary.spriteDictionary.TryGetValue(id, out AbstractSprite abstractSprite))
            {
                // Convert from an abstract sprite to an animated one
                sprite = new AnimatedSprite((AnimatedSprite)abstractSprite);
            }

            return sprite;
        }

        /// <summary>
        /// Populate the sprite dictionary.
        /// </summary>
        /// <returns>A dictionary of sprites.</returns>
        private Dictionary<string, AbstractSprite> PopulateDictionary()
        {
            Dictionary<string, AbstractSprite> dictionary = new Dictionary<string, AbstractSprite>
            {
                // The id of the sprite, a non-animated sprite with a single frame
                { "Grass", new Sprite(objectsTexture, new Frame(1, 1, 32)) },
                { "Sand", new Sprite(objectsTexture, new Frame(34, 1, 32)) },
                { "Sea", new Sprite(objectsTexture, new Frame(67, 1, 32)) },
                { "Pavement", new Sprite(objectsTexture, new Frame(100, 1, 32)) },
                { "Road", new Sprite(objectsTexture, new Frame(133, 1, 32)) },
                { "Brick", new Sprite(objectsTexture, new Frame(166, 1, 32)) },
                { "Coffee", new Sprite(objectsTexture, new Frame(207, 111, 4)) },
                { "CoffeePoop", new Sprite(objectsTexture, new Frame(212, 111, 4)) },
                { "Chips", new Sprite(objectsTexture, new Frame(207, 116, 4)) },
                { "ChipsPoop", new Sprite(objectsTexture, new Frame(212, 116, 4)) },
                { "IceCream", new Sprite(objectsTexture, new Frame(207, 116, 4)) },
                { "IceCreamPoop", new Sprite(objectsTexture, new Frame(212, 116, 4)) },
                { "StainPoop", new Sprite(objectsTexture, new Frame(201, 65, 3)) },
                { "Car", new Sprite(objectsTexture, new Frame(1, 65, 44)) },
                { "UiBackground", new Sprite(objectsTexture, new Frame(50, 65, 16)) },
                // The id of the sprite, then an animated sprite with each frame
                {
                    "NPC1",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(3, 111, 16),
                        new Frame(20, 111, 16),
                        new Frame(3, 111, 16),
                        new Frame(37, 111, 16),
                    })
                },
                {
                    "NPC1_Carry",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(3, 111, 16),
                        new Frame(20, 111, 16),
                        new Frame(3, 111, 16),
                        new Frame(54, 111, 16),
                    })
                },
                {
                    "NPC2",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(71, 111, 16),
                        new Frame(88, 111, 16),
                        new Frame(71, 111, 16),
                        new Frame(105, 111, 16),
                    })
                },
                {
                    "NPC2_Carry",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(71, 111, 16),
                        new Frame(88, 111, 16),
                        new Frame(71, 111, 16),
                        new Frame(122, 111, 16),
                    })
                },
                {
                    "NPC3",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(139, 111, 16),
                        new Frame(156, 111, 16),
                        new Frame(139, 111, 16),
                        new Frame(173, 111, 16),
                    })
                },
                {
                    "NPC3_Carry",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(139, 111, 16),
                        new Frame(156, 111, 16),
                        new Frame(139, 111, 16),
                        new Frame(190, 111, 16),
                    })
                },
                {
                    "NPC4",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(3, 128, 16),
                        new Frame(20, 128, 16),
                        new Frame(3, 128, 16),
                        new Frame(37, 128, 16),
                    })
                },
                {
                    "NPC4_Carry",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(3, 128, 16),
                        new Frame(20, 128, 16),
                        new Frame(3, 128, 16),
                        new Frame(54, 128, 16),
                    })
                },
                {
                    "NPC5",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(71, 128, 16),
                        new Frame(89, 128, 16),
                        new Frame(71, 128, 16),
                        new Frame(105, 128, 16),
                    })
                },
                {
                    "NPC5_Carry",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(71, 128, 16),
                        new Frame(89, 128, 16),
                        new Frame(71, 128, 16),
                        new Frame(122, 128, 16),
                    })
                },
                {
                    "NPC6",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(139, 128, 16),
                        new Frame(156, 128, 16),
                        new Frame(139, 128, 16),
                        new Frame(173, 128, 16),
                    })
                },
                {
                    "NPC6_Carry",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(139, 128, 16),
                        new Frame(156, 128, 16),
                        new Frame(139, 128, 16),
                        new Frame(190, 128, 16),
                    })
                },
                {
                    "BirdFly",
                    new AnimatedSprite(birdTexture, new List<Frame>
                    {
                        new Frame(1, 1, 48),
                        new Frame(99, 1, 48),
                        new Frame(148, 1, 48),
                        new Frame(99, 1, 48),
                        new Frame(1, 1, 48),
                        new Frame(50, 1, 48),
                    })
                },
                {
                    "BirdPoop",
                    new AnimatedSprite(birdTexture, new List<Frame>
                    {
                        new Frame(1, 50, 48),
                        new Frame(50, 50, 48),
                        new Frame(99, 50, 48),
                        new Frame(99, 50, 48),
                        new Frame(99, 50, 48),
                        new Frame(50, 50, 48),
                        new Frame(1, 50, 48),
                    },
                    false)
                },
                {
                    "BirdDiveDown",
                    new AnimatedSprite(birdTexture, new List<Frame>
                    {
                        new Frame(1, 99, 48),
                        new Frame(50, 99, 48),
                        new Frame(99, 99, 48),
                        new Frame(148, 99, 48),
                        new Frame(197, 99, 48),
                        new Frame(197, 99, 48),
                    },
                    false)
                },
                {
                    "BirdDiveUp",
                    new AnimatedSprite(birdTexture, new List<Frame>
                    {
                        new Frame(197, 99, 48),
                        new Frame(148, 99, 48),
                        new Frame(99, 99, 48),
                        new Frame(50, 99, 48),
                        new Frame(1, 99, 48),
                    },
                    false)
                },
                {
                    "BirdDead",
                    new AnimatedSprite(birdTexture, new List<Frame>
                    {
                        new Frame(1, 148, 48),
                        new Frame(50, 148, 48),
                        new Frame(99, 148, 48),
                        new Frame(148, 148, 48),
                        new Frame(197, 148, 48),

                    },
                    false)
                },
                {
                    "DroneFly",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(1, 1, 48),
                        new Frame(50, 1, 48)
                    })
                },
                {
                    "PoopDrop",
                    new AnimatedSprite(objectsTexture, new List<Frame>
                    {
                        new Frame(193, 65, 3),
                        new Frame(197, 65, 3),
                        new Frame(201, 65, 3),
                    },
                    false)
                }
            };

            return dictionary;
        }
    }
}
