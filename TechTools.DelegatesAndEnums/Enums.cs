using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechTools.DelegatesAndEnums
{
    public enum eTipoTercero
    {
        TRANDINA,
        PROMOTICK,
        NotDefined
    }
    public enum eTipoDocFuente
    {
        factura = 0,
        facturaServicios = 2,
        NotasCredito = 3
    }
    public enum eTipoNotifiacion
    {
        email,
        sms
    }
    public enum eTypeAlert
    {
        Information,
        Exclamation,
        Error
    }
    public enum eYesNo
    {
        Yes,
        No,
        None
    }
    public enum eTypeLog
    {
        None,
        Catalogo,
        Error,
        Info,
        Advertencia,
        Guardado
    }
    public enum eTipoDocumento
    {
        Cédula,
        Pasaporte,
        SinIdentificación
    }
    public enum eDialogResult {
        Ok,
        Abort
    }
    public enum eCrud
    {
        Add,
        Edit,
        Delete
    }
    public enum eEstadoVideo {
        Copiando,
        CopiadoLocalmente,
        SincronizadoConElServidor
    }
    public enum eTipoPersona
    {
        Natural,
        Jurídica
    }
}
