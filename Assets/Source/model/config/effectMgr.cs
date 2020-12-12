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

        public void ReadFromBuff(Zby.ByteBuffer rd)
        {
            ID = rd.ReadInt();
            Path = rd.ReadString();
        } 
    }

    public  class  effectMgr
    {
        public static string BinFile = "effect.bin";
        public Dictionary<int,effect> dataMap = new Dictionary<int,effect>(); 

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
                    effect newData = new effect();
                    try{
                        newData.ReadFromBuff(readBuff);
                        dataMap.Add( newData.ID, newData);
                    }catch(Exception e){
                        break;
                    }
                }while(true);
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
