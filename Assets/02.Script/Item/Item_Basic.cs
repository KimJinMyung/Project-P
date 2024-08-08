using EnumTypes;
using EventLibrary;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//����׿�
public enum ItemID
{
    I1001,
    I1002, I1003, I1004,
    I1005,
}

public enum PaymentMethod
{
    Gold,
    ERC
}

public class Item_Basic : MonoBehaviour
{
    //[SerializeField] private ItemID ItemID;
    //����� ��
    [SerializeField] ItemID TempItemID;
    [SerializeField] string TempName;
    [SerializeField] float TempItemGoldPrice;
    [SerializeField] float TempItemERCPrice;

    [FoldoutGroup("Shop UI")][SerializeField] TMP_Text Text_Name;        // ���� ������ �̸� UI ǥ��
    [FoldoutGroup("Shop UI")][SerializeField] TMP_Text Text_GoldPrice;   // ���� ������ ��� ���� UI ǥ��

    private ItemData ItemInfo;      // �������� ����

    //������ ���� ����
    private void SetItemInfo()
    {
        //����׿�
        ItemInfo = new ItemData();

        ItemInfo.Name = TempName;
        ItemInfo.GoldPrice = TempItemGoldPrice;
        ItemInfo.ERCPrice = TempItemERCPrice;

        Text_Name.text = TempName;
        Text_GoldPrice.text = TempItemGoldPrice.ToString();
        //Text_ERCPrice.text = TempItemERCPrice.ToString();

        //Debug.Log(ItemID.I1001.ToString() == "I1001");
    }

    private void Awake()
    {
        SetItemInfo();
    }

    private void OnDestroy()
    {
        
    }
    
    private void AddEvents()
    {

    }

    private void RemoveEvents()
    {

    }

    //BuyItemUIPopUp
    public void BuyItem_Gold()
    {
        EventManager<UIEvents>.TriggerEvent<ItemData>(UIEvents.OnClickBuyItem, ItemInfo);
    }

    //public void BuyItem_ERC()
    //{
    //    EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickStart, ItemInfo, PaymentMethod.ERC);
    //}
}
