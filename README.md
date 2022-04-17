# WarnSystem
WarnSystem is a plugin that let you put a warning on a player for a bad behavior. You can add/see/remove warn for each player and warns are stored in the Synapse Database.

## Installation

1. [Install Synapse](https://github.com/SynapseSL/Synapse/wiki#hosting-guides).
2. Place the [dll](https://github.com/VT-DevGit/WarnSystem/releases/) file in your plugin directory.
3. Restart/Start your server.

## Specification

This system can go up to infinite warn per player. Players are also able to see there warn in the client console with `.seemywarn` command that doesn't require any permission but can be disabled in the config. When a player is warned a message show to informe him of what he did.

## Permission
This plugin only contains 4 permissions, one for using the command, one for adding warns, one for seeing warns and last one for removing warns. In order to use for exemple `warn add` the group will need both `ws.warn` and `ws.add` permissions.

```
- ws.warn	: use warn command
- ws.add	: add warns to players
- ws.see	: see warn of a specific player
- ws.remove	: remove warn of a player
```
