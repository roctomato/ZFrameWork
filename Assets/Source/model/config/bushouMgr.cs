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

        public void ReadFromBuff(Zby.ByteBuffer rd)
        {
            index = rd.ReadInt();
            xing = rd.ReadString();
            bishu = rd.ReadInt();
            duan = rd.ReadInt();
            bishun = rd.ReadString();
        } 
    }

    public  class  bushouMgr
    {
        public static string BinFile = "bushou.bin";
        public Dictionary<int,bushou> dataMap = new Dictionary<int,bushou>(); 

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
                    bushou newData = new bushou();
                    try{
                        newData.ReadFromBuff(readBuff);
                        dataMap.Add( newData.index, newData);
                    }catch(Exception e){
                        break;
                    }
                }while(true);
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
