using System;
using System.Linq.Expressions;
using DisposableSAPBO.RuntimeMapper.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Resources;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class QueryHelper
    {


        public static string ParseToHANAQuery<T>(Expression<Func<T, bool>> expression)
        {
            return make_query_from_binary(expression.Body.To<BinaryExpression>());
        }

        // ReSharper disable once UnusedParameter.Global
        // ReSharper disable once UnusedTypeParameter
        public static bool IntoParentheses(Expression<Func<bool>> expression)
        {
            return true;
        }


        private static string make_query_from_binary(BinaryExpression binaryExpression)
        {
            var result = string.Empty;

            Expression expressionLeft = binaryExpression.Left;
            if (expressionLeft is MemberExpression)
            {
                var expressionMember = expressionLeft.To<MemberExpression>().Member;
                if (expressionMember.GetCustomAttribute<SAPColumnAttribute>() != null)
                {
                    var sapColumnAttribute = expressionMember.GetCustomAttribute<SAPColumnAttribute>();
                    result += $"\"{sapColumnAttribute.Name}\"";
                }else if (expressionMember.GetCustomAttribute<ColumnProperty>() != null)
                {
                    var columnProperty = expressionMember.GetCustomAttribute<ColumnProperty>();
                    result += $"\"{columnProperty.ColumnName}\"";
                }
                
            }
            else if (expressionLeft is ConstantExpression)
                result += parse_value(expressionLeft.To<ConstantExpression>().Value);
            else if (expressionLeft is BinaryExpression)
                result += make_query_from_binary(expressionLeft.To<BinaryExpression>());
            else if (expressionLeft is UnaryExpression)
            {
                var attribute = expressionLeft.To<UnaryExpression>().Operand.To<MemberExpression>().Member.GetCustomAttribute<SAPColumnAttribute>();
                result += $"\"{attribute.Name}\"";
            }
            else if (expressionLeft is MethodCallExpression)
            {
                var unaryExpression = expressionLeft.To<MethodCallExpression>().Arguments[0].To<UnaryExpression>();
                var unaryExpressionOperand = unaryExpression.Operand.To<Expression<Func<bool>>>();
                var expression = unaryExpressionOperand.Body.To<BinaryExpression>();
                result += $"({make_query_from_binary(expression)})";
            }

            Expression expressionRight = binaryExpression.Right;

            result += $" {parse_expression_type(binaryExpression.NodeType, expressionRight)} ";

            if (expressionRight is MemberExpression)
            {
                Delegate compile = Expression.Lambda(expressionRight).Compile();
                object invoke = compile.DynamicInvoke();
                result += parse_value(invoke);
            }
            else if (expressionRight is ConstantExpression)
                result += parse_value(expressionRight.To<ConstantExpression>().Value);
            else if (expressionRight is BinaryExpression)
                result += make_query_from_binary(expressionRight.To<BinaryExpression>());

            return result;
        }

        private static string parse_value(object @object)
        {
            if (@object == null)
                return "null";
            if (@object is string)
                return $"'{@object}'";
            if (@object is DateTime)
                return $"TO_DATE('{(DateTime) @object:yyyy-MM-dd}', 'YYYY-MM-DD')";
            if (@object is bool)
                return @object.Equals(true)? "'Y'" : "'N'";
            return @object.ToString();
        }

        private static string parse_expression_type(ExpressionType expressionType, Expression expressionRight)
        {
            bool isNullRightExpressionValue = false;
            if (expressionRight is ConstantExpression)
                isNullRightExpressionValue = expressionRight.To<ConstantExpression>().Value == null;

            switch (expressionType)
            {
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.Equal:
                    return isNullRightExpressionValue ? "is" : "=";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "or";
                default:
                    throw new Exception(string.Format(ErrorMessages.MissmatchExpression, nameof(expressionType)));
            }
        }
    }
}