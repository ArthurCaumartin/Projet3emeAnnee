using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GasTank : MonoBehaviour
{
    [SerializeField] private int _gazQuantity = 500;
    [SerializeField] private RectTransform _startButton;
    [SerializeField] private Button _launchWaveButton;
    [SerializeField] private SpriteRenderer _siphonVisual;

    private void Start()
    {
        GetComponent<TargetDetector>().TriggerEnvent.AddListener(SetStartButtonScale);
        _startButton.GetComponent<Button>().onClick.AddListener(StartTowerDefencePlacement);

        GetComponent<SpawnSiegeMob>().OnWaveEnd.AddListener(FinishSiphon);

        _launchWaveButton.onClick.AddListener(StartTowerDefence);
        _launchWaveButton.gameObject.SetActive(false);

        SetStartButtonScale(false);
    }

    public void StartTowerDefencePlacement()
    {
        _startButton.DOScale(Vector3.zero, .1f);
        PartyManager.instance.StartTowerDefencePlacement(transform);
        _launchWaveButton.gameObject.SetActive(true);
    }

    public void StartTowerDefence()
    {
        PartyManager.instance.StartTowerDefence();
        _launchWaveButton.gameObject.SetActive(false);
    }

    public void SetStartButtonScale(bool value)
    {
        _startButton.DOScale(value ? Vector3.one : Vector3.zero, .1f)
        .SetEase(Ease.Linear);
    }

    public void FinishSiphon()
    {
        print("Add gaz quantity");
        _siphonVisual.color = Color.white;
        PartyManager.instance.AddGas(_gazQuantity, true);
        PartyManager.instance.SetPartyState(PartyState.Mobile);
        GameManager.instance.ChangeDifficulty(true);
        gameObject.SetActive(false);
    }
}
