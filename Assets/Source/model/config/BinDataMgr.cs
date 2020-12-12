using UnityEngine;

namespace duoli
{
    public class BinDataMgr{
        public languageMgr _languageMgr = new languageMgr();
        public effectMgr _effectMgr = new effectMgr();
        public bushouMgr _bushouMgr = new bushouMgr();
        public jiegouMgr _jiegouMgr = new jiegouMgr();

        public bool LoadData( string path)
        {
            bool ret = false;
            do{

                if ( ! _languageMgr.LoadDefault ( path ) ){
                    Debug.LogError("languageMgr bin failed");
                    break;
                }

                if ( ! _effectMgr.LoadDefault ( path ) ){
                    Debug.LogError("effectMgr bin failed");
                    break;
                }

                if ( ! _bushouMgr.LoadDefault ( path ) ){
                    Debug.LogError("bushouMgr bin failed");
                    break;
                }

                if ( ! _jiegouMgr.LoadDefault ( path ) ){
                    Debug.LogError("jiegouMgr bin failed");
                    break;
                }                
                ret = true;
            }while(false);

            return ret;
        }
        
        public bool LoadCsvData( string path)
        {
            bool ret = false;
            do{

                if ( ! _languageMgr.LoadDefaultCsv ( path ) ){
                    Debug.LogError("languageMgr csv failed");
                    break;
                }

                if ( ! _effectMgr.LoadDefaultCsv ( path ) ){
                    Debug.LogError("effectMgr csv failed");
                    break;
                }

                if ( ! _bushouMgr.LoadDefaultCsv ( path ) ){
                    Debug.LogError("bushouMgr csv failed");
                    break;
                }

                if ( ! _jiegouMgr.LoadDefaultCsv ( path ) ){
                    Debug.LogError("jiegouMgr csv failed");
                    break;
                }                
                ret = true;
            }while(false);

            return ret;
        }
    } 
}
