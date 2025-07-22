using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AuthorizePorRolAttribute : AuthorizeAttribute
{
    private readonly string[] rolesPermitidos;

    public AuthorizePorRolAttribute(params string[] roles)
    {
        this.rolesPermitidos = roles;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var rolUsuario = httpContext.Session["UsuarioRol"]?.ToString();

        if (string.IsNullOrEmpty(rolUsuario))
            return false;

        return rolesPermitidos.Contains(rolUsuario);
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectResult("/Home/NoAutorizado");
    }
}