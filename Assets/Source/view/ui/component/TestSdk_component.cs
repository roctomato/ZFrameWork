using UnityEngine.UI;
using Zby;

public class TestSdk_component: CnUiComponent
{
    public Button btnCall;
    public InputField iptMethod;
    public InputField iptParam;
    public Text txtResult;

    public override bool DoInit()
    {
        bool ret = false;
        do
        {
            btnCall = GetElement<Button>("btnCall");
            if ( null == btnCall)
            {
                ZLog.E(Trans, "no btnCall");
                break;
            }

            iptMethod = GetElement<InputField>("iptMethod");
            if (null == iptMethod)
            {
                ZLog.E(Trans, "no iptMethod");
                break;
            }

            iptParam = GetElement<InputField>("iptParam");
            if (null == iptParam)
            {
                ZLog.E(Trans, "no iptParam");
                break;
            }

            txtResult = GetElement<Text>("txtResult");
            if (null == txtResult)
            {
                ZLog.E(Trans, "no txtResult");
                break;
            }

            ret = true;
        } while (false);
        return ret;
    }
}