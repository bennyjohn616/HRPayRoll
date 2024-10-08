// -----------------------------------------------------------------------
// <copyright file="EvaluateExpression.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PayrollBO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.CodeDom.Compiler;
    using Microsoft.CSharp;
    using System.Reflection;
    using TraceError;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EvaluateExpression
    {
        /// <summary>
        /// Test the expression like Ex- "-11+1-(1*1)+1/1"
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool TestExpression(string expression, out int output)
        {
            //example
            //TestExpression("-11+1-(1*1)+1/1");
            //TestExpression("2+1-(3*2)+8/2");
            //TestExpression("1*2*3*4*5*6");
            //TestExpression("Invalid expression");
            output = 0;
            try
            {
                int result = Evaluate(expression);
                output = result;
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
                return false;
            }
        }

        static int Evaluate(string expression)
        {
            string code = string.Format  // Note: Use "{{" to denote a single "{"
            (
                "public static class Func{{ public static int func(){{ return {0};}}}}",
                expression
            );
            CompilerResults compilerResults = CompileScript(code);
            if (compilerResults.Errors.HasErrors)
            {
                throw new InvalidOperationException("Expression has a syntax error.");
            }
            Assembly assembly = compilerResults.CompiledAssembly;
            MethodInfo method = assembly.GetType("Func").GetMethod("func");
            return (int)method.Invoke(null, null);
        }
        static CompilerResults CompileScript(string source)
        {
            CompilerParameters parms = new CompilerParameters();
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = false;
            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
            return compiler.CompileAssemblyFromSource(parms, source);
        }
    }
}
