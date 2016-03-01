using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CsvHelper;
using wm.Model;

namespace System.Web
{
    public static class hhs
    {
        //private static ResourceManager InitResources(ResourceManager resourceManager, string filename = "BasicResources")
        //{
        //    if (resourceManager == null)
        //    {
        //        Assembly assembly = System.Reflection.Assembly.Load("App_GlobalResources");
        //        resourceManager = new ResourceManager("Resources." + filename, assembly);
        //    }

        //    return resourceManager;
        //}

        //public static ResourceManager resourceManager = null;
        public static List<LocalizationString> resourceManager = null;

        private static void InitResources(string fileName = "LocalizationStrings", string culture = CultureInfoCode.VN)
        {
            if (resourceManager == null)
            {
                resourceManager = new List<LocalizationString>();

                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = "wm.Web2.Migrations.SeedData." + fileName + ".csv";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;

                        while (csvReader.Read())
                        {
                            if (csvReader.GetField<string>("CultureInfoString") == culture)
                            {
                                LocalizationString localString = new LocalizationString
                                {
                                    CultureInfoString = culture,
                                    Code = csvReader.GetField<string>("Code"),
                                    Value = csvReader.GetField<string>("Value"),
                                };

                                resourceManager.Add(localString);
                            }
                        }

                        reader.Close();
                    }
                    stream.Close();
                }

            }
        }

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
            hhs.InitResources();
            LocalizationString result = resourceManager.First(s => s.Code == input);
            return (result == null) ? input : result.Value;
        }
    }
}