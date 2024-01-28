using System.Text.Json.Serialization;
using Core.Application.Models;

namespace Application.Models.Responses;

public sealed record AuthenticationResponseModel(TokenModel AccessToken, [property: JsonIgnore] TokenModel RefreshToken);
