using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IdentityUser.src.Domain.Enums
{
    public enum RoleEnum
    {
        [Description("Usuário com permissões administrativas")]
        Admin = 0,

        [Description("Usuário com permissões básicas")]
        User = 1
    }

   public class RoleEnumConverter : JsonConverter<RoleEnum>
{
    public override RoleEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Enum.TryParse<RoleEnum>(value, true, out var role) ? role : throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, RoleEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

}
