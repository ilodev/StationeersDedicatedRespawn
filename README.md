# StationeersDedicatedRespawn

Plugin for Stationeers to define easier general custom starting/respawn globally and for specific 
players for both, client game and dedicated server.

### Installation
DedicatedRespawn Requires BepInEx 5.0.1 or later from 
https://github.com/BepInEx/BepInEx/releases

- Install BepInEx in the Stationeers (or Dedicated Server) steam folder.
- Launch the game, reach the main menu, then quit back out.
- In the steam folder of the game, there should now be a folder named BepInEx/Plugins.
- Copy the StationeersDedicatedRespawn folder from this mod into BepInEx/Plugins/ folder.
- Launch the game again.

### Configuration

The configuration of the plugin is through an XML file located in the game root folder
"stationeers dedicated server/dedicatedrespawn.xml" or
"stationeersDedicated/dedicatedrespawn.xml":

Note, missing entries in the config file wont be used, e.g. if you don't specify a default respawn condition
the plugin will not override the current value from the settings file.

#### Example XML file 1

```xml
<?xml version="1.0" encoding="utf-8"?>
<Config xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <DefaultStartCondition>Normal</DefaultStartCondition>
  <DefaultRespawnCondition>Normal</DefaultRespawnCondition>
</Config>   
```
This config file will setup Normal conditions on spawn and respawn for all players.

#### Example XML file 2

ConfigItems are required to specify the steamId of the players to change their spawning options.
Players not included in the config file will still use the default values.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Config xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Items>
    <ConfigItem>
      <steamId>12345678</steamId>
      <StartCondition>Vulcan_Green</StartCondition>
      <RespawnCondition>Minimal</RespawnCondition>
    </ConfigItem>
    <ConfigItem>
      <steamId>87654321</steamId>
      <StartCondition>Vulcan</StartCondition>
      <RespawnCondition>Stationeer</RespawnCondition>
    </ConfigItem>
  </Items>
  <DefaultStartCondition>Normal</DefaultStartCondition>
  <DefaultRespawnCondition>Normal</DefaultRespawnCondition>
</Config>   
```

#### Example XML file 3

Default conditions are only required when you want to change the game current conditions. The following xml 
file will only change the two players conditions, but will still use the game defined conditions for everyone else.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Config xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Items>
    <ConfigItem>
      <steamId>12345678</steamId>
      <StartCondition>Vulcan_Green</StartCondition>
      <RespawnCondition>Minimal</RespawnCondition>
    </ConfigItem>
    <ConfigItem>
      <steamId>87654321</steamId>
      <StartCondition>Vulcan</StartCondition>
      <RespawnCondition>Stationeer</RespawnCondition>
    </ConfigItem>
  </Items>
</Config>   
```
