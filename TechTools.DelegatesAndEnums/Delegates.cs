using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechTools.DelegatesAndEnums
{
    public delegate void dLogNotification(eTypeLog tipo, string msg);
    public delegate void dVoid();
    public delegate bool dBool();
    public delegate void dCloseWaitMessage();
    public delegate void dAlert(string msg, eTypeAlert typeAlert);
    public delegate void dStringParameter(string msg);
    public delegate void dDecimal(decimal me);
    public delegate void dDateTime(DateTime me);
    public delegate void dAddLog(string source, string msg, eTypeLog typeLog);
    public delegate void dAddMessageAndSource(string source, string msg);
    public delegate eYesNo dConfirmMessageDialogEvent(string msg);
    public delegate void dObjectParameter(object me);
    public delegate void dIntParameter(int me);
    public delegate void dBoolParameter(bool me);
    public delegate eDialogResult dShowDialogWithParameter(object obj);
    public delegate eDialogResult dShowDialog();
    public delegate void dShowMessageInBackground(string msg, bool showWait = false);
    public delegate decimal dReturnDecimal();
}
