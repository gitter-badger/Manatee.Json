/***************************************************************************************

	Copyright 2014 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		ArrayIndexExpression.cs
	Namespace:		Manatee.Json.Path.Expressions
	Class Name:		ArrayIndexExpression
	Purpose:		Expresses the intent to retrieve a value by providing a
					numeric array index.

***************************************************************************************/
using System;
using System.Linq;

namespace Manatee.Json.Path.Expressions
{
	internal class ArrayIndexExpression<T> : PathExpression<T>
	{
		public override int Priority => 6;
		public int Index { get; set; }
		public ExpressionTreeNode<T> IndexExpression { get; set; }

		public override object Evaluate(T json, JsonValue root)
		{
			var value = IsLocal ? json as JsonValue : root;
			if (value == null)
				throw new NotSupportedException("ArrayIndex requires a JsonValue to evaluate.");
			var results = Path.Evaluate(value);
			if (results.Count > 1)
				throw new InvalidOperationException($"Path '{Path}' returned more than one result on value '{value}'");
			var result = results.FirstOrDefault();
			var index = GetIndex();
			return result != null && result.Type == JsonValueType.Array && index >= 0 && index < result.Array.Count
					   ? result.Array[index]
					   : null;
		}
		public override string ToString()
		{
			var path = Path == null ? string.Empty : Path.GetRawString();
			return string.Format(IsLocal ? "@{0}[{1}]" : "${0}[{1}]", path, GetIndex());
		}

		private int GetIndex()
		{
			var value = IndexExpression?.Evaluate(default(T), null);
			if (value != null) return (int) value;
			return Index;
		}
	}
}