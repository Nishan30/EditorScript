using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public class LoginScreen : EditorWindow
{
    string email = "";
    string password = "";
    string token = "";
    string prompt = "";

    [MenuItem("Custom/Login")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LoginScreen));
    }

    private void OnGUI()
    {
        email = EditorGUILayout.TextField("Email:", email);
        password = EditorGUILayout.PasswordField("Password:", password);
        prompt = EditorGUILayout.TextField("Prompt:", prompt);

        if (GUILayout.Button("Login"))
        {
            WWWForm formData = new WWWForm();
            formData.AddField("email", email);
            formData.AddField("password", password);
            UnityWebRequest loginRequest = UnityWebRequest.Post("https://engin-webapp.azurewebsites.net/v1/signin", formData);
            loginRequest.SendWebRequest().completed += operation =>
            {
                if (loginRequest.result == UnityWebRequest.Result.ConnectionError || loginRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Failed");
                }
                else
                {
                    if(loginRequest.result.ToString() == "Success")
                    {
                        string message = loginRequest.downloadHandler.text;
                        Debug.Log(message);
                        WWWForm formData = new WWWForm();
                        formData.AddField("prompt", prompt);
                        UnityWebRequest promptPost = UnityWebRequest.Post("https://engin-webapp.azurewebsites.net/v1/prompt", formData);

                        promptPost.SendWebRequest().completed += operation =>
                        {
                            if (promptPost.result == UnityWebRequest.Result.ConnectionError || promptPost.result == UnityWebRequest.Result.ProtocolError)
                            {
                                Debug.LogError(promptPost.error);
                            }
                            else
                            {
                                string promptData = promptPost.downloadHandler.text;
                                Debug.Log(promptData);
                                UnityWebRequest promptGet = UnityWebRequest.Get("https://engin-webapp.azurewebsites.net/v1/user/prompt");
                                promptGet.SendWebRequest().completed += operation =>
                                {
                                    if (promptGet.result == UnityWebRequest.Result.ConnectionError || promptGet.result == UnityWebRequest.Result.ProtocolError)
                                    {
                                        Debug.LogError(promptGet.error);
                                    }
                                    else
                                    {
                                        string promptReceive= promptGet.downloadHandler.text;
                                        Debug.Log(promptReceive );
                                    }
                                };
                            }
                        };
                    }
                       
                }
            };
              
        }
    }

}