namespace Cirreum.Runtime;

using Cirreum;
using Cirreum.Authorization;
using Cirreum.Runtime.Authentication;
using Cirreum.Runtime.Authentication.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;

public static class HostingExtensions {

	/// <summary>
	/// Adds standard Oidc Authentication.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="configure">Configure the <see cref="OidcProviderOptions"/>.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <param name="roleClaimType">The custom role claim type. Default: roles</param>
	/// <param name="nameClaimType">The custom name claim type. Default: name</param>
	/// <returns>The <see cref="IUserProfileEnrichmentBuilder"/> to support optional profile enrichment.</returns>
	public static IOidcAuthenticationBuilder AddOidcAuth(this IClientDomainApplicationBuilder builder,
		Action<OidcProviderOptions> configure,
		Action<AuthorizationOptions>? authorization = null,
		string roleClaimType = "roles",
		string nameClaimType = "name") {

		//
		// Authentication
		//
		builder.Services
			.AddOidcAuthentication(o => {
				o.UserOptions.NameClaim = nameClaimType;
				o.UserOptions.RoleClaim = roleClaimType;
				configure(o.ProviderOptions);
			})
			.AddAccountClaimsPrincipalFactory<OidcClaimsPrincipalFactory>();

		//
		// Authorization
		//
		builder.AddDefaultAuthorization(authorization);

		//
		// Return Enrichment Builder
		//
		return new OidcAuthenticationBuilder(builder.Services);

	}

	/// <summary>
	/// Adds standard Oidc Authentication and registers the defined <typeparamref name="TClaimsExtender"/>
	/// as a scoped service.
	/// </summary>
	/// <typeparam name="TClaimsExtender">The claims extender to register</typeparam>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="configure">Configure the <see cref="OidcProviderOptions"/>.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <param name="roleClaimType">The custom role claim type. Default: roles</param>
	/// <param name="nameClaimType">The custom name claim type. Default: name</param>
	/// <returns>The <see cref="IUserProfileEnrichmentBuilder"/> to support optional profile enrichment.</returns>
	public static IOidcAuthenticationBuilder AddOidcAuth<TClaimsExtender>(this IClientDomainApplicationBuilder builder,
		Action<OidcProviderOptions> configure,
		Action<AuthorizationOptions>? authorization = null,
		string roleClaimType = "roles",
		string nameClaimType = "name")
		where TClaimsExtender : class, IClaimsExtender {

		builder.Services.AddScoped<IClaimsExtender, TClaimsExtender>();

		return builder.AddOidcAuth(configure, authorization, roleClaimType, nameClaimType);

	}

}