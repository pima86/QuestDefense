using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GPGSManager : MonoBehaviour
{
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    // 구글 로그인!
    internal void ProcessAuthentication(SignInStatus status)
    {
        //로그인 성공
        if (status == SignInStatus.Success)
            LoadData();
    }

    #region Save
    public void SaveData() // 외부에서 접근할 함수
    {
        OpenSaveGame();
    }


    private void OpenSaveGame()
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;

        // 데이터 접근
        saveGameClient.OpenWithAutomaticConflictResolution("saveGPGS",
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            onsavedGameOpend);
    }


    private void onsavedGameOpend(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (status == SavedGameRequestStatus.Success)
        {
            var update = new SavedGameMetadataUpdate.Builder().Build();

            //json
            var json = JsonUtility.ToJson("구매했음");
            byte[] data = Encoding.UTF8.GetBytes(json);

            // 저장 함수 실행
            saveGameClient.CommitUpdate(game, update, data, OnSavedGameWritten);
        }
        else
        {
            Debug.Log("Save No.....");
        }
    }

    // 저장 확인 
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // 저장완료부분
            Debug.Log("Save End");
        }
        else
        {
            Debug.Log("Save nonononononono...");
        }
    }
    #endregion
    #region Load
    public void LoadData()
    {
        OpenLoadGame();
    }

    private void OpenLoadGame()
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;

        saveGameClient.OpenWithAutomaticConflictResolution("saveGPGS",
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLastKnownGood,
            LoadGameData);
    }

    private void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient saveGameClient = PlayGamesPlatform.Instance.SavedGame;

        // 데이터 로드
        if (status == SavedGameRequestStatus.Success)
            saveGameClient.ReadBinaryData(data, onSavedGameDataRead);
    }

    private void onSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        //첫 로그인
        if (data == "")
        {
            SaveData();
        }
        else
        {
            if (data == "구매했음")
                IAPManager.Inst.BuyAdsBlocking();
        }
    }
    #endregion
}
