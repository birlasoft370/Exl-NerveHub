using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MicUI.WorkManagement.Helper
{
    public class FormulaGen
    {
        public void test(string CodeToCompile, Boolean objflag, out string strError)
        {
            strError = "Test";
        }

        public delegate bool RecordCallback(string line);

        public static void ReadRepository(string filename, RecordCallback callback)
        {
            using (StreamReader reader = new StreamReader(filename))
            {


                while (reader.Peek() >= 0)
                {
                    string inputLine = reader.ReadLine();
                    if (!callback(inputLine))
                        break;
                }
            }
        }

        static Assembly CreateCompiledAssembly(string Code, out string strError)
        {


            // Create an instance of the provider
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            // initialize compiler parameters
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            cp.IncludeDebugInformation = true;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic);

            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        cp.ReferencedAssemblies.Add(location);
                    }
                }
                catch (Exception ex)
                {
                    // throw ex;
                    // this happens for dynamic assemblies, so just ignore it. 
                    if (ex is NotSupportedException)
                        continue;

                }
            }

            cp.GenerateInMemory = true;

            // Compile the code and return the result
            CompilerResults crslt = provider.CompileAssemblyFromSource(cp, Code);
            //CompilerError[] crError =(CompilerError) crslt.Errors;
            strError = string.Empty;
            if (crslt.Errors.HasErrors)
            {
                int errorCount = 1;

                foreach (CompilerError er in crslt.Errors)
                {
                    strError += " " + errorCount.ToString() + "-  " + er.ErrorText + "   ";
                    errorCount++;
                }
                return null;
            }
            else
            {
                return crslt.CompiledAssembly;
            }
        }

        public void parse(string CodeToCompile, FrameworkElement stk, Boolean objflag, out string strError)
        {
            string CodeToCompile1 = "using System;" +
             "using System.Windows;" +
             "using System.Windows.Controls;" +
             "using System.Windows.Data;" +
             "using System.Windows.Documents;" +
             "using System.Windows.Input;" +
             "using System.Windows.Media;" +
             "using System.Windows.Media.Imaging;" +
             "using System.Windows.Navigation;" +
             "using System.Windows.Shapes;" +
             "using System.Collections; " +
             "using System.Collections.Generic; " +
                 "using System.Linq; " +
                 "using BPA.DesktopInfrastructure.BaseClasses; " +
             "namespace Filters" +
              "{" +
              " public class NameFilter" +
              " { public NameFilter(){}  delegate object del();" +
              "  public object DynamicCode(params object[] Parameters){" +
              "   FrameworkElement pg1= (FrameworkElement) Parameters[0];" +
              CodeToCompile.Trim() + ";" +

              "    return (object)\"test\";" +
              "  }" +
              " }" +
              "}";
            Assembly CompiledAssembly = CreateCompiledAssembly(CodeToCompile1, out strError);
            Type typ = CompiledAssembly.GetType("Filters.NameFilter");
            object loObject = Activator.CreateInstance(typ);

            object[] loCodeParms = new object[1];
            loCodeParms[0] = stk;
            //  loCodeParms[0] = this.form1.Page;
            if (!objflag)
            {
                object loResult = typ.InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);
            }




        }

        public void parse(string CodeToCompile, FrameworkElement stk, Boolean objflag, string str, out string strError)
        {
            string CodeToCompile1 = "using System;" +
             "using System.Windows;" +
             "using System.Windows.Controls;" +
             "using System.Windows.Data;" +
             "using System.Windows.Documents;" +
             "using System.Windows.Input;" +
             "using System.Windows.Media;" +
             "using System.Windows.Media.Imaging;" +
             "using System.Windows.Navigation;" +
             "using System.Windows.Shapes;" +
             "using System.Collections; " +
             "using System.Collections.Generic; " +
                 "using System.Linq; " +
                 "using BPA.DesktopInfrastructure.BaseClasses; " +
             "namespace Filters" +
              "{" +
              " public class NameFilter" +
              " { public NameFilter(){}  delegate object del();" +
              "  public object DynamicCode(params object[] Parameters){" +
              "   FrameworkElement pg1= (FrameworkElement) Parameters[0];" +
              CodeToCompile.Trim() + ";" +

              "    return (object)\"test\";" +
              "  }" +
              " }" +
              "}";

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic);
            strError = "";
            Boolean b = false;
            Assembly foundassembly = null;
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        string s = assembly.ExportedTypes.ToArray()[0].Name.ToString();
                        //cp.ReferencedAssemblies.Add(location);
                        if (assembly.ExportedTypes.ToArray()[0].Name == str)
                        {
                            b = true;
                            foundassembly = assembly;
                            break;
                        }
                        else
                        {
                            b = false;
                        }
                    }
                }

                catch (Exception ex)
                {
                    // throw ex;
                    // this happens for dynamic assemblies, so just ignore it. 
                    if (ex is NotSupportedException)
                        continue;
                }
            }
            if (b == true)
            {
                Type typ = foundassembly.GetType();
                object loObject = Activator.CreateInstance(typ);

                object[] loCodeParms = new object[1];
                loCodeParms[0] = stk;
                //  loCodeParms[0] = this.form1.Page;
                if (!objflag)
                {
                    object loResult = typ.InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);
                }
            }
            else
            {
                Assembly CompiledAssembly = CreateCompiledAssembly(CodeToCompile1, out strError);
                Type typ = CompiledAssembly.GetType("Filters.NameFilter");
                object loObject = Activator.CreateInstance(typ);

                object[] loCodeParms = new object[1];
                loCodeParms[0] = stk;
                //  loCodeParms[0] = this.form1.Page;
                if (!objflag)
                {
                    object loResult = typ.InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);
                }
            }
        }

        public void parse(string CodeToCompile, Boolean objflag, out string strError)
        {
            StackPanel stk = new StackPanel();
            string CodeToCompile1 =
             //"using System;" +
             "using System.Windows;" +
             "using System.Windows.Controls;" +
             //"using System.Windows.Data;" +
             //"using System.Windows.Documents;" +
             //"using System.Windows.Input;" +
             //"using System.Windows.Media;" +
             //"using System.Windows.Media.Imaging;" +
             //"using System.Windows.Navigation;" +
             //"using System.Windows.Shapes;" +
             //"using System.Collections; " +
             //"using System.Collections.Generic; " +
             // "using BPA.DesktopInfrastructure.BaseClasses; " +
             // "using BPA.AppConfig.BusinessEntity.Application;" +
             // "using System.Linq; " +
             "namespace Filters" +
              "{" +
              " public class NameFilter" +
              " { public NameFilter(){}  delegate object del();" +
              "  public object DynamicCode(params object[] Parameters){" +
               "   StackPanel pg1= (StackPanel) Parameters[0];" +
              CodeToCompile.Trim() + ";" +

              "    return (object)\"test\";" +
              "  }" +
              " }" +
              "}";
            Assembly CompiledAssembly = CreateCompiledAssembly(CodeToCompile1, out strError);
            if (CompiledAssembly != null)
            {
                Type typ = CompiledAssembly.GetType("Filters.NameFilter");
                object loObject = Activator.CreateInstance(typ);

                object[] loCodeParms = new object[1];
                loCodeParms[0] = stk;
                //  loCodeParms[0] = this.form1.Page;
                if (!objflag)
                {
                    object loResult = typ.InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);
                }

            }



        }


        public void ReadAssembly(string CodeToCompile, FrameworkElement stk, Boolean objflag, string str, string s)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic);

            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    string location = assembly.Location;

                    if (assembly.FullName == str.ToString())
                    {
                        Type typ = assembly.GetType("Filters." + s);

                        object loObject = Activator.CreateInstance(typ);

                        object[] loCodeParms = new object[1];
                        loCodeParms[0] = stk;
                        //  loCodeParms[0] = this.form1.Page;
                        if (!objflag)
                        {
                            object loResult = typ.InvokeMember("DynamicCode", BindingFlags.InvokeMethod, null, loObject, loCodeParms);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    // this happens for dynamic assemblies, so just ignore it. 
                    if (ex is NotSupportedException)
                        continue;

                }
            }



        }
    }
}