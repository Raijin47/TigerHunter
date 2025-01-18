using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [SerializeField] private Transform _poolContainer;

    [SerializeField] private GameAudio _audio;

    [SerializeField] private Locator _locator;

    [SerializeField] private Wallet _wallet;
    [SerializeField] private AudioSpriteSwap _audioSettings;

    private readonly GameAction GameAction = new();
    private readonly Bootstrap Bootstrap = new();
    private readonly SaveService SaveService = new();

    public static GameAudio Audio;
    public static Wallet Wallet;
    public static GameAction Action;
    public static Locator Locator;
    public static SaveService Data;

    public AudioSpriteSwap AudioSettings => _audioSettings;
    public Transform PoolContainer => _poolContainer;

    private void Awake()
    {
        Instance = this;
        Audio = _audio;
        Wallet = _wallet;
        Action = GameAction;
        Locator = _locator;
        Data = SaveService;
    }

    private void Start()
    {
        _audioSettings.Init();
        _audio.Init();
        SaveService.LoadingData();
        _wallet.Init();

        Bootstrap.Init();
    }

    public void OnStart() => Action.SendStart();
    public void OnLose() => Action.SendLose();
    public void OnRestart() => Action.SendRestart();
    public void OnEnter() => Action.SendEnter();
    public void OnExit() => Action.SendExit();
    public void OnPause(bool pause) => Action.SendPause(pause);
    public void OnWin() => Action.SendWin();
    public void OnNext() => Action.SendNext();
    public void AddMoney(int value) => Wallet.Add(value);
    public void SaveProgress() => SaveService.SaveProgress();
}