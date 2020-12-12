using System;
using System.IO;
using System.Collections.Generic;
using Zby;

namespace duoli
{ 
    public  class  effect
    {
        public int	ID; //索引
        public string	Path; //特效文件路径

        public void ReadFrom(Zby.ByteBuffer rd)
        {
            ID = rd.ReadInt();
            Path = rd.ReadString();
        } 
        public void ReadFrom(BetterList<string> temp)
        {
            int i = 0;
            int.TryParse(temp[i++], out ID);
            Path = temp[i++];
        } 
    }

    public  class  effectMgr
    {
        public const string BinFile = "effect.bin";
        public const string CsvFile = "effect.csv";
        
        public Dictionary<int,effect> dataMap = new Dictionary<int,effect>(); 

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
                        effect newData = new effect();
                        try{
                            newData.ReadFrom(readBuff);
                            dataMap.Add( newData.ID, newData);
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
                        
                        effect  newData = new effect ();
                        newData.ReadFrom(temp);
                        dataMap.Add(newData.ID, newData);
                     }
                     line++;
                 }
             }catch(Exception e){
                return false;
            }
            return dataMap.Count > 0;
        }

        public effect FindData(int key ){
            effect ret = null;
            if ( dataMap.ContainsKey(key)){
                ret = dataMap[key];
            }
            return ret;
        }
    }
}
