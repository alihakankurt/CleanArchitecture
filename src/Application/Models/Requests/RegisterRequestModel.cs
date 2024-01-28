namespace Application.Models.Requests;

public sealed record RegisterRequestModel(string FirstName, string LastName, string Email, string Password, string ConfirmedPassword);
