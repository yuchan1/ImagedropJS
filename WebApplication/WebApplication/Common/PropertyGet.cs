using System;
using System.Linq.Expressions;

namespace WebApplication.Common {

    /// <summary>
    /// PropertyGet: プロパティ名を取得するクラス(C#5.0でnameof演算子が使用できないための代用)
    /// </summary>
    internal static class PropertyGet {

        /// <summary>
        /// プロパティ名を取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e) {
            var memberEx = (MemberExpression)e.Body;
            return memberEx.Member.Name;
        }
    }
}
