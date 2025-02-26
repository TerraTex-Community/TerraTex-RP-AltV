#Not Maintained
As I do not work on this actively anymore - it is just like trying from time to time stuff. Made it public as future reference

# Usefull Links
- altV-CustomCommandsSystem: https://github.com/emre1702/altv-CustomCommandsSystem/wiki


# How to generate Migration:
Go in server folder and execute in Paket-Manager-Console (Extras->Paket Manager->Konsole):   
`Add-Migration InitialCreate`

## Configs in server.toml

```
	[TerraTex.Database]
	host = ""
	user = ""
	password = ""
	database-name = ""

	[TerraTex.Email]
	enabled = true 
	host = ""
	port = 587
	user = ""
	password = ""
	enableSSL = true
	fromEmail = ""
	fromName = ""

	[TerraTex.General]
	# optional development config - enables some debug stuff and disables some permission checks
	isDevelopmentServer = false
```
