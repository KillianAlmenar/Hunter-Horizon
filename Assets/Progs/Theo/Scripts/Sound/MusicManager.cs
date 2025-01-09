using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum MusicState
{
    Calm,
    Strong
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    const float STEP_DURATION = 8f;
    const float END_DURATION = 2f;

    const int MIN_ENEMIES_FOR_TRANSITION = 1;

    const float SNAPSHOT_TRANSITION_DURATION = 2f;
    [SerializeField] AudioMixer mixer;

    AudioSource[] sources;
    AudioSource currentSource;
    int currentSourceIndex = 0;

    static MusicData data;
    public string dataName;
    int currentPart = 0;
    MusicState currentState = MusicState.Calm;
    bool isStateChanging = false;
    bool isStopping = false;

    const float DELAY_BEFORE_TRANSITION = 2f;
    float delayBeforeTransition = 0f;
    int nbEnemiesAttackDuringFrame = 0;

    enum SubPart
    {
        Intro,
        Loop,
        Outro
    }
    SubPart subPart = SubPart.Intro;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            sources = GetComponents<AudioSource>();
            currentSource = sources[0];
            instance.Load(dataName, true);
        }
        else
        {
            Destroy(gameObject);
            instance.Load(dataName, false);
        }
    }

    private void Load(string name, bool firstLoading)
    {
        if (!firstLoading && dataName == name)
            return;

        subPart = SubPart.Intro;
        currentState = MusicState.Calm;
        currentPart = 0;
        nbEnemiesAttackDuringFrame = 0;
        isStopping = false;

        dataName = name;
        data = Resources.Load<MusicData>(dataName);
        StopAllCoroutines();
        Play(data.parts[0].calmIntro);
    }

    private void Update()
    {
        if (GameManager.instance.IsInABossFight)
            BeginBossFight();

        if (isStopping)
            return;

        if (nbEnemiesAttackDuringFrame == 0)
        {
            delayBeforeTransition += Time.deltaTime;
            if (delayBeforeTransition > DELAY_BEFORE_TRANSITION)
            {
                delayBeforeTransition = 0f;
                SetState(MusicState.Calm);
            }
        }
        else
        {
            delayBeforeTransition = 0f;
        }
        nbEnemiesAttackDuringFrame = 0;

        if (isStateChanging
            || currentSource.time < currentSource.clip.length - END_DURATION
            || subPart == SubPart.Outro
            || GetOtherSource().isPlaying)
            return;
        
        subPart = SubPart.Loop;
        switch (currentState)
        {
            case MusicState.Calm:
                Play(data.parts[currentPart].calmLoop);
                break;

            case MusicState.Strong:
                Play(data.parts[currentPart].strongLoop);
                break;
        }
    }

    AudioSource GetOtherSource()
    {
        return sources[(currentSourceIndex + 1) % sources.Length];
    }

    private void Play(AudioClip clip)
    {
        StartCoroutine(SmoothlyCutSource(currentSource));
        currentSourceIndex = (currentSourceIndex + 1) % sources.Length;
        currentSource = sources[currentSourceIndex];
        currentSource.clip = clip;
        currentSource.Play();
    }

    IEnumerator SmoothlyCutSource(AudioSource source)
    {
        const float CUT_DURATION = 1f;
        while (source.volume > 0f)
        {
            source.volume = Mathf.Max(source.volume - Time.deltaTime / CUT_DURATION, 0f);
            yield return null;
        }
        source.Stop();
        source.volume = 1f;
    }

    public void SetState(MusicState state)
    {
        if (subPart != SubPart.Loop || isStateChanging || state == currentState)
            return;

        isStateChanging = true;
        float delay = Mathf.Ceil(currentSource.time / STEP_DURATION) * STEP_DURATION - currentSource.time;
        StartCoroutine(SetStateDelayed(currentState, currentPart, delay));
        currentState = state;
        if (state == MusicState.Calm)
        {
            currentPart = (currentPart + 1) % data.parts.Length;
        }
    }

    public void BeginFight()
    {
        ++nbEnemiesAttackDuringFrame;
        if (currentState == MusicState.Calm
            && subPart == SubPart.Loop
            && nbEnemiesAttackDuringFrame >= MIN_ENEMIES_FOR_TRANSITION)
        {
            SetState(MusicState.Strong);
        }
    }

    public void BeginBossFight()
    {
        nbEnemiesAttackDuringFrame = MIN_ENEMIES_FOR_TRANSITION;
        if (currentState == MusicState.Calm
            && subPart == SubPart.Loop)
        {
            SetState(MusicState.Strong);
        }
    }

    public void Stop()
    {
        if (isStopping)
            return;
        isStopping = true;

        if (currentState == MusicState.Strong)
        {
            foreach (AudioSource source in sources)
            {
                if (source.isPlaying)
                {
                    StartCoroutine(SmoothlyCutSource(source));
                }
            }
            mixer.FindSnapshot("Normal").TransitionTo(SNAPSHOT_TRANSITION_DURATION);
            currentSourceIndex = (currentSourceIndex + 1) % sources.Length;
            currentSource = sources[currentSourceIndex];
            currentSource.clip = data.parts[currentPart].strongOutro;
            currentSource.Play();
        }
    }
    
    IEnumerator SetStateDelayed(MusicState previousState, int previousPart, float delay)
    {
        yield return new WaitForSeconds(delay);
        isStateChanging = false;
        bool stateChanged = previousState != currentState;

        switch (currentState)
        {
            case MusicState.Calm:
                {
                    mixer.FindSnapshot("Normal").TransitionTo(SNAPSHOT_TRANSITION_DURATION);

                    if (stateChanged)
                    {
                        if (subPart == SubPart.Outro)
                        {
                            Play(data.parts[previousPart].strongOutro);
                            StartCoroutine(SetStateDelayed(previousState, previousPart, data.parts[previousPart].strongOutro.length - END_DURATION));
                            subPart = SubPart.Intro;
                        }
                        else
                        {
                            Play(data.parts[currentPart].calmIntro);
                        }
                    }
                    else
                    {
                        Play(data.parts[currentPart].calmLoop);
                        subPart = SubPart.Loop;
                    }
                    break;
                }

            case MusicState.Strong:
                {
                    mixer.FindSnapshot("Fight").TransitionTo(SNAPSHOT_TRANSITION_DURATION);

                    if (stateChanged && subPart == SubPart.Loop)
                    {
                        Play(data.parts[currentPart].strongIntro);
                        subPart = SubPart.Intro;
                    }
                    break;
                }
        }
    }
}
