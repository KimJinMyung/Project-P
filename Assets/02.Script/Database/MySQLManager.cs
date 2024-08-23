using System;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using UnityEngine.Windows;
using WebSocketSharp;
using static Unity.Burst.Intrinsics.X86.Avx;

public class MySQLManager : Singleton<MySQLManager>
{
    private AndroidJavaObject _androidJavaObject;
    private string connectionString;
    [SerializeField] private Text textCheck;

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

    public void ReadData()
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM test";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    //int age = Convert.ToInt32(reader["Age"]);
                    //Debug.Log("name: " + name);
                    //text.text = $"SELECT * FROM test \n name: {name}";
                    //Debug.Log("Name: " + name + ", Age: " + age);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to read data: " + ex.Message);
            }
        }
    }

    // Kotlin���� �Ѱܹ��� īī���� ���� ������
    public void GetUserData(string userData)
    {
        //userData = "3666640951||dls625@hanmail.net||.||https://img1.kakaocdn.net/thumb/R110x110.q70/?fname=https://t1.kakaocdn.net/account_images/default_profile.jpeg";

        InsertData("MemberInfo", userData);
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
                break;
            default:
                Debug.LogError("Invalid query type provided.");
                return;
        }
    }
}
