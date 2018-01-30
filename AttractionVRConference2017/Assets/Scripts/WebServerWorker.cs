/*
 * A simple web server that can send/recieve messages through HTTP.
 * 
 * Parameters: prefix - location to serve the web server (e.g. http://localhost:8080/)
 *             method - callback function for  displaying web page
 *             action_method - callback function for processing POST data
 * 
 * Code was modified from https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server
 */

using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SimpleWebServer
{
	public class WebServerWorker
	{
		private readonly HttpListener _listener = new HttpListener();
		private readonly Func<HttpListenerRequest, string> _responderMethod;
		private readonly Action<string> _recievingMethod;

		public WebServerWorker(string[] prefixes, Func<HttpListenerRequest, string> method, Action<string> action_method)
		{
			if (!HttpListener.IsSupported)
				throw new NotSupportedException(
					"Needs Windows XP SP2, Server 2003 or later.");

			// URI prefixes are required, for example 
			// "http://localhost:8080/index/".
			if (prefixes == null || prefixes.Length == 0)
				throw new ArgumentException("prefixes");

			// A responder method is required
			if (method == null && action_method == null)
				throw new ArgumentException("method");

			foreach (string s in prefixes)
				_listener.Prefixes.Add(s);

			_responderMethod = method;
			_recievingMethod = action_method;
			_listener.Start();
		}

		public WebServerWorker(Func<HttpListenerRequest, string> method, Action<string> action_method, params string[] prefixes)
			: this(prefixes, method, action_method) { }
		
		public WebServerWorker(Func<HttpListenerRequest, string> method, params string[] prefixes)
			: this(prefixes, method, null) { }
		
		public WebServerWorker(Action<string> action_method, params string[] prefixes)
			: this(prefixes, null, action_method) { }

		public void Run()
		{
			ThreadPool.QueueUserWorkItem((o) =>
				{
					//Console.WriteLine("Webserver running...");
					Debug.Log("Webserver running...");
					try
					{
						while (_listener.IsListening)
						{
							ThreadPool.QueueUserWorkItem((c) =>
								{
									var ctx = c as HttpListenerContext;

									// Process Request
									if(_recievingMethod != null)
									{
										var request = ctx.Request;
										System.IO.StreamReader reader = new System.IO.StreamReader (
											request.InputStream, request.ContentEncoding);
										string requestText = reader.ReadToEnd();
										_recievingMethod(requestText);
										//Debug.Log("DEBGUG requestText: "+requestText);
									}

									// Send Response
									try
									{
										string rstr = "";
										if(_responderMethod != null)
											rstr = _responderMethod(ctx.Request);
										byte[] buf = Encoding.UTF8.GetBytes(rstr);
										ctx.Response.ContentLength64 = buf.Length;
										ctx.Response.OutputStream.Write(buf, 0, buf.Length);
									}
									catch { } // suppress any exceptions
									finally
									{
										// always close the stream
										ctx.Response.OutputStream.Close();
									}
								}, _listener.GetContext());
						}
					}
					catch { } // suppress any exceptions
				});
		}

		public void Stop()
		{
			_listener.Stop();
			_listener.Close();
		}
	}
}