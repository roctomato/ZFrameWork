using UnityEngine.UI;
using Zby;

public class Panel_component: CnUiComponent
{
    public Button _btn1;
    public Button _btn2;
    public Text _text;

    public override bool DoInit()
    {
        bool ret = false;
        do
        {
            _btn1 = GetElement<Button>("Btn1");
            if ( null == _btn1)
            {
                ZLog.E(Trans, "no Btn1");
                break;
            }

            _btn2 = GetElement<Button>("Btn2");
            if (null == _btn2)
            {
                ZLog.E(Trans, "no Btn2");
                break;
            }

            _text = GetElement<Text>("Text");
            if ( null == _text)
            {
                ZLog.E(Trans, "no text");
                break;
            }
            ret = true;
        } while (false);
        return ret;
    }
}