using System.Collections.Generic;
using FPLibrary;
using UnityEngine;

[System.Serializable]
    public class GlobalInfo : ScriptableObject
    {

        #region public instance fields
        public float version;
        public LanguageOptions selectedLanguage;
        public CharacterInfo player1Character;
        public CharacterInfo player2Character;
        public CharacterInfo p1CharStorage;
        public CharacterInfo p2CharStorage;
        public StageOptions selectedStage;
        public StorageMode stagePrefabStorage = StorageMode.Legacy;
        public StorageMode stageMusicStorage = StorageMode.Legacy;
        public bool p1CPUControl;
        public bool p2CPUControl;
        public string gameName;


        //-----------------------------------------------------------------------------------------------------------------
        public GameGUI gameGUI;
        public StoryMode storyMode;
        //-----------------------------------------------------------------------------------------------------------------


        //public int fps = 60;
        public int fps { get { return MainScript.fps; } set { MainScript.fps = value; } }
        public float gameSpeed = 1;
        public Fix64 _gameSpeed = 1;
        public int executionBufferTime = 10;
        public ExecutionBufferType executionBufferType;
        public int plinkingDelay = 1;

        public float preloadingTime = 1f;
        public Fix64 _preloadingTime = 1;
        public bool preloadCharacter1 = true;
        public bool preloadCharacter2 = true;
        public bool preloadStage = true;
        public bool preloadHitEffects = true;
        public bool warmAllShaders = true;

        public float gravity = .37f;
        public Fix64 _gravity = .37;
        public bool detect3D_Hits;
        public bool runInBackground;
        public LanguageOptions[] languages = new LanguageOptions[] { new LanguageOptions() };
        public CameraOptions cameraOptions;
        public CharacterRotationOptions characterRotationOptions;
        public RoundOptions roundOptions;
        public BounceOptions groundBounceOptions;
        public BounceOptions wallBounceOptions;
        public CounterHitOptions counterHitOptions;
        public ComboOptions comboOptions;
        public BlockOptions blockOptions;
        public KnockDownOptions knockDownOptions;
        public HitOptions hitOptions;

        public InputReferences[] player1_Inputs = new InputReferences[0]; // Reference to Unity's InputManager to Combat's keys
        public InputReferences[] player2_Inputs = new InputReferences[0]; // Reference to Unity's InputManager to Combat's keys
        public InputOptions inputOptions = new InputOptions();

        public StageOptions[] stages = new StageOptions[0];
        public CharacterInfo[] characters = new CharacterInfo[0];
        public DebugOptions debugOptions = new DebugOptions();
        public TrainingModeOptions trainingModeOptions = new TrainingModeOptions();
        public ChallengeModeOptions[] challengeModeOptions = new ChallengeModeOptions[0];
        public AIOptions aiOptions = new AIOptions();
        public NetworkOptions networkOptions = new NetworkOptions();

        public bool music = true;
        public float musicVolume = 1f;
        public bool soundfx = true;
        public float soundfxVolume = 1f;
        #endregion


        #region trackable definitions
        public int currentRound { get; set; }
        public bool lockInputs { get; set; }
        public bool lockMovements { get; set; }
        public int selectedChallenge { get; set; }
        #endregion

        #region public instance methods
        public virtual void ValidateStoryModeInformation()
        {
            // First, check that every character index in Story Mode is valid
            for (int i = this.storyMode.selectableCharactersInStoryMode.Count - 1; i >= 0; --i)
            {
                int character = this.storyMode.selectableCharactersInStoryMode[i];

                if (character < 0 || character >= this.characters.Length)
                {
                    this.storyMode.characterStories.Remove(character);
                    this.storyMode.selectableCharactersInStoryMode.RemoveAt(i);
                }
                else if (!this.storyMode.characterStories.ContainsKey(character))
                {
                    this.storyMode.characterStories[character] = new CharacterStory();
                }
            }

            // Then check that every character index in Versus Mode is valid
            for (int i = this.storyMode.selectableCharactersInVersusMode.Count - 1; i >= 0; --i)
            {
                int character = this.storyMode.selectableCharactersInVersusMode[i];
                if (character < 0 || character >= this.characters.Length)
                {
                    this.storyMode.selectableCharactersInVersusMode.RemoveAt(i);
                }
            }

            // Finally, check that every character and stage index are valid in the Character Stories
            this.ValidateCharacterStory(this.storyMode.defaultStory);
            foreach (CharacterStory story in this.storyMode.characterStories.Values)
            {
                this.ValidateCharacterStory(story);
            }
        }
        #endregion

        #region protected instance methods
        protected virtual void ValidateCharacterStory(CharacterStory story)
        {
            if (story != null && story.fightsGroups != null)
            {
                foreach (FightsGroup group in story.fightsGroups)
                {
                    List<StoryModeBattle> battles = new List<StoryModeBattle>(group.opponents);

                    for (int i = battles.Count - 1; i >= 0; --i)
                    {
                        StoryModeBattle battle = battles[i];

                        if (battle.opponentCharacterIndex < 0 || battle.opponentCharacterIndex >= this.characters.Length)
                        {
                            battles.RemoveAt(i);
                        }
                        else
                        {
                            for (int j = battle.possibleStagesIndexes.Count - 1; j >= 0; --j)
                            {
                                int stageIndex = battle.possibleStagesIndexes[j];

                                if (stageIndex < 0 || stageIndex >= this.stages.Length)
                                {
                                    battle.possibleStagesIndexes.RemoveAt(j);
                                }
                            }

                            if (battle.possibleStagesIndexes.Count == 0 && this.stages.Length > 0)
                            {
                                battle.possibleStagesIndexes.Add(i % this.stages.Length);
                            }
                        }
                    }

                    group.opponents = battles.ToArray();
                }
            }
        }
        #endregion
    }