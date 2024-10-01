using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    private GasolineManager _gasolineManager;
    [SerializeField] private int _coreNumber;
    public int CoreNumber { get => _coreNumber; set => _coreNumber = value; }
    public TextMeshProUGUI _gasText, _coreText;

    public List<UpgradeSlot> _upgradeSlots = new List<UpgradeSlot>();

    private void Start()
    {
        _gasolineManager = GetComponentInParent<GasolineManager>();
        _gasText.text = _gasolineManager.Gasoline.ToString();
    }
    public bool BuyCore(int value)
    {
        if (CoreNumber < 4 && value <= _gasolineManager.Gasoline)
        {
            _gasolineManager.Gasoline -= value;
            _gasText.text = _gasolineManager.Gasoline.ToString();
            CoreNumber++;
            _coreText.text = CoreNumber + " / 4";
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetSlots()
    {
        for (int i = 0; i < _upgradeSlots.Count; i++)
        {
            _upgradeSlots[i].ResetSlot();
        }
    }

    public void SellCore(int value)
    {
        switch (value)
        {
            case 1:
                _gasolineManager.Gasoline += 500;
                break;
            case 2:
                _gasolineManager.Gasoline += 1000;
                break;
            case 3:
                _gasolineManager.Gasoline += 2000;
                break;
            case 4:
                _gasolineManager.Gasoline += 4000;
                break;
        }
        _gasText.text = _gasolineManager.Gasoline.ToString();
        
        CoreNumber--;
        _coreText.text = CoreNumber + " / 4";
    }

    public List<int> GetInfoUpgrades()
    {
        List<int> upgradesValues = new List<int>();
        
        for (int i = 0; i < _upgradeSlots.Count; i++)
        {
            DragDropResearch objectTempScript = _upgradeSlots[i]._objectDropped.GetComponent<DragDropResearch>();
            upgradesValues.Add(objectTempScript._upgradeValue);
        }

        return upgradesValues;
    }
}
