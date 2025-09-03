using UnityEngine;
using UnityEngine.SceneManagement; // Needed if you're loading scenes

public class GoToNextRoomInTime : MonoBehaviour
{
    [SerializeField] private float timeToWait = 5f; // Time before moving to next room
    [SerializeField] private string nextRoomName;   // The name of the next scene/room

    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToWait)
        {
            GoToNextRoom();
        }
    }

    private void GoToNextRoom()
    {
        if (!string.IsNullOrEmpty(nextRoomName))
        {
            SceneManager.LoadScene(nextRoomName);
        }
        else
        {
            Debug.LogWarning("Next room name is not set!");
        }
    }
}
