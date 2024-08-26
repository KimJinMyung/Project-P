using System;
using System.Data;
using DG.Tweening.Core.Easing;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySQLManager : MonoBehaviour
{
    private static MySQLManager _instance;
    public static MySQLManager Instance
    {
        get
        {
            // ���� �ν��Ͻ��� null�̸�, ���ο� GameManager ������Ʈ�� ����
            if (_instance == null)
            {
                _instance = FindObjectOfType<MySQLManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<MySQLManager>();
                    singletonObject.name = typeof(MySQLManager).ToString() + " (Singleton)";

                    // GameManager ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private AndroidJavaObject _androidJavaObject;
    private string connectionString;
    [SerializeField] private Text textCheck;
    //[SerializeField] private GameObject DBDataManager_;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _androidJavaObject = new AndroidJavaObject("com.unity3d.player.KakaoLogin");
        // �����ͺ��̽� ���� ���ڿ� ����
        connectionString = "Server=3.38.178.218;Database=ProjectP;User ID=ubuntu;Password=P@ssw0rd!;Pooling=false;SslMode=None;AllowPublicKeyRetrieval=true;";
    }

    // type = Table �̸� str = �Ѱܹ��� || �� ���е� ����
    public void InsertData(string type, string str)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                // check��
                //textCheck.text += $"{str}\n";

                conn.Open();

                string[] strArr = str.Split("||");

                string query = $"INSERT INTO {type} (";

                string values = "VALUES (";

                switch (type)
                {
                    case "MemberInfo": // Kakao = ȸ����ȣ||�̸���||�г���||�����ʻ���URL
                        if (!string.IsNullOrEmpty(strArr[0]))
                        {
                            query += "MemberID";
                            values += "@MemberID";
                        }
                        if (!string.IsNullOrEmpty(strArr[1]))
                        {
                            query += ", Email";
                            values += ", @Email";
                        }
                        if (!string.IsNullOrEmpty(strArr[2]))
                        {
                            query += ", Nickname";
                            values += ", @Nickname";
                        }
                        if (!string.IsNullOrEmpty(strArr[3]))
                        {
                            query += ", ProfileURL";
                            values += ", @ProfileURL";
                        }
                        //if (!string.IsNullOrEmpty(strArr[4]))
                        //{
                        //    query += ", GuestPassword";
                        //    values += ", @GuestPassword";
                        //}
                        break;
                    case "Assets": // Kakao = ȸ����ȣ||�̸���||�г���||�����ʻ���URL
                        if (!string.IsNullOrEmpty(strArr[0]))
                        {
                            query += "MemberID";
                            values += "@MemberID";
                        }
                        if (!string.IsNullOrEmpty(strArr[1]))
                        {
                            query += ", Gold";
                            values += ", @Gold";
                        }
                        if (!string.IsNullOrEmpty(strArr[2]))
                        {
                            query += ", HeartTime";
                            values += ", @HeartTime";
                        }
                        if (!string.IsNullOrEmpty(strArr[3]))
                        {
                            query += ", ItemCount";
                            values += ", @ItemCount";
                        }
                        //if (!string.IsNullOrEmpty(strArr[4]))
                        //{
                        //    query += ", GuestPassword";
                        //    values += ", @GuestPassword";
                        //}
                        break;
                    default:
                        Debug.LogError("Invalid query type provided.");
                        return;
                }

                query += ") ";
                values += ")";

                query += values;

                // check��
                //textCheck.text += $"{query}\n";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                switch (type)
                {
                    case "MemberInfo":
                        if (!string.IsNullOrEmpty(strArr[0])) cmd.Parameters.AddWithValue("@MemberID", Int64.Parse(strArr[0]));
                        if (!string.IsNullOrEmpty(strArr[1])) cmd.Parameters.AddWithValue("@Email", strArr[1]);
                        if (!string.IsNullOrEmpty(strArr[2])) cmd.Parameters.AddWithValue("@Nickname", strArr[2]);
                        if (!string.IsNullOrEmpty(strArr[3])) cmd.Parameters.AddWithValue("@ProfileURL", strArr[3]);
                        //if (!string.IsNullOrEmpty(strArr[4])) cmd.Parameters.AddWithValue("@GuestPassword", "");
                        break;
                    case "Assets":
                        if (!string.IsNullOrEmpty(strArr[0])) cmd.Parameters.AddWithValue("@MemberID", Int64.Parse(strArr[0]));
                        if (!string.IsNullOrEmpty(strArr[1])) cmd.Parameters.AddWithValue("@Gold", strArr[1]);
                        if (!string.IsNullOrEmpty(strArr[2])) cmd.Parameters.AddWithValue("@HeartTime", strArr[2]);
                        if (!string.IsNullOrEmpty(strArr[3])) cmd.Parameters.AddWithValue("@ItemCount", strArr[3]);
                        //if (!string.IsNullOrEmpty(strArr[4])) cmd.Parameters.AddWithValue("@GuestPassword", "");
                        break;
                    default:
                        Debug.LogError("Invalid query type provided.");
                        return;
                }

                cmd.ExecuteNonQuery();

                // ���� �α��ν� DBDataManager�� Dictionary ������ ����
                InputDataAtDictionary(type,str);

                Debug.Log("Data Inserted Successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to insert data: " + ex.Message);
            }
        }
    }

    public void ReadData(string str)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                //str = "MemberInfo||3666640951";

                // üũ��
                //textCheck.text += $"{str}\n";

                // strArr[0]�� ���̺�� strArr[1]�� ȸ����ȣ ����
                string[] strArr = str.Split("||");

                string query = string.Empty;

                conn.Open();

                switch (strArr[0])
                {
                    case "MemberInfo": // Kakao = ȸ����ȣ||�̸���||�г���||�����ʻ���URL
                        query = $"SELECT * FROM {strArr[0]} WHERE MemberID = {strArr[1]}";
                        // üũ��
                        // textCheck.text += $"{query}\n";
                        break;
                    case "Assets": // Kakao = ȸ����ȣ||�̸���||�г���||�����ʻ���URL
                        query = $"SELECT * FROM {strArr[0]} WHERE MemberID = {strArr[1]}";
                        break;
                    default:
                        Debug.LogError("Invalid query type provided.");
                        return;
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    switch (strArr[0])
                    {
                        case "MemberInfo": // Kakao = ȸ����ȣ||�̸���||�г���||�����ʻ���URL
                            string MemberID = reader["MemberID"].ToString();
                            string Email = reader["Email"].ToString();
                            string Nickname = reader["Nickname"].ToString();
                            string ProfileURL = reader["ProfileURL"].ToString();

                            // üũ��
                            //textCheck.text += $"{MemberID}\n{Email}\n{Nickname}\n{ProfileURL}";

                            InputDataAtDictionary("MemberInfo", $"{MemberID}||{Email}||{Nickname}||{ProfileURL}");
                            break;
                        case "Assets": // Kakao = ȸ����ȣ||�̸���||�г���||�����ʻ���URL
                            string ID = reader["MemberID"].ToString();
                            string Gold = reader["Gold"].ToString();
                            string HeartTime = reader["HeartTime"].ToString();
                            string ItemCount = reader["ItemCount"].ToString();

                            // üũ��
                            //textCheck.text += $"{MemberID}\n{Email}\n{Nickname}\n{ProfileURL}";

                            InputDataAtDictionary("Assets", $"{ID}||{Gold}||{HeartTime}||{ItemCount}");
                            break;
                        default:
                            Debug.LogError("Invalid query type provided.");
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to read data: " + ex.Message);
            }
        }
    }

    // Kotlin���� �Ѱܹ��� īī���� ���� ������
    public void GetAndSetUserData(string userData)
    {
        //userData = "3666640951||dls625@hanmail.net||.||https://img1.kakaocdn.net/thumb/R110x110.q70/?fname=https://t1.kakaocdn.net/account_images/default_profile.jpeg";

        InsertData("MemberInfo", userData);

        // ������ �Ұ�!
        //InsertData("Assets", userData);
    }

    public void InputDataAtDictionary(string type, string str)
    {
        string[] strArr = str.Split("||");

        switch (type)
        {
            case "MemberInfo":
                DBDataManager.Instance.UserData.Add("MemberID",strArr[0]);
                DBDataManager.Instance.UserData.Add("Email", strArr[1]);
                DBDataManager.Instance.UserData.Add("Nickname", strArr[2]);
                DBDataManager.Instance.UserData.Add("ProfileURL", strArr[3]);
                DBDataManager.Instance.ShowDicDataCheck("UserData");
                // GuestLogin���� �Ⱥҷ�������, ���⼭ Assets ������ Call.
                ReadData($"Assets||{strArr[0]}");
                break;
            case "Assets":
                DBDataManager.Instance.UserAssetsData.Add("MemberID", strArr[0]);
                DBDataManager.Instance.UserAssetsData.Add("Gold", strArr[1]);
                DBDataManager.Instance.UserAssetsData.Add("HeartTime", strArr[2]);
                DBDataManager.Instance.UserAssetsData.Add("ItemCount", strArr[3]);
                DBDataManager.Instance.ShowDicDataCheck("Assets");
                // �� �ҷ����� Scene Change
                SceneManager.LoadScene("Jinmyung");
                break;
            default:
                Debug.LogError("Invalid query type provided.");
                return;
        }

    }
}
