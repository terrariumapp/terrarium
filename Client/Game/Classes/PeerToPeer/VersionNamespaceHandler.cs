//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;
using System.Net;
using System.Reflection;
using System.Text;
using Terrarium.Game;
using Terrarium.Net;

namespace Terrarium.PeerToPeer
{
    /// <summary>
    ///  The version namespace handler implements the version namespace for the
    ///  GET method.  During the GET method an XML document is
    ///  returned with the version information.  Use of any other Method is an
    ///  error that is returned and the state of the response is given an error status
    ///  code.
    /// </summary>
    internal class VersionNamespaceHandler : IHttpNamespaceHandler
    {
        internal static Version peerVersion = Assembly.GetExecutingAssembly().GetName().Version;

        #region IHttpNamespaceHandler Members

        public void ProcessRequest(HttpApplication webapp)
        {
            webapp.HttpResponse.Server = "Microsoft .Net Terrarium";
            webapp.HttpResponse.Date = DateTime.Now;
            webapp.HttpResponse.ContentType = "text/xml";
            webapp.HttpResponse.StatusCode = HttpStatusCode.OK;
            webapp.HttpResponse.StatusDescription = "OK";
            webapp.HttpResponse.KeepAlive = false;
            string body;

            if (webapp.HttpRequest.Method == "GET")
            {
                body = "<version><build>" + peerVersion.Build + "</build>";
                body += "<major>" + peerVersion.Major + "</major>\t";
                body += "<minor>" + peerVersion.Minor + "</minor>\t</version>";
                body += "<channel>" + GameEngine.Current.PeerChannel + "</channel>";
            }
            else
            {
                webapp.HttpResponse.StatusCode = HttpStatusCode.MethodNotAllowed;
                webapp.HttpResponse.StatusDescription = "Method Not Allowed";
                body = "<HTML><BODY>" + "The method " + webapp.HttpRequest.Method;
                body += " is not allowed.</BODY></HTML>";
                webapp.HttpResponse.ContentType = "text/html";
            }

            // Encode the body message and place it on the stream.
            var bodyBytes = Encoding.ASCII.GetBytes(body);
            webapp.HttpResponse.ContentLength = bodyBytes.Length;
            webapp.HttpResponse.Close(bodyBytes);
        }

        #endregion
    }
}