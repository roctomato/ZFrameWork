using System;
using UnityEngine;
using System.IO;

public class LuaCustomLoader
{
    public static byte[] LoaderFromLoacalFile(ref string filePath)
    {
        //AppLog.Debug("load lua file:" + filePath);

        //文件所在的绝对路径
        filePath = filePath.Replace(".", "/");
        string[] folds = { "/script/", "/ReuseScript/"};
        string path = null;
        
        for( int i = 0; i < folds.Length; i ++ ){
            string cur_path = Application.dataPath + folds[i] + filePath + ".lua";
            if (File.Exists(cur_path))
            {
                path = cur_path;
               
                break;
            }
        }
        
        if ( path == null ){
            return null;
        }

        StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8);
        string lua = "";
        try
        {
            lua = sr.ReadToEnd();
        }
        catch (System.Exception)
        {
        }
        sr.Close();

        if (lua == "")
        {
            return null;
        }
        else
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(lua);
            return bytes;
        }
    }

}