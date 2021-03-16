using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System;
using System.IO;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    ThreadedJob job = new ThreadedJob();
    FileStream file = new FileStream();
    public static string path;
    public Slider preloader;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteKey("rus");
        path = Path.Combine(Application.persistentDataPath, "rus.txt");
        Debug.Log(path);
        if (PlayerPrefs.HasKey("rus"))
        {
            Debug.Log("PlayerPrefs");
            
            StartCoroutine("WaytThread");
        }
        else
        {
            Debug.Log("Start");
            ServerController.onSuccessHandler += SuccessHandler;
            ServerController.onPreloadHandler += PreloadHandler;
            ServerController.GetFileFromLink("getRusDictionary", path);
        }
        
    }

    private void PreloadHandler(int bytes)
    {
        //Debug.Log((bytes  * 100) / 15006980);
        preloader.value = ((float)((bytes * 100) / 15006980))/100;
    }

    private void OnDestroy()
    {
        job = null;
    }
    void SuccessHandler(WWW www)
    {
        Debug.Log("SuccessHandler");
        PlayerPrefs.SetString("rus", "rus");
        
        //file.CreateFile(path, www.text);
        StartCoroutine("WaytThread");
        ServerController.onSuccessHandler -= SuccessHandler;
    }

    void ReadWords()
    {
        string line = file.GetContent(path);
        GLobalParametrs.dictionary = JsonUtility.FromJson<WordsModel>(line);
        
    }

    private void SetDictionary()
    {
        
        foreach (WordsModel.Word word in GLobalParametrs.dictionary.words)
        {
            if(!GLobalParametrs.dictionary.dict.ContainsKey(word.word.ToLower()))
                GLobalParametrs.dictionary.dict.Add( word.word.ToLower(), word.eng.ToLower());
        }
        
        
    }

    IEnumerator WaytThread()
    {
        yield return new WaitForSeconds(2);

        job.method = ReadWords;
        job.Start();
        yield return StartCoroutine(job.WaitFor());
        preloader.value = 0.25f;
        job.method = SetDictionary;
        job.Start();
        yield return StartCoroutine(job.WaitFor());
        preloader.value = 0.5f;
        Debug.Log("Words load complite");

        if (PlayerPrefs.HasKey("leter"))
            ReadLetter(PlayerPrefs.GetString("leter"));
        else
        {
            ServerController.onSuccessHandler += SuccessLetterHandler;
            ServerController.PostREquest("getRusLatterStatistic", null, false);
        }
    }

    void SuccessLetterHandler(WWW www)
    {
        ServerController.onSuccessHandler -= SuccessLetterHandler;
        ReadLetter(www.text);
    }

    void ReadLetter(string line)
    {
        GLobalParametrs.letters = JsonUtility.FromJson<LettersModel>(line);
        PlayerPrefs.SetString("leter", line);
        GLobalParametrs.letters.SetLeters();
        Debug.Log("Words load complite");
        preloader.value = 0.75f;

        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        preloader.value = 1f;
        SceneManager.LoadScene(1);
    }
}
