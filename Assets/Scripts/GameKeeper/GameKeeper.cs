using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameKeeper : MonoBehaviour
{
    public static GameKeeper Instance;

    [SerializeField] private Snake _snake;
    [SerializeField] private TMP_Text _gameOverView;
    [SerializeField] private TMP_Text _restartView;

    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _fadeOutTime;

    private readonly float _restartTime = 5f;
    private bool _isFaded = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _snake.IsDeadthOccured += StopGame;
    }

    private void OnDisable()
    {
        _snake.IsDeadthOccured -= StopGame;
    }

    private void StopGame(bool isSnakeAlive)
    {
        if (isSnakeAlive == false)
        {
            StartCoroutine(FadeIn(_fadeInTime, _gameOverView));
            StartCoroutine(FadeOut(_fadeOutTime, _gameOverView));
            StartCoroutine(RestartLevel(_restartTime));
        }
    }

    private IEnumerator RestartLevel(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private IEnumerator FadeIn(float timeSpeed, TMP_Text text, float alpha = 0f)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        alpha = 1f;

        while (text.color.a < alpha)
        {
            Debug.Log(text.color.a);
            text.color = new Color(text.color.r, text.color.g, text.color.b, IncreaseAlpha(timeSpeed, text));
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FadeOut(float timeSpeed, TMP_Text text, float alpha = 1f)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        alpha = 0f;
       
        while (text.color.a > alpha)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, DecreaseAlpha(timeSpeed, text));
            yield return new WaitForFixedUpdate(); 
        }
    }

    private float IncreaseAlpha(float timeSpeed, TMP_Text text)
    {
        return text.color.a + (Time.deltaTime / timeSpeed);
    }

    private float DecreaseAlpha(float timeSpeed, TMP_Text text)
    {
        return text.color.a - (Time.deltaTime / timeSpeed);
    }
}
 