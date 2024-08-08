using EnumTypes;
using EventLibrary;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGold : Singleton<PlayerGold>
{
    [FoldoutGroup("Player Gold UI")][SerializeField] private TMP_Text GoldPrice;    // ��� UI ǥ��
    [FoldoutGroup("Player Gold UI")][SerializeField] private TMP_Text ERCPrice;     // ERC UI ǥ��

    private float GoldValue;    // Player�� ������ �ִ� Gold�� ����
    private float ERCValue;     // Player�� ������ �ִ� ERC�� ����

    protected new void Awake()
    {
        base.Awake();

        ReadPlayerCapital();

        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void AddEvents()
    {
        EventManager<UIEvents>.StartListening<ItemData, float>(UIEvents.OnClickStart, BuyItem_Gold);
    }

    private void RemoveEvents()
    {
        EventManager<UIEvents>.StopListening<ItemData, float>(UIEvents.OnClickStart, BuyItem_Gold);
    }

    //DataManager���� Data�� �о����
    private void ReadPlayerCapital()
    {
        //����׿�
        GoldValue = 10000;
        ERCValue = 1000;

        UpdateUIText();
    }

    // Player �ڿ� UI ������Ʈ
    private void UpdateUIText()
    {
        GoldPrice.text = GoldValue.ToString();
        ERCPrice.text = ERCValue.ToString();
    }

    // ������ ���� - ��� (�⺻)
    public void BuyItem_Gold(ItemData itemInfo, float Count)
    {
        if (GoldValue < itemInfo.GoldPrice * Count)
        {
            BuyItem_ERC(itemInfo, Count);
        }
        else
            GoldValue -= itemInfo.GoldPrice * Count;

        UpdateUIText();
    }

    //������ ���� - ERC (Gold�� ���� ���ݺ��� ������)
    private void BuyItem_ERC(ItemData itemInfo, float Count)
    {
        if (ERCValue < itemInfo.ERCPrice * Count)
        {
            DebugLogger.Log("�ݾ��� �����մϴ�.");
            return;
        }
        else
            ERCValue -= itemInfo.ERCPrice * Count;
    }
}
