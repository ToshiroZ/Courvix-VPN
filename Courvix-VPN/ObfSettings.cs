using System.Reflection;


[assembly: Obfuscation(Feature = "apply to type *: apply to member * when method or constructor: virtualization", Exclude = false)]

[assembly: Obfuscation(Feature = "Overload Induction", Exclude = false)]
[assembly: Obfuscation(Feature = "code control flow obfuscation", Exclude = false)]
[assembly: Obfuscation(Feature = "embed Newtonsoft.Json.dll", Exclude = false)]
[assembly: Obfuscation(Feature = "embed Guna.UI2.dll", Exclude = false)]
[assembly: Obfuscation(Feature = "embed DiscordRPC.dll", Exclude = false)]
[assembly: Obfuscation(Feature = "encrypt resources", Exclude = false)]
[assembly: Obfuscation(Feature = "encrypt resources [compress]", Exclude = false)]