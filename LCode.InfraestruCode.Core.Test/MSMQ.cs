using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LCode.InfraestruCode.Core.Tools.MSMQ;

namespace LCode.InfraestruCode.Core.Test
{
    [TestClass]
    public class MSMQ
    {
        [TestMethod]
        public void TestMethod1()
        {
            Cola.NombreCola = "Test";
            Cola.IniciarMQ();
            Cola.EnviarObjeto("Prueba"+DateTime.Now.ToString());
            bool x = false;
            while (!x)
            { 
                var Test = Cola.RecibirTransaccion();
            }
        }
    }
}
