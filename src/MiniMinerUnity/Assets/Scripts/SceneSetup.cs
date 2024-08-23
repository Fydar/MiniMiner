using MiniMinerUnity.DialogueSystem;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MiniMinerUnity
{
    [Serializable]
    public class SceneSetup
    {
        [Header("Game Data")]
        public EquipmentItemTemplate[] Equipment;
        public RewardType[] Rarity1Rewards;
        public RewardType[] Rarity2Rewards;
        public RewardType[] Rarity3Rewards;
        public RewardType[] Rarity4Rewards;
        public RewardType[] Rarity5Rewards;

        [Header("Dialogue")]
        public DialogueSystem.DialogueSystem Dialogue;
        public TextStyle IntroStyle1;
        public TextStyle IntroStyle2;

        [Header("Shop")]
        public CanvasGroup ShopFader;
        public Text CurrencyText;

        [Space]
        public RectTransform RewardTab;
        public Image RewardGraphic;

        public RectTransform RewardDetails;
        public Text RewardQuantity;
        public Text[] RewardName;
        public Text RewardValueCounter;
        public RectTransform RewardStarHolder;
        public UIPool<Image> RewardStarPool;

        [Space]
        public RectTransform EquipmentTab;
        public RectTransform EquipmentShopHolder;
        public UIPool<EquipmentShopRenderer> EquipmentShopRendererPool;

        [Header("Interaction Shop")]
        public RectTransform InteractionShop;
        public Text InteractionShopCurrencyText;

        [Header("World")]
        public Tilemap RarityMap;
        public TileBase[] RarityTiles;

        [Header("General")]
        public Canvas overlayCanvas;
        public GameObject transitionStack;
        public Transition CircleWipe;
        public Transition SawToothWipe;
        public Transition LeftToRightWipe;
        public Transition RightToLeftWipe;
        public Transition TopToBottomWipe;
        public Transition BottomToTopWipe;

        [Header("Music")]
        public AudioSource MainMenuMusic;
        public AudioSource IntroMusic;
        public AudioSource WorldMusic;
        public float WorldMusicOverworldVolume = 1.0f;
        public float WorldMusicMiningVolume = 0.6f;

        [Header("Pause")]
        public RectTransform PauseMenu;

        [Header("Main Menu")]
        public RectTransform MainMenu;

        [Space]
        public SfxGroup MainMenuPunch;
        public RectTransform MainMenuPart1;
        public RectTransform MainMenuPart2;
        public RectTransform MainMenuPart3;
        public RectTransform MainMenuContinueText;
        public SfxGroup MainMenuContinueSound;

        [Header("Introduction")]
        public Image TalkingToCharacter;
        public PerlinShake TalkingToCharacterShake;

        [Header("Overworld")]
        public WorldData VoidWorld;
        public WorldData WorldOverworld;
        public PlayerController PlayerPrefab;

        [Header("Mining")]
        public WorldData WorldMining;
        public WallRenderer WallRenderer;
        public ParticleSystem DustParticles;
        public ParticleSystem HitDustParticles;
        public ParticleSystem DustFallParticles;
        public Animator MiningSelection;
        public TransitionGraphic CracksTransition;

        [Space]
        public Image CurrentEquipment;
        public Image PreviousArrow;
        public Image NextArrow;
        public Image EquipmentSelector;
        public Text CurrentEquipmentLevelText;
        public Color Darken;

        [Space]
        public Text[] BagCapacity;

        [Header("Audio")]
        public SfxGroup NudgeSound;
        public SfxGroup StepSound;
        public SfxGroup NoSound;
        public SfxGroup OkaySound;
        public SfxGroup CollectSound;
        public SfxGroup CollapseSound;
        public SfxGroup CollectAllSound;
        public SfxGroup UIAppearSound;
        public SfxGroup StarAppearSound;
        public SfxGroup BoulderBreak;

        public void SetActiveWorld(WorldData world)
        {
            VoidWorld.gameObject.SetActive(false);
            WorldOverworld.gameObject.SetActive(false);
            WorldMining.gameObject.SetActive(false);

            if (world != null)
            {
                world.gameObject.SetActive(true);
                overlayCanvas.worldCamera = world.WorldCamera;
                transitionStack.transform.SetParent(world.WorldCamera.transform);
                transitionStack.transform.localPosition = new Vector3(0.0f, 0.0f, 10.0f);
            }

            CircleWipe.SetTime(0.0f);
            SawToothWipe.SetTime(0.0f);
            LeftToRightWipe.SetTime(0.0f);
            RightToLeftWipe.SetTime(0.0f);
            TopToBottomWipe.SetTime(0.0f);
            BottomToTopWipe.SetTime(0.0f);
        }
    }
}
