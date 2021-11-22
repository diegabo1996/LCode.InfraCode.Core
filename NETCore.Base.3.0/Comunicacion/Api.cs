using System;
using System.IO;
using System.Net;
using BCP.NETCore.Base;
using Newtonsoft.Json;

public class Api
{
    RegistroLogs Logs = new RegistroLogs();
    public string Post(string url, string data)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string Resp = sr.ReadToEnd();
#if DEBUG
                Logs.RegistrarEvento(TipoEvento.Informativo, Resp);
#endif
                return Resp;
            }
        }
        catch (Exception ex)
        {
            Logs.RegistrarEvento(TipoEvento.Error, ex);
            return null;
        }
    }
    public string Post(string url, object data)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(JsonConvert.SerializeObject(data));
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string Resp = sr.ReadToEnd();
#if DEBUG
                Logs.RegistrarEvento(TipoEvento.Informativo, Resp);
#endif
                return Resp;
            }
        }
        catch (Exception ex)
        {
            Logs.RegistrarEvento(TipoEvento.Error, ex);
            return null;
        }
    }

    public string PostAPIAuth(string url, string AuthToken, string data)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, AuthToken);
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                string Resp = sr.ReadToEnd();
                return Resp;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public string GetRaw(string Url)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var request = (HttpWebRequest)WebRequest.Create(Url);
            request.ContentType = "application/json";
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                //string Resultado = "{\"data\": [ { \"access_token\":\"EAACO2oIwEJYBAGtm4gw6ZCHjByRAbdqvnIG5gdqmKUWVyhm80oDEAmtZAazbgn8U7qmtn3ZAdcC5PZB9hp716RooZAfxGQbEkFPH32OlAYnaZAlZAG1e6tRkKyUdZCQ5rw5uFa4gqxaQwM29j9bKBVwvB3LsMp2pVwlE09d75yYzX7xLONIwX2Qe\", \"category\": \"Pub\", \"category_list\": [ { \"id\": \"218693881483234\", \"name\": \"Pub\" } ], \"name\": \"Valentino's\", \"id\": \"389133528186094\", \"perms\": [ \"ADMINISTER\", \"EDIT_PROFILE\", \"CREATE_CONTENT\", \"MODERATE_CONTENT\", \"CREATE_ADS\", \"BASIC_ADMIN\" ] }, { \"access_token\": \"EAACO2oIwEJYBAH8iV23wHqpVJL4SxPaLxwbQUQulo7nNWL4XVy6tA8aUjuxUZBQjXsovZAJGQCLDwCm81eI3SbYG2qbiC9OXZCbTrp17eMFm2PqBbMln1eGzapgoehNQpEi3qwKZCHuIa6ZCxmAPKbLXSrymZAl4V2ZA9wseHcLxgZDZD\", \"category\": \"Entrepreneur\", \"category_list\": [ { \"id\": \"1617\", \"name\": \"Entrepreneur\" } ], \"name\": \"Boty BCP\", \"id\": \"116645115662344\", \"perms\": [ \"ADMINISTER\", \"EDIT_PROFILE\", \"CREATE_CONTENT\", \"MODERATE_CONTENT\", \"CREATE_ADS\", \"BASIC_ADMIN\" ] } ] }";
                string Resultado = sr.ReadToEnd();
                return Resultado;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public string PostUrlEncode(string url, string data)
    {
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (se, cert, chain, sslerror) =>
            {
                return true;
            };
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(data);
            }
            var response = (HttpWebResponse)request.GetResponse();
            if (response == null)
                throw new InvalidOperationException("GetResponse returns null");
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
