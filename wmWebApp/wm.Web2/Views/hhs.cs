using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace System.Web
{
    public static class hhs
    {
        private static ResourceManager InitResources(ResourceManager resourceManager, string filename = "BasicResources")
        {
            if (resourceManager == null)
            {
                Assembly assembly = System.Reflection.Assembly.Load("App_GlobalResources");
                resourceManager = new ResourceManager("Resources." + filename, assembly);
            }

            return resourceManager;
        }

        public static ResourceManager resourceManager = null;
        //http://stackoverflow.com/a/273971/3161505
        public static string GetName<T, U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                return member.Member.Name;
            }

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        public static string GetTranslatedName<T, U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                return hhs.GetTranslatedName(member.Member.Name);
            }

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        public static string GetTranslatedName(string input)
        {
            resourceManager = hhs.InitResources(resourceManager);
            string result = resourceManager.GetString(input);
            return (result == null || result == String.Empty) ? input : result;
        }
    }
}