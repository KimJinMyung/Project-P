using DataStruct;
using EnumTypes;
using EventLibrary;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldPackageSlot : MonoBehaviour
{
    [FoldoutGroup("Gold Shop UI")][SerializeField] TMP_Text Text_PriceERC;
    [FoldoutGroup("Gold Shop UI")][SerializeField] TMP_Text Text_PackageName;
    [FoldoutGroup("Gold Shop UI")][SerializeField] Image Image_PackageIcon;

    private GoldPackageData PackageInfo;

    private void Awake()
    {
        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        EventManager<UIEvents>.StartListening(UIEvents.OnClickEnableGoldBuyPopup, PopUpOn);
        EventManager<UIEvents>.StartListening(UIEvents.OnClickGoldBuyExit, PopUpOff);
    }
    private void RemoveEvent()
    {
        EventManager<UIEvents>.StopListening(UIEvents.OnClickEnableGoldBuyPopup, PopUpOn);
        EventManager<UIEvents>.StopListening(UIEvents.OnClickGoldBuyExit, PopUpOff);
    }

    //private void Start()
    //{
    //    Canvas.gameObject.SetActive(false);
    //}

    //��Ű�� ���� �ʱ�ȭ
    public void SetPackageInfo(GoldPackageData packageData)
    {
        PackageInfo = packageData;

        Text_PackageName.text = PackageInfo.Name;
        Text_PriceERC.text = PackageInfo.ERCPrice.ToString();
    }

    //���� â PopUp On
    private void PopUpOn()
    {
        this.gameObject.SetActive(true);
    }

    //���� â PopUp Off
    private void PopUpOff()
    {
        this.gameObject.SetActive(false);
    }

    //��� ����
    public void BuyGold_ERC()
    {
        //ERC ����
        EventManager<UIEvents>.TriggerEvent(UIEvents.OnClickGoldBuyButton, PackageInfo);
        // ERC ���� �Һ� �ڵ� ����

        //��� ȹ��
        EventManager<GoldEvent>.TriggerEvent(GoldEvent.OnGetGold, PackageInfo.GiveGold);

        //��� ���� �ݱ�
        //EventManager<UIEvents>.TriggerEvent(UIEvents.GoldStoreExit);
    }
}
