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
		public const String Get = "GET";
		public const String Post = "POST";
		public const String Delete = "DELETE";
		public const String Patch = "PATCH";
		private String authorizationHeader;
		private String baseUrl;
		private int timeout;

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
		}

		public int CallLimit { get; set; }

		public int CallRemaining { get; set; }
		
		public Tuple<HttpStatusCode, WebHeaderCollection, String> Request (String verb, String url, IDictionary<String, String> parameters, String input)
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
				Console.WriteLine("Request [{0}] {1}", request.Method, request.RequestUri);
#if DEBUG
#endif
				response = (HttpWebResponse)request.GetResponse ();
			} catch (HttpException hex) {
				return Tuple.Create ((HttpStatusCode)hex.ErrorCode, (WebHeaderCollection)null, "{ exception : '" + hex.Message + "' }");
			} catch (Exception ex) {
				return Tuple.Create (HttpStatusCode.BadRequest, (WebHeaderCollection)null, "{ exception : '" + ex.Message + "' }");
			}

			HttpStatusCode code = response.StatusCode;
			using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
				data = reader.ReadToEnd ();
			}
			WebHeaderCollection headers = response.Headers;

			this.UpdateLimits(headers);
#if DEBUG
			Console.WriteLine("Calls {0}/{1}", (this.CallLimit - this.CallRemaining), this.CallLimit);
#endif
			return Tuple.Create (code, headers, data);
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

		private void UpdateLimits (WebHeaderCollection headers)
		{
			String limiteValue = headers["X-RateLimit-Limit"];
			if (!String.IsNullOrEmpty(limiteValue)) {
				this.CallLimit = Int32.Parse(limiteValue);
			}
			String remainingValue = headers["X-RateLimit-Remaining"];
			if (!String.IsNullOrEmpty(remainingValue)) {
				this.CallRemaining = Int32.Parse(remainingValue);
			}
		}
	}
}
