using cubido.OpenApi.Angular.Extensions;
using cubido.OpenApi.Extensions;
using cubido.OpenApi.Models;
using cubido.OpenApi.Models.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace cubido.OpenApi.Angular.Template.Generated
{
    // T4 template classes do not offer typed parameters, nor have a common base class, hence duplicate code for each template class
    public static class Template
    {

        /// <summary>Creates the template output</summary>
        /// <param name="parameters">Parameter passed to the template.</param>
        public static string TransformText(Models.Model model, ConvertOptions options)
        {
            var template = new Model()
            {
                Session = new Dictionary<string, object>()
                {
                    { "model", model },
                    { "options", options }
                }
            };
            template.Initialize();
            return template.TransformText();
        }

        /// <summary>Creates the template output</summary>
        /// <param name="parameters">Parameter passed to the template.</param>
        public static string TransformText(Controller controller, ConvertOptions options)
        {
            var template = new Service()
            {
                Session = new Dictionary<string, object>()
                {
                    { "controller", controller },
                    { "options", options }
                }
            };
            template.Initialize();
            return template.TransformText();
        }

        // template cannot be included multiple times directly (gives compile error due to duplicate generated properties in .cs class), hence own helper method
        internal static string TransformServiceParameter(OperationParameter parameter)
        {
            var sb = new StringBuilder();

            var parameterKey = parameter.Name;
            if (parameterKey.Contains("-"))
            {
                sb.Append("'").Append(parameterKey).Append("'");
            }
            else
            {
                sb.Append(parameterKey);
            }

            sb.Append(": ");

            var parameterName = parameter.Name.ToLowerCamelCase();
            if (parameter.Type is StringType
                || parameter.Type is EnumType enumType1 && enumType1.Underlying is StringType
                || parameter.Type is FileType
                || parameter.Type is ArrayType arrayType1 && arrayType1.Inner is FileType)
            {
                sb.Append(parameterName.ToLowerCamelCase());
            }
            else if (parameter.Type is BooleanType || parameter.Type is NumberType || parameter.Type is EnumType enumType2 && enumType2.Underlying is NumberType)
            {
                sb.Append(parameterName).Append(" && ").Append(parameterName).Append(".toString()");
            }
            else if (parameter.Type is DateTimeType)
            {
                sb.Append(parameterName).Append(" && ").Append(parameterName).Append(".toJSON()");
            }
            else if (parameter.Type is ArrayType)
            {
                sb.Append(parameterName).Append(".join(',')");
            }
            else
            {
                sb.Append("JSON.stringify(").Append(parameterName).Append(")");
            }
            return sb.ToString();
        }

        internal static string TransformConstructor(AbstractType type, string parameter)
        {
            switch (type)
            {
                case AbstractType abstractType when type.IsPrimitive(): throw new InvalidOperationException("No constructor is needed for primitive types.");
                case ModelType modelType:
                case DateTimeType dateTimeType: return $"{parameter} && new {type.ToTypeScriptType()}({parameter})";
                case ArrayType arrayType: return $"{parameter} && {parameter}.map(item => {TransformConstructor(arrayType.Inner, "item")})";
                // TODO import { mapValues } from 'lodash'
                case DictionaryType dictionaryType:
                {
                    string helper;
                    switch (dictionaryType.Key)
                    {
                        case NumberType numberType: helper = "ServiceHelper.mapNumberDictionary"; break;
                        case StringType stringType: helper = "ServiceHelper.mapStringDictionary"; break;
                        default: throw new NotImplementedException($"Dictionary key of type {dictionaryType.Key.GetType().Name} is not implemented.");
                    }
                    return $"{parameter} && {helper}({parameter} as {dictionaryType.ToTypeScriptType()}, value => {TransformConstructor(dictionaryType.Value, "value")})";
                }
                default:  throw new NotImplementedException($"Type {type.GetType().Name} is not implemented.");
            }
        }
    }
}
