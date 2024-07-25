using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int MaxNumberOfShots = 3;

    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;

    private int _usedNumberOfShots;

    private IconHandler _iconHandler;

    private List<Pig> _pigs = new List<Pig>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _iconHandler = FindObjectOfType<IconHandler>();

        Pig[] pigs = FindObjectsOfType<Pig>();
        for (int i=0; i < pigs.Length; i++)
        {
            _pigs.Add(pigs[i]);
        }
    }

    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);

        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        if(_usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        if (_usedNumberOfShots == MaxNumberOfShots) 
        { 
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);

        if (_pigs.Count == 0)
        {
            WinGame();
        }

        else
        {
            RestartGame();
        }

    }

    public void RemovePig(Pig pig)
    {
        _pigs.Remove(pig);
        CheckForAllDeadPigs();
    }

    private void CheckForAllDeadPigs()
    {
        if (_pigs.Count == 0)
        {
            WinGame();
        }
    }

    #region Win/Lose

    private void WinGame()
    {
        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
