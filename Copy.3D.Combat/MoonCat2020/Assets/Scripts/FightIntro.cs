using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FightIntro : MonoBehaviour
{
    private int _roundCounter;

    public Texture2D _roundOneTexture2D;
    public Texture2D _roundTwoTexture2D;
    public Texture2D _roundThreeTexture2D;
    public Texture2D _fightTexture2D;

    private AudioSource _fightIntroAudioSource;

    public AudioClip _roundOneAnnouncement;
    public AudioClip _roundTwoAnnouncement;
    public AudioClip _roundThreeAnnouncement;
    public AudioClip _roundFightAnnouncement;

    private float _fightIntroFadeValue;

    private float _fightIntroFadeSpeed = .33f;

    private bool _displayingRound;
    private bool _displayingFight;

    public static bool _fightIntroFinished;

    private FightIntroState _fightIntroState;

    // Start is called before the first frame update
    void Start()
    {
        _fightIntroAudioSource = GetComponent<AudioSource>();

        _fightIntroFinished = false;
        _fightIntroFadeValue = 0;

        _roundCounter = 1;
        _displayingRound = false;
        _displayingFight = false;


        StartCoroutine(FightIntroManager());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator FightIntroManager()
    {
        while (true)
        {
            switch (_fightIntroState)
            {
                case FightIntroState.FightIntroInitialize:
                    FightIntroInitialize();
                    break;
                case FightIntroState.FightIntroFadeInRound:
                    FightIntroFadeInRound();
                    break;
                case FightIntroState.FightIntroFightAnnouncement:
                    FightIntroFightAnnouncement();
                    break;
            }

            yield return null;
        }
    }

    private void FightIntroInitialize()
    {
        Debug.Log(nameof(FightIntroInitialize));

        _displayingRound = true;

        switch (_roundCounter)
        {
            case 1:
                _fightIntroAudioSource.PlayOneShot(_roundOneAnnouncement);
                break;
            case 2:
                _fightIntroAudioSource.PlayOneShot(_roundTwoAnnouncement);
                break;
            case 3:
                _fightIntroAudioSource.PlayOneShot(_roundThreeAnnouncement);
                break;
        }

        _fightIntroState = FightIntroState.FightIntroFadeInRound;
    }

    private void FightIntroFadeInRound()
    {
        Debug.Log(nameof(FightIntroFadeInRound));

        _fightIntroFadeValue += _fightIntroFadeSpeed * Time.deltaTime;

        if (_fightIntroFadeValue > 1)
        {
            _fightIntroFadeValue = 1;
        }

        if (_fightIntroFadeValue == 1)
        {
            _displayingFight = true;

            _fightIntroAudioSource.PlayOneShot(_roundFightAnnouncement);

            _fightIntroState = FightIntroState.FightIntroFightAnnouncement;
        }
    }

    private void FightIntroFightAnnouncement()
    {
        Debug.Log(nameof(FightIntroFightAnnouncement));

        _fightIntroFadeValue -= _fightIntroFadeSpeed * 2 * Time.deltaTime;

        if (_fightIntroFadeValue < 0)
        {
            _fightIntroFadeValue = 0;
        }

        if (_fightIntroFadeValue == 0)
        {
            _displayingRound = false;
            _displayingFight = false;

            _fightIntroFinished = true;
            StopCoroutine(FightIntroManager());
        }
    }

    private void IncreaseRoundCounter()
    {
        Debug.Log(nameof(IncreaseRoundCounter));

        _roundCounter++;
    }

    private void OnGUI()
    {
        GUI.color = new Color(1, 1, 1, _fightIntroFadeValue);

        if (_displayingRound)
        {
            switch (_roundCounter)
            {
                case 1:
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _roundOneTexture2D);
                    break;
                case 2:
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _roundTwoTexture2D);
                    break;
                case 3:
                    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _roundThreeTexture2D);
                    break;
            }
        }

        if (_displayingFight)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _fightTexture2D);
        }
    }
}