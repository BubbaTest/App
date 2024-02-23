using System;
using System.Web;
using System.Web.Security;

namespace Gaia.Seguridad.Controllers
{
    public static class SessionHelper
    {

        public static void AddItem<T>(this HttpSessionStateBase session, T item) where T : class
        {
            session[GetKey(item.GetType())] = item;
        }

        public static T GetItem<T>(this HttpSessionStateBase session) where T : class
        {
            if (session == null)
                CerrarSesion();
            
            return session[GetKey(typeof(T))] as T;
        }

        public static string GetKey(Type itemType)
        {
            return itemType.FullName;
        }

        public static void Autenticar(string userName)
        {
            FormsAuthentication.SetAuthCookie(userName, false);
        }

        public static void CerrarSesion()
        {
            int contador = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < contador; i++)
            {
                var cookie = new HttpCookie(HttpContext.Current.Request.Cookies[i].Name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = string.Empty;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }

            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            FormsAuthentication.SignOut();
        }
    }
}