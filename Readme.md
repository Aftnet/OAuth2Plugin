#OpenID Connect Broker

Encapsulates the logic needed to get Open ID Connect tokens from colpliant identity providers.
In particular, it provides functinality for:

- Displaying login and user consent screen
- Saving refresh tokens using the system's secure password store
- Getting tokens and renewing them when their valdity exires

##Installation

###Universal apps

```
Install-Package AppStorageService.Universal
```

###Classic desktop applications

```
Install-Package AppStorageService.Desktop
```

##Usage

Coming soon