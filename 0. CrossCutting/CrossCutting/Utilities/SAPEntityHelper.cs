using System;
using System.Linq.Expressions;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities
{
    public static class SAPEntityHelper
    {
        public static string DocumentNameOf<T>(Expression<Func<SAPbobsCOM.IDocuments, T>> function)
        {
            var binaryExpression = function.Body.To<MemberExpression>();
            return binaryExpression.Member.Name;
        }

        public static string DocumentLineNameOf<T>(Expression<Func<SAPbobsCOM.IDocument_Lines, T>> function)
        {
            var binaryExpression = function.Body.To<MemberExpression>();
            return binaryExpression.Member.Name;
        }
    }
}