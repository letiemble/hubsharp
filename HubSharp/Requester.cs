using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace HubSharp.Core
{
	public class Requester
	{
		private String authorizationHeader;
		private String baseUrl;
		private int timeout;
		private String hostname;
		private int port;
		private String prefix;

		public Requester (String loginOrToken, String password, String baseUrl, int timeout)
		{
			if (!String.IsNullOrEmpty (password)) {
				String str = loginOrToken + ":" + password;
				this.authorizationHeader = "Basic " + Convert.ToBase64String (Encoding.UTF8.GetBytes (str));
			} else if (!String.IsNullOrEmpty (loginOrToken)) {
				this.authorizationHeader = "token " + loginOrToken;
			} else {
				this.authorizationHeader = null;
			}

			this.baseUrl = baseUrl;
			this.timeout = timeout;

			Uri uri = new Uri (this.baseUrl);
			this.hostname = uri.Host;
			this.port = uri.Port;
			this.prefix = uri.AbsoluteUri;
		}

		public Tuple<WebHeaderCollection, String> RequestAndCheck (String verb, String url, IDictionary<String, String> parameters, String input)
		{
			Tuple<HttpStatusCode, WebHeaderCollection, String> result = this.RequestRaw (verb, url, parameters, input);

			HttpStatusCode code = result.Item1;
			WebHeaderCollection headers = result.Item2;
			String data = result.Item3;
			if (code >= HttpStatusCode.BadRequest) {
				throw new GitHubException ("Error:" + code + " " + data);
			}

			return Tuple.Create(headers, data);
		}

		public Tuple<HttpStatusCode, WebHeaderCollection, String> RequestRaw (String verb, String url, IDictionary<String, String> parameters, String input)
		{
			Stream stream;
			String data;
			byte[] buffer;

			if (url.StartsWith ("/")) {
				url = this.baseUrl + url;
			}
			url = this.CompleteUrl (url, parameters);

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (url);
			request.Method = verb;

			if (!String.IsNullOrEmpty (this.authorizationHeader)) {
				request.Headers ["Authorization"] = this.authorizationHeader;
			}
			if (input != null) {
				request.ContentType = "application/json";
				buffer = Encoding.UTF8.GetBytes (input);

				stream = request.GetRequestStream ();
				stream.Write (buffer, 0, buffer.Length);
				stream.Close ();
			}

			HttpWebResponse response = null;
			try {
				response = (HttpWebResponse)request.GetResponse ();
			} catch (HttpException hex) {
				return Tuple.Create ((HttpStatusCode)hex.ErrorCode, (WebHeaderCollection) null, "{ exception : '" + hex.Message + "' }");
			} catch (Exception ex) {
				return Tuple.Create (HttpStatusCode.BadRequest, (WebHeaderCollection) null, "{ exception : '" + ex.Message + "' }");
			}

			HttpStatusCode code = response.StatusCode;
			using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
				data = reader.ReadToEnd ();
			}
			WebHeaderCollection headers = response.Headers;

			return Tuple.Create(code, headers, data);
		}

		private String CompleteUrl (String url, IDictionary<String, String> parameters)
		{
			String result = url;
			if (parameters != null) {
				foreach (String key in parameters.Keys) {
					String part = HttpUtility.UrlEncode (key) + "=" + HttpUtility.UrlEncode (parameters [key]);
					if (result.Contains ("?")) {
						result += "&" + part;
					} else {
						result += "?" + part;
					}
				}
			}
			return result;
		}
	}
}
