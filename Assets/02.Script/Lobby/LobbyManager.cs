using System;
using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Sprite[] lobbySprites; // 각 단계별 스프라이트 배열
    [SerializeField] private Image backgroundImage; // 배경 이미지

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = backgroundImage.GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        EventManager<DataEvents>.StartListening(DataEvents.UpdateLobby, UpdateLobbyBackground);
        EventManager<StageEvent>.StartListening<bool>(StageEvent.SetMiniGame, SetLobbyUI);
    }

    // Awake에서 호출 시 Null 에러
    private void Start()
    {
        // 게임 시작 시 로비 배경 업데이트
        UpdateLobbyBackground();
    }

    private void OnDisable()
    {
        // 이벤트 핸들러 해제
        EventManager<DataEvents>.StopListening(DataEvents.UpdateLobby, UpdateLobbyBackground);
        EventManager<StageEvent>.StopListening<bool>(StageEvent.SetMiniGame, SetLobbyUI);
    }

    // 현재 챕터와 스테이지에 따라 로비 배경 이미지를 변경하는 메서드
    private void UpdateLobbyBackground()
    {
        int currentChapter = PlayerInformation.Instance.PlayerViewModel.CurrentChapter;
        int currentStage = PlayerInformation.Instance.PlayerViewModel.CurrentStage;

        // DebugLogger.Log($"챕터: {currentChapter}");
        // DebugLogger.Log($"스테이지: {currentStage}");
        
        // 조건: 각 챕터의 특정 스테이지를 클리어했을 때만 배경 변경
        if (ShouldChangeBackground(currentChapter, currentStage))
        {
            // 스프라이트 인덱스를 챕터에 맞게 설정 (챕터별로 다른 이미지를 적용)
            int spriteIndex = GetSpriteIndex(currentChapter, currentStage);
            if (spriteIndex >= 0 && spriteIndex < lobbySprites.Length)
            {
                backgroundImage.sprite = lobbySprites[spriteIndex];
            }
            else
            {
                DebugLogger.LogError("스프라이트 인덱스 범위를 벗어났습니다.");
            }
        }
    }

    // 특정 챕터와 스테이지일 때 배경 변경 여부를 결정하는 메서드
    private bool ShouldChangeBackground(int chapter, int stage)
    {
        switch (chapter)
        {
            case 1:
                return stage == 10; // 1챕터의 10스테이지 클리어 시
            case 2:
            case 3:
            case 4:
                return stage == 1 || stage == 11 || stage == 21 || stage == 30; // 2, 3, 4챕터의 10, 20, 30스테이지 클리어 시
            default:
                return false;
        }
    }

    // 챕터와 스테이지에 맞는 스프라이트 인덱스를 반환하는 메서드
    private int GetSpriteIndex(int chapter, int stage)
    {
        if (chapter == 2)
        {
            if (stage == 1) return 1;
            if (stage == 11) return 2;
            if (stage == 21) return 3;
        }
        if (chapter == 3)
        {
            if (stage == 1) return 4;
            if (stage == 11) return 5;
            if (stage == 21) return 6;
        }
        if (chapter == 4)
        {
            if (stage == 1) return 7;
            if (stage == 11) return 8;
            if (stage == 21) return 9;
            if (stage == 30) return 10;
        }

        return -1; // 스프라이트가 없는 경우
    }

    private void SetLobbyUI(bool SetEnable)
    {
        _canvas.enabled = SetEnable;
    }
}
