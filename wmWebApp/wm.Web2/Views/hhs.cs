using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace System.Web
{
    public static class hhs
    {
        //http://stackoverflow.com/a/273971/3161505
        public static string GetName<T, U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
                return member.Member.Name;

            throw new ArgumentException("Expression is not a member access", "expression");
        }
    }
}