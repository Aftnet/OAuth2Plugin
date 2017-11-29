[![Build status](https://ci.appveyor.com/api/projects/status/yhhaem05r4l0kdqv?svg=true)](https://ci.appveyor.com/project/Aftnet/oauth2plugin)
[![NuGet version](https://img.shields.io/nuget/v/Xam.Plugin.OAuth2.svg)](https://www.nuget.org/packages/Xam.Plugin.OAuth2/)

# OAuth2 and OpenID Connect Broker

Log in to OAuth2 providers in as little as two lines of code

```
var broker = CrossOAuth2.GetGoogleAccountBroker("your_app_id", "your_app_secret", "scopes_you_need");
var token = await Broker.GetAccessTokenAsync();
```

The plugin handles the OAuth2 Authorization Code flow for your app, displaying platform native webviews as needed for the user to do interactive authentication and grant permissions.

It also caches tokens in RAM where appropriate (Open Id Connect identity tokens are cached until expiry is reached for example) and has hooks to allow token saving and retrieval from permanent storage, like the platform's keychain. Needless to say, this works in combination with something like a [SecureStorage plugin](https://github.com/Aftnet/SecureStoragePlugin)

Main advantages:

- No messing around with registering app specific protocols and their handles
- No relying on mystery meat SDKs from identity providers
- No need for any special support from identity providers, if you can set the redirect uri to localhost you are good to go

## Supported platforms

- UWP
- Windows desktop (.NET 4.6.1+)
- iOS
- Android

## Usage

1. Register your app with the identity provider: if needing to choose app type, server side web application or "other" usually works best
2. Use the "Client id" and "Client secret" generated for the app you registered when calling `CrossOAuth2.Get-Broker-Name`). Choose a scope as needed. For OpenIdConnect providers, you'll also need their Authority URI.
3. *await* `GetAccessTokenAsync` (or `GetIDTokenAsync` for Open ID Connect providers, if you need the ID token).