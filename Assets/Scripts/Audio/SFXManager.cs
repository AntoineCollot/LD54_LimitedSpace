using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GlobalSFX
{
    Fire,
    ProjectileHit,
    EnemyDamaged,
    EnemyKilled,
    ItemSpawn,
    PlayerDeath,
    DoorOpen,
    WallOpen,
    ButtonClick,
    RAMPickUp,
    RAMHover,
    RAMModeEnter,
    RAMModeExit,
    RAMUse,
    Typewritting,
    LevelCleared,
    Explosion,
    EnemyAttack,
    DigitSwitch
}
public class SFXManager : MonoBehaviour
{
    public SFXBank bank;

    [HideInInspector] public GlobalSFX lastSFX;
    [HideInInspector] public float lastSFXTime;
    public const float MIN_SFX_INTERVAL = 0.1f;

    AudioSource sfxSource = null;
    public AudioSource musicSource;

    public static bool muteSFX = false;
    public static bool muteMusic = false;

    public static SFXManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        sfxSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        //Mute
        sfxSource.mute = muteSFX;
        if(musicSource!=null)
            musicSource.mute = muteMusic;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (!Instance.musicSource.isPlaying && scene.name != "MainMenu")
            Instance.musicSource.Play();
    }

    public static void MuteSFX(bool value)
    {
        muteSFX = value;
        Instance.sfxSource.mute = muteSFX;
    }

    public static void MuteMusic(bool value)
    {
        muteMusic = value;
        if (Instance.musicSource != null)
            Instance.musicSource.mute = muteMusic;
    }

    public static void PlaySound(GlobalSFX sfx)
    {
        if (Instance == null || (Instance.lastSFX == sfx && Time.unscaledTime - Instance.lastSFXTime < MIN_SFX_INTERVAL))
            return;

        PlaySound((int)sfx);
        Instance.lastSFX = sfx;
        Instance.lastSFXTime = Time.unscaledTime;
    }

    public static void PlaySound(int id)
    {
        if (Instance == null)
            return;
        Instance.sfxSource.PlayOneShot(Instance.bank.clips[id], Instance.bank.volumes[id]);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SFXManager))]
public class SFXManagerEditor : Editor
{
    bool showSFXList = true;
    SerializedObject sfxBankObj;
    SerializedProperty clips;
    SerializedProperty volumes;

    private void OnEnable()
    {
        sfxBankObj = new SerializedObject(((SFXManager)target).bank);
        clips = sfxBankObj.FindProperty("clips");
        volumes = sfxBankObj.FindProperty("volumes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SFXManager sfxManager = (SFXManager)target;

        showSFXList = EditorGUILayout.Foldout(showSFXList, "Global SFX");
        if(showSFXList && sfxManager.bank!=null)
        {
            SFXBankEditor.ShowClips(clips, volumes,sfxManager.bank);
        }
        if(sfxBankObj!=null)
            sfxBankObj.ApplyModifiedProperties();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);

        DrawDefaultInspector();
    }
}
#endif