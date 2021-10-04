using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace INLivefeedConsumer
{
    public class clsFunction
    {
        public string CallAPI(string strBody,string strURL)
        {
          try
            {
                
                DateTime CurrentDT;
                System.TimeSpan SMSPushLength;

                HttpWebRequest HttpWReq;
                HttpWebResponse HttpWResp;
                Stream streamResponse;
                StreamReader streamRead;
                String HTTPWebRespStr = "";
                String httpWebR = "";

                CurrentDT = DateTime.Now;

                HttpWReq = (HttpWebRequest)(WebRequest.Create(strURL));
                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                proxy.Credentials = CredentialCache.DefaultCredentials;
                HttpWReq.Proxy = proxy;
                var strProtocol = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Protocol"];
                if (strProtocol == "Get")
                {
                    HttpWReq.Method = WebRequestMethods.Http.Get;
                }
                else {
                    HttpWReq.Method = WebRequestMethods.Http.Post;
                }

               
                HttpWReq.Timeout = 120 * 1000;

                //// Set the content type of the data being posted.
                HttpWReq.ContentType = "application / xml";

                StreamWriter sw;
                sw = new StreamWriter(HttpWReq.GetRequestStream());
                sw.Write(strBody);
                sw.Flush();
                sw.Close();
                HttpWResp = (HttpWebResponse)(HttpWReq.GetResponse());
                if (HttpWReq.HaveResponse)
                {
                    streamResponse = HttpWResp.GetResponseStream();
                    streamRead = new StreamReader(streamResponse);
                    HTTPWebRespStr = System.Convert.ToString(streamRead.ReadToEnd());
                    streamResponse.Close();
                    streamRead.Close();
                    HttpWResp.Close();
                }
                SMSPushLength = DateTime.Now.Subtract(CurrentDT);
                String HTTPWebRespStrNoNewLineIn = HTTPWebRespStr;

                HTTPWebRespStrNoNewLineIn = HTTPWebRespStrNoNewLineIn.Replace("<br>", "");
                if (HTTPWebRespStrNoNewLineIn.Length > 87)
                {
                    HTTPWebRespStrNoNewLineIn = HTTPWebRespStrNoNewLineIn.Substring(0, 87);
                }
                httpWebR = HTTPWebRespStrNoNewLineIn;
                if (httpWebR.Length > 49)
                {
                    httpWebR = HTTPWebRespStrNoNewLineIn.Substring(0, 49);
                }

                return "Success";

            }
            catch (Exception e)
            {
                return e.HResult.ToString();
            }
        }

        public string CallSQlSP(string strBody, string strConnection)
        {
            try
            {
                var strSP = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SP"];

                SqlConnection cn = new SqlConnection(strConnection);
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = cn;
                cn.Open();
             
                cmd = new SqlCommand(strSP + " '" + strBody +"'", cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = int.MaxValue;

                cmd.ExecuteReader();
            

                cn.Dispose();
                SqlConnection.ClearPool(cn);

                cn.Close();
                cn = null/* TODO Change to default(_) if this is not a reference type */;
                return "Success";

            }
            catch (Exception e)
            {
                return e.HResult.ToString();
            }
        }

    }
}
