using System;
using System.IO;
using System.Collections.Generic;
using Zby;

namespace duoli
{ 
    public  class  language
    {
        public string	Key; //键值
        public string	Content; //中文

        public void ReadFromBuff(Zby.ByteBuffer rd)
        {
            Key = rd.ReadString();
            Content = rd.ReadString();
        } 
    }

    public  class  languageMgr
    {
        public static string BinFile = "language.bin";
        public Dictionary<string,language> dataMap = new Dictionary<string,language>(); 

        public bool LoadDefault( string path)
        {
            return LoadFromFile( path + BinFile);
        }
        
        public bool LoadFromFile( string fileName)
        {
            dataMap.Clear();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                Zby.ByteBuffer readBuff = new Zby.ByteBuffer(stream);
                do{
                    language newData = new language();
                    try{
                        newData.ReadFromBuff(readBuff);
                        dataMap.Add( newData.Key, newData);
                    }catch(Exception e){
                        break;
                    }
                }while(true);
            }
            return dataMap.Count > 0;
        }

        public language FindData(string key ){
            language ret = null;
            if ( dataMap.ContainsKey(key)){
                ret = dataMap[key];
            }
            return ret;
        }
    }
}
