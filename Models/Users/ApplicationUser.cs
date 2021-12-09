﻿using System.Text.Json.Serialization;

using MeAnotoApi.Information;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Identity;

namespace MeAnotoApi.Models.Users;
/// <summary>
/// Appplication user
/// </summary>
public class ApplicationUser : IdentityUser {
	/// <summary>
	/// Chilean RUN
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Run)]
	public string Run { get; set; }
	/// <summary>
	/// First name
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.FirstName)]
	public string FirstName { get; set; }
	/// <summary>
	/// Last name
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.LastName)]
	public string LastName { get; set; }
	/// <summary>
	/// Institution
	/// </summary>
	[JsonIgnore]
	public Institution Institution { get; set; }
}
