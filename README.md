# PaladinsAPI
PaladinsAPI is a Class Library built in C# which can provide you the main endpoints from the paladins api.

## Installation and usage
1) Get your devId and authkey from https://fs12.formsite.com/HiRez/form48/secure_index.html  
2) Download PaladinsAPI from nuget: https://www.nuget.org/packages/PaladinsAPI 
3) Initialize the API:
```C#
var client = new PaladinsApi("DEV_ID_HERE", "DEV_PASS_HERE", Platform.PC|Platform.Xbox|Platform.Ps4);
var player = await client.GetPlayer("PLAYER_NAME_HERE");
```
4) Use methods described in documentation.  

## Documentation
TODO..