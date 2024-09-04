using DataStruct;
using EnumTypes;
using EventLibrary;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_PopUp_BuyItem : MonoBehaviour
{
    [FoldoutGroup("UI Prefab")] [SerializeField] private Image itemIcon;    //구매할 아이템의 아이콘 표시 UI
    [FoldoutGroup("UI Prefab")] [SerializeField] private TMP_Text textItemPrice;   //구매할 아이템의 가격 표시 UI
    [FoldoutGroup("UI Prefab")] [SerializeField] private TMP_Text textBuyItemCount;    //구매할 아이템의 갯수 표시 UI
    
    private ItemData _data;     //구매할 아이템의 정보
    private float _buyItemCount; //구매할 아이템의 갯수

    private void Awake()
    {
        EventManager<UIEvents>.StartListening(UIEvents.OnClickEnableItemBuyPopup, PopUpOn);
        EventManager<UIEvents>.StartListening(UIEvents.OnClickChangeBuyItemCount, UpdateBuyItemText);
        EventManager<DataEvents>.StartListening<ItemData>(DataEvents.OnItemDataLoad, SetBuyItem);
    }

    private void Start()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager<UIEvents>.StopListening(UIEvents.OnClickEnableItemBuyPopup, PopUpOn);
        EventManager<UIEvents>.StopListening(UIEvents.OnClickChangeBuyItemCount, UpdateBuyItemText);
        EventManager<DataEvents>.StopListening<ItemData>(DataEvents.OnItemDataLoad, SetBuyItem);
    }

    private void OnEnable()
    {
        _buyItemCount = 1f;
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickChangeBuyItemCount);
    }

    //구매 창 PopUp On
    private void PopUpOn()
    {
        this.transform.parent.gameObject.SetActive(true);
    }

    //구매 창 PopUp Off
    public void PopUpOff()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    //구매할 아이템 초기화
    private void SetBuyItem(ItemData item)
    {
        _data = item;
    }

    //구매 갯수 증가
    public void Plus_BuyItemCount()
    {
        _buyItemCount = Mathf.Clamp(_buyItemCount + 1, 1, 99);
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickChangeBuyItemCount);
    }

    //구매 갯수 감소
    public void Minus_BuyItemCount()
    {
        _buyItemCount = Mathf.Clamp(_buyItemCount - 1, 1, 99);
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickChangeBuyItemCount);
    }

    //구매할 아이템 정보 표시 UI Update
    private void UpdateBuyItemText()
    {
        textItemPrice.text = (_data.GoldPrice * _buyItemCount).ToString();
        textBuyItemCount.text = _buyItemCount.ToString();
    }

    //아이템 구매
    public void BuyItem_Gold()
    {
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickItemBuyButton, _data, _buyItemCount);
    }
}