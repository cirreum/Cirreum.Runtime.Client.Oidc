# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Development Commands

### Build the project
```bash
dotnet build
```

### Run tests
```bash
dotnet test
```

### Pack NuGet package
```bash
dotnet pack --configuration Release
```

### Restore dependencies
```bash
dotnet restore
```

## Architecture Overview

This is a .NET 10.0 Blazor WebAssembly authentication library that provides OIDC (OpenID Connect) authentication support for Cirreum client applications. The library extends the Microsoft.AspNetCore.Components.WebAssembly.Authentication framework with custom claims handling and user profile enrichment capabilities.

### Key Components

1. **Authentication Builders**
   - `IOidcAuthenticationBuilder`: Interface for configuring OIDC authentication, extends `IUserProfileEnrichmentBuilder`
   - `OidcAuthenticationBuilder`: Concrete implementation that wraps the service collection

2. **Claims Handling**
   - `OidcClaimsPrincipalFactory`: Custom claims principal factory that:
     - Extends `CommonClaimsPrincipalFactory<RemoteUserAccount>` from Cirreum.Runtime.Client
     - Reads JWT tokens using Microsoft.IdentityModel.JsonWebTokens
     - Maps issuer claims and role claims from the JWT
     - Supports claims extenders and authentication post-processors

3. **Extension Methods**
   - `HostingExtensions.AddOidcAuth()`: Main entry point for configuring OIDC authentication
     - Configures OIDC provider options
     - Sets up standard authorization policies (Standard, StandardInternal, StandardAgent, StandardManager, StandardAdmin)
     - Supports custom claims extenders
   - `OidcAuthenticationExtensions`: Provides fluent API for:
     - Adding session monitoring
     - Adding application user services with custom loaders

### Dependencies
- Cirreum.Runtime.Client (v1.0.1) - Base client runtime library
- Microsoft.IdentityModel.JsonWebTokens (v8.15.0) - JWT token handling
- Microsoft.AspNetCore.Components.WebAssembly.Authentication - Core Blazor WASM auth

### Project Structure
- Target Framework: .NET 10.0
- SDK: Razor (supports Blazor components)
- Platform: Browser (WebAssembly)
- Namespace: `Cirreum.Runtime`

### Versioning
- Uses Directory.Build.props for centralized configuration
- Local release builds use version 1.0.100-rc by default
- CI/CD detection for Azure DevOps and GitHub Actions
- Follows semantic versioning with major version bumps being rare due to foundational nature