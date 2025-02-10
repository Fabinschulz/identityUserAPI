using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IdentityUser.src.Domain.Enums
{
    /// <summary>
    /// Enum representing the roles in the system.
    /// </summary>
    public enum RoleEnum
    {
        /// <summary>
        /// Usuário com permissões administrativas
        /// </summary>
        [Description("Usuário com permissões administrativas")]
        Admin,

        /// <summary>
        /// Usuário com permissões básicas
        /// </summary>
        [Description("Usuário com permissões básicas")]
        User
    }

    /// <summary>
    /// Converter for RoleEnum to handle JSON serialization and deserialization.
    /// </summary>
    public class RoleEnumConverter : JsonConverter<RoleEnum>
    {
        /// <summary>
        /// Reads and converts the JSON to a RoleEnum.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>The converted RoleEnum.</returns>
        /// <exception cref="JsonException">Thrown when the JSON cannot be converted to RoleEnum.</exception>
        public override RoleEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    var intValue = reader.GetInt32();
                    if (Enum.IsDefined(typeof(RoleEnum), intValue))
                    {
                        return (RoleEnum)intValue;
                    }
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var value = reader.GetString();
                    if (Enum.TryParse<RoleEnum>(value, true, out var role))
                    {
                        return role;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new JsonException($"Failed to deserialize RoleEnum: {ex.Message}", ex);
            }

            throw new JsonException($"Unexpected token type for RoleEnum: {reader.TokenType}");
        }

        /// <summary>
        /// Writes and converts the RoleEnum to JSON.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The RoleEnum value to convert.</param>
        /// <param name="options">The serializer options.</param>
        public override void Write(Utf8JsonWriter writer, RoleEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

    }

}
