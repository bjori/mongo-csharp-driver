﻿/* Copyright 2015 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver.Relinq.Structure.Expressions
{
    internal class DocumentWrappedFieldExpression : SerializationExpression, IFieldExpression
    {
        private readonly Expression _expression;
        private readonly string _fieldName;
        private readonly IBsonSerializer _serializer;

        public DocumentWrappedFieldExpression(Expression expression, string fieldName, IBsonSerializer serializer)
        {
            _expression = Ensure.IsNotNull(expression, "expression");
            _fieldName = Ensure.IsNotNull(fieldName, "fieldName");
            _serializer = Ensure.IsNotNull(serializer, "serializer");
        }

        public Expression Expression
        {
            get { return _expression; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        public override MongoExpressionType MongoNodeType
        {
            get { return MongoExpressionType.DocumentWrappedField; }
        }

        public override ExpressionType NodeType
        {
            get { return ExpressionType.Extension; }
        }

        public override IBsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public override Type Type
        {
            get { return _expression.Type; }
        }

        public override string ToString()
        {
            return "[" + _fieldName + "]";
        }

        public DocumentWrappedFieldExpression Update(Expression expression)
        {
            if (expression != _expression)
            {
                return new DocumentWrappedFieldExpression(expression, _fieldName, _serializer);
            }

            return this;
        }

        protected override Expression Accept(MongoExpressionVisitor visitor)
        {
            return visitor.VisitDocumentWrappedField(this);
        }
    }
}
