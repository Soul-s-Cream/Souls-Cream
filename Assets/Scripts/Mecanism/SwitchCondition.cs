using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCondition : Switch
{
    #region Mecanism Logic
    protected override void SwitchingOn()
    {
        AddInput(true);
    }

    protected override void SwitchingOff()
    {
        AddInput(true);
    }
    #endregion
}
