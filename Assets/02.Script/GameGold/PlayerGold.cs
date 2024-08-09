using EnumTypes;
using DataStruct;
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

    [FoldoutGroup("Payment UI")][SerializeField] private GameObject PaymentPopup;

    public Canvas canvas { get; private set; }

    private float GoldValue;    // Player�� ������ �ִ� Gold�� ����
    private float ERCValue;     // Player�� ������ �ִ� ERC�� ����

    protected new void Awake()
    {
        base.Awake();

        canvas = GetComponentInParent<Canvas>();

        ReadPlayerCapital();

        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void AddEvents()
    {
        EventManager<UIEvents>.StartListening<ItemData, float>(UIEvents.OnClickItemBuyButton, BuyItem_Gold);
        EventManager<UIEvents>.StartListening<GoldPackageData>(UIEvents.OnClickGoldBuyButton, BuyItem_ERC);
        EventManager<GoldEvent>.StartListening<float>(GoldEvent.OnGetGold, GetGold);
    }

    private void RemoveEvents()
    {
        EventManager<UIEvents>.StopListening<ItemData, float>(UIEvents.OnClickItemBuyButton, BuyItem_Gold);
        EventManager<UIEvents>.StartListening<GoldPackageData>(UIEvents.OnClickGoldBuyButton, BuyItem_ERC);
        EventManager<GoldEvent>.StopListening<float>(GoldEvent.OnGetGold, GetGold);
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
    private void BuyItem_Gold(ItemData itemInfo, float Count)
    {
        if (GoldValue < itemInfo.GoldPrice * Count)
        {
            //BuyItem_ERC(itemInfo, Count);
            //EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickGoldBuyButton, itemInfo, Count);

            // �˾� â ����
            EventManager<UIEvents>.TriggerEvent(UIEvents.GoldStorePopup);
        }
        else
        {
            GoldValue -= itemInfo.GoldPrice * Count;
            EventManager<DataEvents>.TriggerEvent(DataEvents.OnPaymentSuccessful, true);
        }

        UpdateUIText();
    }

    //������ ���� - ERC (Gold�� ���� ���ݺ��� ������)
    private void BuyItem_ERC(GoldPackageData itemInfo)
    {
        if (ERCValue < itemInfo.ERCPrice)
        {
            DebugLogger.Log("�ݾ��� �����մϴ�.");

            //�ݾ� ���� Popup On
            EventManager<DataEvents>.TriggerEvent(DataEvents.OnPaymentSuccessful, false);

            return;
        }
        else
        {
            ERCValue -= itemInfo.ERCPrice;

            //��� �Ϸ� PopUp On 
            EventManager<DataEvents>.TriggerEvent(DataEvents.OnPaymentSuccessful, true);
        }


        UpdateUIText();
    }

    private void GetGold(float getGold)
    {
        GoldValue += getGold;

        UpdateUIText();
    }
}
