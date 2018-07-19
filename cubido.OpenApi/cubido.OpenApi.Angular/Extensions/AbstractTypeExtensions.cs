using cubido.OpenApi.Models.Types;
using System;
using System.Linq;

namespace cubido.OpenApi.Angular.Extensions
{
    public static class AbstractTypeExtensions
    {
        public static string ToTypeScriptType(this AbstractType abstractType)
        {
            switch (abstractType)
            {
                case null: return "any";
                case BooleanType booleanType: return "boolean";
                case NumberType numberType: return "number";
                case StringType stringType: return "string";
                case DateTimeType dateTimeType: return "Date";
                case FileType fileType: return "File";
                case ArrayType arrayType: return arrayType.Inner.ToTypeScriptType() + "[]";
                case EnumType<string> stringEnumType: return "( " + String.Join(" | ", stringEnumType.Values.Select(value => $"'{value}'")) + " )";
                case EnumType<int> numberEnumType: return "( " + String.Join(" | ", numberEnumType.Values) + " )";
                case ModelType modelType: return modelType.Model?.GetImportAlias();
                case DictionaryType dictionaryType:
                    return $"{{ [key: {dictionaryType.Key.ToTypeScriptType()}]: {dictionaryType.Value.ToTypeScriptType()}; }}";
                default: throw new NotImplementedException($"Type {abstractType.GetType()} is not implemented.");
            }
        }

        public static bool IsPrimitive(this AbstractType abstractType)
        {
            switch (abstractType)
            {
                case null: return true;
                case BooleanType booleanType:
                case NumberType numberType:
                case StringType stringType:
                case FileType fileType:
                case EnumType enumType: return true;
                case DateTimeType dateTimeType:
                case ModelType modelType: return false;
                case ArrayType arrayType: return arrayType.Inner.IsPrimitive();
                case DictionaryType dictionaryType: return dictionaryType.Key.IsPrimitive() && dictionaryType.Value.IsPrimitive();
                default: throw new NotImplementedException($"Type {abstractType.GetType()} is not implemented.");
            }
        }

    }
}
