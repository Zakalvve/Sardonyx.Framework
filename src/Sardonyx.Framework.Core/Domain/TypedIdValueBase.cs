using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sardonyx.Framework.Core.Domain
{
    public abstract class TypedIdValueBase<T> : IEquatable<TypedIdValueBase<T>> where T : IEquatable<T>
    {
        [JsonProperty("id")]
        public T? Value { get; }

        protected TypedIdValueBase(T? value)
        {
            if ((value is string && string.IsNullOrWhiteSpace(value.ToString())) || (value is int? && value == null))
            {
                throw new InvalidOperationException("Id value cannot be empty!");
            }

            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is TypedIdValueBase<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(TypedIdValueBase<T> other)
        {
            return Value.Equals(other.Value);
        }

        public static bool operator ==(TypedIdValueBase<T> obj1, TypedIdValueBase<T> obj2)
        {
            if (Equals(obj1, null))
            {
                if (Equals(obj2, null))
                {
                    return true;
                }

                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(TypedIdValueBase<T> x, TypedIdValueBase<T> y)
        {
            return !(x == y);
        }
    }
    public class TypedIdValueBaseConverter<T> : JsonConverter where T : IEquatable<T>
    {
        // This converter should be applied only to types derived from TypedIdValueBase
        public override bool CanConvert(Type objectType)
        {
            return typeof(TypedIdValueBase<T>).IsAssignableFrom(objectType);
        }

        // Read JSON and convert to an instance of a derived TypedIdValueBase type
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);

            // Ensure the token is a string and create a new instance using reflection
            if (token.Type == JTokenType.String)
            {
                var value = token.ToString();

                // Use Activator to create an instance of the specific derived type (e.g., ListId, TaskId)
                return Activator.CreateInstance(objectType, value);
            }
            else if (token.Type == JTokenType.Integer)
            {
                var value = Convert.ToInt32(token);

                return Activator.CreateInstance(objectType, value);
            }

            throw new JsonSerializationException($"Unexpected token type {token.Type} when parsing TypedIdValueBase.");
        }

        // Write JSON from an instance of a derived TypedIdValueBase type
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TypedIdValueBase<T> typedId)
            {
                writer.WriteValue(typedId.Value);
            }
            else
            {
                throw new JsonSerializationException($"Expected value to be of type TypedIdValueBase, but was {value.GetType()}.");
            }
        }
    }
}
