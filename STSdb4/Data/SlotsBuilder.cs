using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace STSdb4.Data
{
    public class SlotsBuilder
    {
        private static ConcurrentDictionary<TypeArray, Type> map = new ConcurrentDictionary<TypeArray, Type>();

        private static Type BuildType(Type baseInterface, string className, string fieldsPrefix, params Type[] types)
        {
            if (className == null)
                throw new ArgumentNullException("className");

            if (fieldsPrefix == null)
                throw new ArgumentNullException("fieldsPrefix");

            if (types.Length == 0)
                throw new ArgumentException("types.Length == 0");

            if (STSdb4.General.Environment.RunningOnMono)
                return BuildTypeCodeDom(baseInterface, className, fieldsPrefix, types);
            else
                return BuildTypeEmit(baseInterface, className, fieldsPrefix, types);
        }

        private static Type BuildTypeEmit(Type baseInterface, string className, string fieldsPrefix, params Type[] types)
        {
            var assemblyName = new AssemblyName(className);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            string[] genericParameters = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
                genericParameters[i] = "T" + fieldsPrefix + i;

            TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Class | TypeAttributes.Public);

            if(baseInterface != null)
                typeBuilder.AddInterfaceImplementation(baseInterface);

            CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes), new object[] { });
            typeBuilder.SetCustomAttribute(customAttribute);

            var typeParams = typeBuilder.DefineGenericParameters(genericParameters);

            FieldBuilder[] fields = new FieldBuilder[types.Length];
            for (int i = 0; i < types.Length; i++)
                fields[i] = typeBuilder.DefineField(fieldsPrefix + i, typeParams[i], FieldAttributes.Public);

            var defConstructor = typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            var constr = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, typeParams);
            var ilGenerator = constr.GetILGenerator();

            for (int i = 0; i < types.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldarg_S, i + 1);
                ilGenerator.Emit(OpCodes.Stfld, fields[i]);
            }

            ilGenerator.Emit(OpCodes.Ret);

            return typeBuilder.CreateType().MakeGenericType(types);
        }

        private static Type BuildTypeCodeDom(Type baseInterface, string className, string fieldsPrefix, params Type[] types)
        {
            var compileUnit = new CodeCompileUnit();
            CodeNamespace globalNamespace = new CodeNamespace();

            globalNamespace.Imports.Add(new CodeNamespaceImport("System"));
            globalNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            globalNamespace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            globalNamespace.Imports.Add(new CodeNamespaceImport("System.Text"));

            var classNamespace = new CodeNamespace("STSdb4.Data");

            var generatedClass = new CodeTypeDeclaration(className);
            generatedClass.IsClass = true;
            generatedClass.Attributes = MemberAttributes.Public;

            for (int i = 0; i < types.Length; i++)
                generatedClass.TypeParameters.Add(new CodeTypeParameter("T" + fieldsPrefix + i));

            if(baseInterface != null)
                generatedClass.BaseTypes.Add(baseInterface);

            var serializableAttribute = new CodeTypeReference(typeof(System.SerializableAttribute));
            generatedClass.CustomAttributes.Add(new CodeAttributeDeclaration(serializableAttribute));

            classNamespace.Types.Add(generatedClass);

            compileUnit.Namespaces.Add(globalNamespace);
            compileUnit.Namespaces.Add(classNamespace);

            CodeMemberField[] fields = new CodeMemberField[types.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = new CodeMemberField("T" + fieldsPrefix + i, fieldsPrefix + i);
                fields[i].Attributes = MemberAttributes.Public;
                generatedClass.Members.Add(fields[i]);
            }

            CodeConstructor defaultConstructor = new CodeConstructor();
            defaultConstructor.Attributes = MemberAttributes.Public;

            generatedClass.Members.Add(defaultConstructor);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;

            for (int i = 0; i < types.Length; i++)
            {
                CodeTypeReference type = new CodeTypeReference("T" + fieldsPrefix + i);
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(type, fieldsPrefix.ToLower() + i));
            }

            for (int i = 0; i < types.Length; i++)
            {
                CodeFieldReferenceExpression left = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldsPrefix + i);
                constructor.Statements.Add(new CodeAssignStatement(left, new CodeArgumentReferenceExpression(fieldsPrefix.ToLower() + i)));
            }

            generatedClass.Members.Add(constructor);

            string stsdbAssemblyName = Assembly.GetExecutingAssembly().Location;
            string[] assemblies = { "System.dll", "mscorlib.dll", stsdbAssemblyName };

            CompilerParameters parameters = new CompilerParameters(assemblies);

            CodeDomProvider runTimeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            parameters = new CompilerParameters(assemblies);

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.IncludeDebugInformation = true;
            parameters.CompilerOptions = "/optimize";

            CompilerResults compilerResults = runTimeProvider.CompileAssemblyFromDom(parameters, compileUnit);
            var generatedType = compilerResults.CompiledAssembly.GetTypes()[0];

            return generatedType.MakeGenericType(types);
        }

        public static Type BuildType(params Type[] types)
        {
            if (types.Length == 0)
                throw new ArgumentException("types array is empty.");

            switch (types.Length)
            {
                case 01: return typeof(Slots<>).MakeGenericType(types);
                case 02: return typeof(Slots<,>).MakeGenericType(types);
                case 03: return typeof(Slots<,,>).MakeGenericType(types);
                case 04: return typeof(Slots<,,,>).MakeGenericType(types);
                case 05: return typeof(Slots<,,,,>).MakeGenericType(types);
                case 06: return typeof(Slots<,,,,,>).MakeGenericType(types);
                case 07: return typeof(Slots<,,,,,,>).MakeGenericType(types);
                case 08: return typeof(Slots<,,,,,,,>).MakeGenericType(types);
                case 09: return typeof(Slots<,,,,,,,,>).MakeGenericType(types);
                case 10: return typeof(Slots<,,,,,,,,,>).MakeGenericType(types);
                case 11: return typeof(Slots<,,,,,,,,,,>).MakeGenericType(types);
                case 12: return typeof(Slots<,,,,,,,,,,,>).MakeGenericType(types);
                case 13: return typeof(Slots<,,,,,,,,,,,,>).MakeGenericType(types);
                case 14: return typeof(Slots<,,,,,,,,,,,,,>).MakeGenericType(types);
                case 15: return typeof(Slots<,,,,,,,,,,,,,,>).MakeGenericType(types);
                case 16: return typeof(Slots<,,,,,,,,,,,,,,,>).MakeGenericType(types);
            }

            return map.GetOrAdd(new TypeArray(types), BuildType(typeof(ISlots), "Slots", "Slot", types));
        }

        private class TypeArray : IEquatable<TypeArray>
        {
            private int? hashcode;

            public readonly Type[] Types;

            public TypeArray(Type[] types)
            {
                Types = types;
            }

            public bool Equals(TypeArray other)
            {
                if (Object.ReferenceEquals(this, other))
                    return true;

                if (Object.ReferenceEquals(other, null))
                    return false;

                if (this.Types.Length != other.Types.Length)
                    return false;

                for (int i = 0; i < Types.Length; i++)
                {
                    if (this.Types[i] != other.Types[i])
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                if (hashcode == null)
                {
                    int code = 0;
                    for (int i = 0; i < Types.Length; i++)
                        code ^= Types[i].GetHashCode();

                    hashcode = code;
                }

                return hashcode.Value;
            }
        }
    }
}
