using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

using ICSharpCode.SharpZipLib.Zip;
using Zby;

public class LuaCustomLoader
{
    ZipFile _luaZip = null;
    HashSet<string> _pathList = new HashSet<string>();

    public bool  InitZipFile(string zipfile)
    {
        bool ret = false;
        do{
            if ( _luaZip != null ){
                ZLog.E(null,"cant reinit zipfile ");
                break;  
            }

            if (!File.Exists(zipfile))
            {
                ZLog.E(null,"zipfile {0} not exist", zipfile);
                break;
            }
            _luaZip = new ZipFile( zipfile);
            ret = true;
        }while(false);
        return ret;
    }

    public void AddPath( string path) {
        _pathList.Add(path);
    }

    public void AddPath(string[] paths) {
        foreach (var item in paths)
        {
            _pathList.Add(item);
        }
    }

    public byte[] LoaderFromZipFile(ref string filePath)
    {
        byte[] buffer = null;
        filePath = filePath.Replace(".", "/");
        filePath += ".lua";
 
        var entry = _luaZip.GetEntry(filePath);
        if (entry != null)
        {
            using (var stream = _luaZip.GetInputStream(entry))
            {
                buffer = new byte[entry.Size];
                stream.Read(buffer, 0, buffer.Length);
            }
        }
        return buffer;
    }
    public byte[] LoaderFromLoacalFile(ref string filePath)
    {
 
        //文件所在的绝对路径
        filePath = filePath.Replace(".", "/");
        string path = null;
        
        foreach(  string fold in  _pathList ){
            string cur_path = fold+ filePath + ".lua";
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