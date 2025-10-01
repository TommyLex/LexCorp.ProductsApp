using Microsoft.AspNetCore.Identity;
using System;

namespace LexCorp.Products.Data.Models.Auth
{
  /// <summary>
  /// Represents an application user with a GUID as the primary key.
  /// </summary>
  public class User : IdentityUser<Guid>
  {
  }
}
