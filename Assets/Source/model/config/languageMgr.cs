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

        public void ReadFrom(Zby.ByteBuffer rd)
        {
            Key = rd.ReadString();
            Content = rd.ReadString();
        } 
        public void ReadFrom(BetterList<string> temp)
        {
            int i = 0;
            Key = temp[i++];
            Content = temp[i++];
        } 
    }

    public  class  languageMgr
    {
        public const string BinFile = "language.bin";
        public const string CsvFile = "language.csv";
        
        public Dictionary<string,language> dataMap = new Dictionary<string,language>(); 

        public bool LoadDefault( string path)
        {
            return LoadFromFile( path + BinFile);
        }
   
        public bool LoadDefaultCsv( string path)
        {
            return LoadFromCsvFile( path + CsvFile);
        }
        
        public bool LoadFromFile( string fileName)
        {
            dataMap.Clear();
            try{
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    Zby.ByteBuffer readBuff = new Zby.ByteBuffer(stream);
                    do{
                        language newData = new language();
                        try{
                            newData.ReadFrom(readBuff);
                            dataMap.Add( newData.Key, newData);
                        }catch(Exception e){
                            break;
                        }
                    }while(true);
                }
            }catch(Exception e){
                return false;
            }
            return dataMap.Count > 0;
        }

        public bool LoadFromCsvFile( string fileName)
        {
            dataMap.Clear();
            try{
                ByteReader reader = ByteReader.Open(fileName);
                int line = 0;
                for (; ; )
                {
                     BetterList<string> temp = reader.ReadCSV();
                     if (temp == null || temp.size == 0) break;
                     if (line > 2 )
                     {
                        
                        language  newData = new language ();
                        newData.ReadFrom(temp);
                        dataMap.Add(newData.Key, newData);
                     }
                     line++;
                 }
             }catch(Exception e){
                return false;
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
