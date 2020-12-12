using System;
using System.IO;
using System.Collections.Generic;
using Zby;

namespace duoli
{ 
    public  class  bushou
    {
        public int	index; //索引
        public string	xing; //名字
        public int	bishu; //笔画数
        public int	duan; //段数
        public string	bishun; //笔顺

        public void ReadFrom(Zby.ByteBuffer rd)
        {
            index = rd.ReadInt();
            xing = rd.ReadString();
            bishu = rd.ReadInt();
            duan = rd.ReadInt();
            bishun = rd.ReadString();
        } 
        public void ReadFrom(BetterList<string> temp)
        {
            int i = 0;
            int.TryParse(temp[i++], out index);
            xing = temp[i++];
            int.TryParse(temp[i++], out bishu);
            int.TryParse(temp[i++], out duan);
            bishun = temp[i++];
        } 
    }

    public  class  bushouMgr
    {
        public const string BinFile = "bushou.bin";
        public const string CsvFile = "bushou.csv";
        
        public Dictionary<int,bushou> dataMap = new Dictionary<int,bushou>(); 

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
                        bushou newData = new bushou();
                        try{
                            newData.ReadFrom(readBuff);
                            dataMap.Add( newData.index, newData);
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
                        
                        bushou  newData = new bushou ();
                        newData.ReadFrom(temp);
                        dataMap.Add(newData.index, newData);
                     }
                     line++;
                 }
             }catch(Exception e){
                return false;
            }
            return dataMap.Count > 0;
        }

        public bushou FindData(int key ){
            bushou ret = null;
            if ( dataMap.ContainsKey(key)){
                ret = dataMap[key];
            }
            return ret;
        }
    }
}
