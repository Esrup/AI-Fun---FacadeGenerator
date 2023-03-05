using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;

namespace AIFacade.Model
{
    public class httpreq
    {
        public static BitmapImage callSTDiff(string prompt)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:7860/sdapi/v1/txt2img");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            Image image = null;
            BitmapImage bmp = null;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"prompt\":\"{"+prompt+"}\"," +
                  "\"steps\":\"50\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();



            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var jsonObject = JObject.Parse(result);
                JArray name = (JArray)jsonObject["images"];

                for (int i = 0; i < name.Count; i++)
                {
                    byte[] bytes = Convert.FromBase64String(name[i].ToString());

                    
                        using (var ms = new System.IO.MemoryStream(bytes))
                        {
                            var images = new BitmapImage();
                            images.BeginInit();
                            images.CacheOption = BitmapCacheOption.OnLoad; // here
                            images.StreamSource = ms;
                            images.EndInit();
                        bmp = images;
                        }
                    

                    //using (MemoryStream ms = new MemoryStream(bytes))
                    //{
                    //    image = Image.FromStream(ms);
                    //}

                    //image.Save(@"C:\Users\esrup\Documents\GitHub\stable-diffusion-webui\outputs\txt2img-images\2023-03-04\hej.png");

                }
            }
            return bmp;

        }

    }
}
