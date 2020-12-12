using System;
using System.IO;
using System.Collections.Generic;
using Zby;

namespace duoli
{ 
    public  class  jiegou
    {
        public int	index; //索引
        public int	add; //追加结构伤害段数
        public string	xing; //名字
        public string	parent_name; //大类名称

        public void ReadFrom(Zby.ByteBuffer rd)
        {
            index = rd.ReadInt();
            add = rd.ReadInt();
            xing = rd.ReadString();
            parent_name = rd.ReadString();
        } 
        public void ReadFrom(BetterList<string> temp)
        {
            int i = 0;
            int.TryParse(temp[i++], out index);
            int.TryParse(temp[i++], out add);
            xing = temp[i++];
            parent_name = temp[i++];
        } 
    }

    public  class  jiegouMgr
    {
        public const string BinFile = "jiegou.bin";
        public const string CsvFile = "jiegou.csv";
        
        public Dictionary<int,jiegou> dataMap = new Dictionary<int,jiegou>(); 

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
                        jiegou newData = new jiegou();
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
                        
                        jiegou  newData = new jiegou ();
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

        public jiegou FindData(int key ){
            jiegou ret = null;
            if ( dataMap.ContainsKey(key)){
                ret = dataMap[key];
            }
            return ret;
        }
    }
}
