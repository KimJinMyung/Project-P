using EnumTypes;
using EventLibrary;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopUp_BuyItem : MonoBehaviour
{
    private ItemData _data;     //������ �������� ����
    private float BuyItemCount; //������ �������� ����

    [FoldoutGroup("UI Prefab")]
    [SerializeField] private Image ItemIcon;    //������ �������� ������ ǥ�� UI
    [FoldoutGroup("UI Prefab")]
    [SerializeField] private TMP_Text Text_ItemPrice;   //������ �������� ���� ǥ�� UI
    [FoldoutGroup("UI Prefab")]
    [SerializeField] private TMP_Text Text_BuyItemCount;    //������ �������� ���� ǥ�� UI

    private void Awake()
    {
        EventManager<UIEvents>.StartListening(UIEvents.OnClickChangeBuyItemCount, UpdateBuyItemText);
        EventManager<UIEvents>.StartListening<ItemData>(UIEvents.OnClickBuyItem, SetBuyItem);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager<UIEvents>.StopListening(UIEvents.OnClickChangeBuyItemCount, UpdateBuyItemText);
        EventManager<UIEvents>.StopListening<ItemData>(UIEvents.OnClickBuyItem, SetBuyItem);
    }

    private void OnEnable()
    {
        BuyItemCount = 1f;
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickChangeBuyItemCount);
    }

    //������ ������ �ʱ�ȭ
    private void SetBuyItem(ItemData item)
    {
        _data = item;
    }

    //���� ���� ����
    public void Plus_BuyItemCount()
    {
        BuyItemCount = Mathf.Clamp(BuyItemCount + 1, 0, 99);
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickChangeBuyItemCount);
    }

    //���� ���� ����
    public void Minus_BuyItemCount()
    {
        BuyItemCount = Mathf.Clamp(BuyItemCount - 1, 0, 99);
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickChangeBuyItemCount);
    }

    //������ ������ ���� ǥ�� UI Update
    private void UpdateBuyItemText()
    {
        Text_ItemPrice.text = (_data.GoldPrice * BuyItemCount).ToString();
        Text_BuyItemCount.text = BuyItemCount.ToString();
    }

    //������ ����
    public void BuyItem_Gold()
    {
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickStart, _data, BuyItemCount);
    }
}
