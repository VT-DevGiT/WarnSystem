# WarnSystem
WarnSystem is a warn system that use no DBO (DataBase Object) and only Synapse native database.
The Admin can warn a player and the warn is register.

## Installation

1. [Install Synapse](https://github.com/SynapseSL/Synapse/wiki#hosting-guides).
2. Place the [dll](https://github.com/VT-DevGit/WarnSystem/releases/) file in your plugin directory.
3. Restart/Start your server.

## Specification

This system can go up to infinite warn per player. Players are also able to see there warn in the client console with `.seemywarn` command that doesn't require any permission but can be disabled in the config. It display a broadcast to the player with a message (configurable).

## Permission
This plugin only contains one permission, for the command to warn other player. You can remove, add and see warn.

```
- ws.warn	: add/see/remove warns
```
