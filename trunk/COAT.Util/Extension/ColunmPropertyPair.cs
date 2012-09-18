using System;
using System.Linq.Expressions;
using System.Reflection;

namespace COAT.Extension
{
    public class ColunmPropertyPair
    {
        public int ColunmIndex { get; protected set; }
        public string ColunmName { get; protected set; }
        public string PropertyName { get; protected set; }
        public bool IsOptional { get; protected set; }

        public ColunmPropertyPair(int colunmIndex, string propName, bool isOptinal = false)
        {
            ColunmIndex = colunmIndex;
            PropertyName = propName;
            IsOptional = isOptinal;
        }

        public ColunmPropertyPair(string colunmName, string propName, bool isOptinal = false)
        {
            ColunmIndex = -1;
            ColunmName = colunmName;
            PropertyName = propName;
            IsOptional = isOptinal;
        }
    }


    public class ColunmPropertyPair<T> : ColunmPropertyPair
    {
        public ColunmPropertyPair(int colunmIndex, Expression<Func<T, dynamic>> expression, bool isOptinal = false)
            : base(colunmIndex, GetPropertyName(expression), isOptinal)
        {

        }

        public ColunmPropertyPair(string colunmName, Expression<Func<T, dynamic>> expression, bool isOptinal = false)
            : base(colunmName, GetPropertyName(expression), isOptinal)
        {

        }


        private static string GetPropertyName(Expression<Func<T, dynamic>> expression)
        {
            string propertyName = null;
            bool flag = false;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression body = (MemberExpression)expression.Body;
                        propertyName = (body.Member is PropertyInfo) ? body.Member.Name : null;
                        flag = true;
                        break;
                    }
            }
            if (!flag || propertyName == null)
            {
                throw new InvalidOperationException();
            }

            return propertyName;

        }
    }
}