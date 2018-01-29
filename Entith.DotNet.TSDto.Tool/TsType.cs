using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Entith.DotNet.TSDto
{
    internal class TsType
    {
        private readonly Type _type;
        private readonly ConversionContext _context;

        internal TsType(Type type, ConversionContext context)
        {
            _type = type;
            _context = context;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            AddDeclaration(sb);
            AddProperties(sb);
            sb.AppendLine("}");
            sb.AppendLine();

            string result = sb.ToString();
            return result;
        }

        private void AddDeclaration(StringBuilder sb)
        {
            sb.Append($"export interface {_context.GenerateName(_type)}");

            AddParent(sb);

            sb.AppendLine(" {");
        }

        private void AddParent(StringBuilder sb)
        {
            if (_type.BaseType == typeof(object) || !_context.HasType(_type.BaseType))
                return;

            sb.Append($" extends {_context.GenerateName(_type.BaseType)}");
        }

        private void AddProperties(StringBuilder sb)
        {
            foreach (var prop in _type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                AddProperty(prop, sb);
        }

        private void AddProperty(PropertyInfo prop, StringBuilder sb)
        {
            Type type = prop.PropertyType;
            string typeName;
            string propName;

            bool isArray = false;
            if(type.IsArray)
            {
                isArray = true;
                type = type.GetElementType();
            }
            else if(typeof(IEnumerable<object>).IsAssignableFrom(type))
            {
                isArray = true;
                type = type.GetGenericArguments()[0];
            }

            if (Helper.IsNumericType(type))
                typeName = "number";
            else if (Type.GetTypeCode(type) == TypeCode.Boolean)
                typeName = "boolean";
            else if (_context.HasType(type))
                typeName = _context.GenerateName(type);
            else
                typeName = "string";

            if (isArray)
                typeName += "[]";

            propName = prop.Name.Substring(0, 1).ToLower() + prop.Name.Substring(1);

            sb.AppendLine($"  {propName}: {typeName};");
        }
    }
}
