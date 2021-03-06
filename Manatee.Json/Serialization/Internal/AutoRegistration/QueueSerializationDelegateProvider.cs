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
 
	File Name:		DictionarySerializationDelegateProvider.cs
	Namespace:		Manatee.Json.Serialization.Internal.AutoRegistration
	Class Name:		DictionarySerializationDelegateProvider
	Purpose:		Provides delegates for serializing Queue types.

***************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manatee.Json.Serialization.Internal.AutoRegistration
{
	internal class QueueSerializationDelegateProvider : SerializationDelegateProviderBase
	{
		public override bool CanHandle(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Queue<>);
		}

		private static JsonValue Encode<T>(Queue<T> queue, JsonSerializer serializer)
		{
			var array = new JsonArray();
			for (int i = 0; i < queue.Count; i++)
			{
				array.Add(serializer.Serialize(queue.ElementAt(i)));
			}
			return array;
		}
		private static Queue<T> Decode<T>(JsonValue json, JsonSerializer serializer)
		{
			var queue = new Queue<T>();
			for (int i = 0; i < json.Array.Count; i++)
			{
				queue.Enqueue(serializer.Deserialize<T>(json.Array[i]));
			}
			return queue;
		}
	}
}