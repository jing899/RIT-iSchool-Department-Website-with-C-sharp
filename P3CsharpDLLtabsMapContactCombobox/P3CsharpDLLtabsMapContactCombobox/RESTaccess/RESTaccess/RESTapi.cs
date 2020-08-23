using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RESTaccess
{
    public class RESTapi
    {
        string baseUri = null;

        public RESTapi( string _base)
        {
            baseUri = _base;
        }

        #region Common method getRESTData( url ) used throughout this code
        public string getRESTData(string url)
        {
            // const string baseUri = "http://ist.rit.edu/api";

            // connect to the api
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri + url);

            try
            {
                // waits and gets the response for this web request
                WebResponse response = request.GetResponse();

                // using the response stream from the web request, read it
                using (Stream responseStream = response.GetResponseStream())
                {
                    // read the response from the API
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }

            }
            catch (WebException we)
            {
                WebResponse err = we.Response;
                using (Stream responseStream = err.GetResponseStream())
                {
                    StreamReader r = new StreamReader(responseStream, Encoding.UTF8);
                    string errorText = r.ReadToEnd();
                    // do something like log this error to an error log file
                    // or cheap out with this error message
                    Console.WriteLine("ERROR: " + errorText);
                }
                // Can't do anything more, throw this exception... the easy way
                throw;
            }

        } // getRESTData

        #endregion


    }
}
