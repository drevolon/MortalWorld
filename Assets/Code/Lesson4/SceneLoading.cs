using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    [Header("����������� ������")]
    [SerializeField]
    private int _dataId;

    [Header("��������� �������")]
    [SerializeField]
    private Image _loadingImg;
    [SerializeField]
    private TMP_Text _progressText;

    void Start()
    {
        StartCoroutine(AsyncLoad());
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_dataId);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            _loadingImg.fillAmount = progress;
            _progressText.text = string.Format("{0:0}%",_progressText);
            yield return null;
        }
    }
    
}
