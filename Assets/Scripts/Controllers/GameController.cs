using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameController : MonoBehaviour
{
    public GameScene currentScene;
    public BottomBarController bottomBar;
    public SpriteSwitcher backgroundController;
    public ChooseController chooseController;
    public QuizController quizController; 
    public AudioController audioController;
    public PuzzleController puzzleController;
    public PopUpController popupController;
    public PesanController pesanController;

    private bool isPaused = false; 

    public DataHolder data;

    public string menuScene;
    public GameObject endingPanel; 

    private State state = State.IDLE;

    private List<StoryScene> history = new List<StoryScene>();

    private enum State
    {
        IDLE, ANIMATE, CHOOSE, QUIZ, PUZZLE, POPUP, PESAN 
    }

    void Start()
    {
        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            data.prevScenes.ForEach(scene =>
            {
                history.Add(this.data.scenes[scene] as StoryScene);
            });
            currentScene = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
            bottomBar.SetSentenceIndex(data.sentence - 1);
        }
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            history.Add(storyScene);
            bottomBar.PlayScene(storyScene, bottomBar.GetSentenceIndex());
            backgroundController.SetImage(storyScene.background);
            PlayAudio(storyScene.sentences[bottomBar.GetSentenceIndex()]);
        }
    }

    void Update()
    {
        if (state == State.IDLE) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (bottomBar.IsCompleted())
                {
                    bottomBar.StopTyping();
                    if (bottomBar.IsLastSentence())
                    {
                        PlayScene((currentScene as StoryScene).nextScene);
                    }
                    else
                    {
                        bottomBar.PlayNextSentence();
                        PlayAudio((currentScene as StoryScene)
                            .sentences[bottomBar.GetSentenceIndex()]);
                    }
                }
                else
                {
                    bottomBar.SpeedUp();
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (bottomBar.IsFirstSentence())
                {
                    if(history.Count > 1)
                    {
                        bottomBar.StopTyping();
                        bottomBar.HideSprites();
                        history.RemoveAt(history.Count - 1);
                        StoryScene scene = history[history.Count - 1];
                        history.RemoveAt(history.Count - 1);
                        PlayScene(scene, scene.sentences.Count - 2, false);
                    }
                }
                else
                {
                    bottomBar.GoBack();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                List<int> historyIndicies = new List<int>();
                history.ForEach(scene =>
                {
                    historyIndicies.Add(this.data.scenes.IndexOf(scene));
                });
                SaveData data = new SaveData
                {
                    sentence = bottomBar.GetSentenceIndex(),
                    prevScenes = historyIndicies
                };
                SaveManager.SaveGame(data);
                SceneManager.LoadScene(menuScene);
            }
        }
    }

    public void PlayScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        if (scene == null)
        {
            ShowEndingPanel();
        }
        else
        {
            StartCoroutine(SwitchScene(scene, sentenceIndex, isAnimated));
        }
    }

    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        state = State.ANIMATE;
        currentScene = scene;
        if (isAnimated)
        {
            bottomBar.Hide();
            yield return new WaitForSeconds(1f);
        }
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            history.Add(storyScene);
            PlayAudio(storyScene.sentences[sentenceIndex + 1]);
            if (isAnimated)
            {
                backgroundController.SwitchImage(storyScene.background);
                yield return new WaitForSeconds(1f);
                bottomBar.ClearText();
                bottomBar.Show();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                backgroundController.SetImage(storyScene.background);
                bottomBar.ClearText();
            }
            bottomBar.PlayScene(storyScene, sentenceIndex, isAnimated);
            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            chooseController.SetupChoose(scene as ChooseScene);
        }
        else if (scene is QuizScene) 
        {
            state = State.QUIZ;
            quizController.SetupQuiz(scene as QuizScene);
        }
        else if (scene is PuzzleScene) 
        {
            state = State.PUZZLE;
            puzzleController.SetupPuzzle(scene as PuzzleScene);
        }
        else if (scene is PopUpScene) 
        {
            state = State.POPUP;
            popupController.SetupPopUp(scene as PopUpScene);
        }
        else if (scene is PesanScene) 
        {
            state = State.PESAN;
            pesanController.SetupPesan(scene as PesanScene);
        }
    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }

    private void ShowEndingPanel()
    {
        endingPanel.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void NextScene()
{
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int nextSceneIndex = currentSceneIndex + 1;

    // Pastikan nextSceneIndex tidak melebihi jumlah total scene
    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    {
        // Simpan status chapter berikutnya di PlayerPrefs melalui GameManager
        GameManager.Instance.SetChapterCompletion(nextSceneIndex, true);
        Debug.Log("Unlocked Chapter " + nextSceneIndex);

        // Load the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }
    else
    {
        Debug.Log("No more scenes to load!");
    }
}
    public void NextSceneGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Pastikan nextSceneIndex tidak melebihi jumlah total scene
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No more scenes to load! Scene index out of range.");
        }
    }


    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Game Resumed");
    }
}
