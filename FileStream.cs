using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileStream
{
    #region Crete file
    public void CreateFile(string path, string filecontent)
    {
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(filecontent);
            }
        }
    }
    public void CreateFileFromBytes(string path, byte[] data)
    {
        if (!File.Exists(path))
        {
            File.WriteAllBytes(path, data);
        }
    }
    #endregion
    #region Read File
    public string GetContent(string path)
    {
        
        string content = "";
        
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                content += s;
            }
        }
        return content;
    }
    #endregion
}
