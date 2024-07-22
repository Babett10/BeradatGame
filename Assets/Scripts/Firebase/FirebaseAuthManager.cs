using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Unity.VisualScripting;
using TMPro;

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public static FirebaseUser user;
    public DatabaseReference databaseReference;

    // Login Variables
    [Space]
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public InputField nameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField confirmPasswordRegisterField;

    [Header("UI")]
    public GameObject loginFailedPanel;
    public GameObject registrationSuccess;
    public GameObject registFailedPanel;
    public TMP_Text loginFailedText;
    public TMP_Text registrationSuccessText;
    public TMP_Text registFailedText;

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }

    void InitializeFirebase()
    {
        // Set the default instance object
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            UIManager.Instance.OpenAuthPanel();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            References.userName = user.DisplayName;
            UIManager.Instance.OpenMainMenuPanel();
        }
        else
        {
            UIManager.Instance.OpenAuthPanel();
        }
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                UIManager.Instance.OpenAuthPanel();
                ClearLoginInputFieldText();
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    private void ClearLoginInputFieldText()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void Logout()
    {
        if (auth != null && user != null)
        {
            auth.SignOut();
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
            ShowLoginFailedPanel(failedMessage);
        }
        else
        {
            AuthResult authResult = loginTask.Result;
            user = authResult.User;

            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

            References.userName = user.DisplayName;
            SaveUserData(user.UserId, user.DisplayName, user.Email);
            UIManager.Instance.OpenMainMenuPanel();
        }
    }

    public void Register()
    {
        if (string.IsNullOrEmpty(nameRegisterField.text))
        {
            ShowRegistFailedPanel("User Name is empty");
            return;
        }
        else if (string.IsNullOrEmpty(emailRegisterField.text))
        {
            ShowRegistFailedPanel("Email field is empty");
            return;
        }
        else if (string.IsNullOrEmpty(passwordRegisterField.text))
        {
            ShowRegistFailedPanel("Password field is empty");
            return;
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            ShowRegistFailedPanel("Password does not match");
            return;
        }
        else
        {
            StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
        }
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogError(registerTask.Exception);

            FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Registration Failed! Becuase ";
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Registration Failed";
                    break;
            }

            ShowRegistFailedPanel(failedMessage);
            Debug.Log(failedMessage);
        }
        else
        {
            // Get The User After Registration Success
            AuthResult authResult = registerTask.Result;
            FirebaseUser newUser = authResult.User;

            UserProfile userProfile = new UserProfile { DisplayName = name };

            var updateProfileTask = newUser.UpdateUserProfileAsync(userProfile);

            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

            if (updateProfileTask.Exception != null)
            {
                // Delete the user if user update failed
                newUser.DeleteAsync();

                Debug.LogError(updateProfileTask.Exception);

                FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Profile update Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Profile update Failed";
                        break;
                }

                Debug.Log(failedMessage);
                ShowRegistFailedPanel(failedMessage);
            }
            else
            {
                Debug.Log("Registration Sucessful Welcome " + newUser.DisplayName);
                ShowRegistrationSuccessPanel(newUser.DisplayName);
                SaveUserData(newUser.UserId, newUser.DisplayName, newUser.Email);
                UIManager.Instance.OpenLoginPanel();
            }
        }
    }

    private void ShowRegistrationSuccessPanel(string displayName)
    {
        if (registrationSuccess != null)
        {
            registrationSuccessText.text = "Registration Successful Welcome " + displayName;
            registrationSuccess.SetActive(true);
            StartCoroutine(CloseRegistrationSuccessPanel());
        }
    }

    private IEnumerator CloseRegistrationSuccessPanel()
    {
        yield return new WaitForSeconds(2f);
        if (registrationSuccess != null)
        {
            registrationSuccess.SetActive(false);
        }
    }

    private void ShowRegistFailedPanel(string text)
    {
        if (registFailedPanel != null)
        {
            registFailedText.text = text;
            registFailedPanel.SetActive(true);
            StartCoroutine(CloseRegistFailedPanel());
        }
    }

    private IEnumerator CloseRegistFailedPanel()
    {
        yield return new WaitForSeconds(2f);
        if (registFailedPanel != null)
        {
            registFailedPanel.SetActive(false);
        }
    }

    private void ShowLoginFailedPanel(string text)
    {
        if (loginFailedPanel != null)
        {
            loginFailedText.text = text;
            loginFailedPanel.SetActive(true);
            StartCoroutine(CloseLoginFailedPanel());
        }
    }

    private IEnumerator CloseLoginFailedPanel()
    {
        yield return new WaitForSeconds(2f);
        if (loginFailedPanel != null)
        {
            loginFailedPanel.SetActive(false);
        }
    }

    private void SaveUserData(string userId, string userName, string email)
    {
        // Create a new user object
        User newUser = new User
        {
            userId = userId,
            userName = userName,
            email = email,
            // Tambahkan properti lain jika diperlukan
        };

        // Serialize user object to JSON
        string json = JsonUtility.ToJson(newUser);

        // Write user data to Firebase Realtime Database
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to save user data to Firebase Database: " + task.Exception);
            }
            else
            {
                Debug.Log("User data saved successfully to Firebase Database");
            }
        });
    }
    public void ResetPlayerPrefs()
    {
        // Hapus semua PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Set stage 1 agar terbuka
        PlayerPrefs.SetInt("Stage1", 1);
        PlayerPrefs.Save();

        Debug.Log("All stages have been reset. Only Stage 1 is unlocked.");
    }
}

// User class definition
public class User
{
    public string userId;
    public string userName;
    public string email;
    // Tambahkan properti lain jika diperlukan
}
