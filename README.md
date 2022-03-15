# WarnSystem
WarnSystemV2 is a second WarnSystem that use no DBO and only Synapse native database.

## Installation

1. [Install Synapse](https://github.com/SynapseSL/Synapse/wiki#hosting-guides).
2. Place the [dll](https://gitub.com/VT-DevGit/WarnSystem/releases/) file in your plugin directory.
3. Restart/Start your server.

## Specification

This system can go up to infinite warn per player. It display a broadcast to the player with a message (configurable).

## Permission
This plugin only contains one permission, for the command to warn other player. You can remove, add and see warn. Players are also able to see there warn in the client console with `.warning` command that doesn't require any permission.

```
- ws.warn	: add/see/remove warns
```
