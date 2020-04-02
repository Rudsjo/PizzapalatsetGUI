using System;
using System.Collections.Generic;
using System.Text;

namespace CookingTerminal
{
    /* 
     * Container of simple attached properties used thorugh the application 
     */

    /// <summary>
    /// Attached property used to flag if a control is busy
    /// </summary>
    public class IsBusyProperty : BaseAttachedProperty<IsBusyProperty, bool> { }

}
