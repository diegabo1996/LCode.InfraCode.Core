using Newtonsoft.Json;
using System;
using System.Messaging;

namespace LCode.InfraestruCode.Core.Tools.MSMQ.Core
{
    public static class Cola
    {
        public static string NombreCola = string.Empty;
        public static MessageQueue MQ;
        public static void IniciarMQ()
        {
            MQ = CrearColaPrivada(NombreCola);
        }
        public static MessageQueue CrearColaPrivada(string Nombre)
        {
            string path = string.Format(@".\private$\{0}", Nombre);
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path);
                return new MessageQueue(path);
            }
            return new MessageQueue(path);
        }
        public static void EnviarObjeto(object Objeto)
        {
            System.Messaging.Message mm = new System.Messaging.Message();
            mm.Body = JsonConvert.SerializeObject(Objeto);
            mm.Label = "Msg" + DateTime.Now.ToString("yyyyMMddHHmmss");
            mm.Recoverable = true;
            MQ.Send(mm);
        }
        public static string RecibirTransaccion()
        {
            try
            {
                var mes = MQ.Receive();
                mes.Formatter = new XmlMessageFormatter(
                new String[] { "System.String,mscorlib" });
                return mes.Body.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
