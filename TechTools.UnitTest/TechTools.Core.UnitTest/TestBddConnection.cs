using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TechTools.Core.Hana;
using System.Data;

namespace TechTools.Core.Hana.UnitTest
{
    [TestClass]
    public class TestBddConnection
    {
        [TestMethod]
        public void GetDataTableByQuery()
        {
            var sql = @"
            select
	            t0.""Id"",
                to_char(t0.""Fecha"", 'dd/mm/yyyy') ""fechaFactura"",
	            t0.""NumFolio"",
	            t3.""COD_RESPUESTA_WS"",
	            round(sum(t1.""SubtotalMenosDescuentos""), 0) ""montoFactura""-- no se toman en cuenta los descuentos
            from
                ""JbpVw_Factura"" t0 inner join
                ""JbpVw_FacturaLinea"" t1 on  t1.""IdFactura"" = t0.""Id"" inner join
                ""JbpVw_SocioNegocio"" t2 on t2.""CodSocioNegocio"" = t0.""CodCliente"" left outer join
                ""JBP_FACTURAS_ENVIADAS_PROMOTICK"" t3 on t3.""ID_FACTURA"" = t0.""Id""
            where
                t0.""Fecha"" > to_date('2020-02-29', 'yyyy-mm-dd')
                and t2.""Ruc"" in ('0912274123001')
                and(t3.""COD_RESPUESTA_WS"" is null or t3.""COD_RESPUESTA_WS"" <> 1) --solo las facturas que no se han enviado ó que hubo un problema en el envío
            group by
                t0.""Id"",
	            t0.""Fecha"",
	            t0.""NumFolio"",
	            t3.""COD_RESPUESTA_WS""
            ";
            var dt = new BaseCore().GetDataTableByQuery(sql);
            foreach (DataRow dr in dt.Rows) {
                var monto = dr["montoFactura"].ToString();
            }
            //var dt = new BaseCore().GetDataTableByQuery("select top 3 \"Nombre\" from \"JbpVw_SocioNegocio\";");
            Assert.AreEqual(true,dt.Rows.Count>0);
        }
    }
}
