# W0T.ReplayAnalyzer for .NET
This library helps you to analyze WorldofTanks replay files in .NET.

## Installation
This package is available via NuGet or you can download and compile it yourself.

## Dependencies
* Newtonsoft.Json (min.Version 5.0.1)

## Usage
First you need to create a instance of "Replay" and include the path to the replay file in the constructor.
```csharp
var replayFilePath = "C:\\dummy\\test.wotreplay";

Replay replay = new Replay(replayFilePath);
```
Then you can do different things:
1. You can get the replay settings (Map, Server, Region, etc.),
2. all Players of the Battle including their battle results or
3. all informations about the player who recorded the replay.

```csharp
replay.Battle();       // 1.
replay.Players();      // 2.
replay.MainPlayer();   // 3.
```

---

The "Battle" function returns a object of the type **BattleSettings**.

**BattleSettings** has the properties:
* regionCode
* serverName
* dateTime
* mapDisplayName
* mapName
* gameplayID
* battleType

---

The "Players" function returns a list of **Player**.

**Player** has the properties:
* id
* name
* clanAbbrev
* clanDBID
* crewGroup
* team
* vehicle
* preBattleID
* igrType
* forbidInBattleInvitations
* **results**

...while **results** has properties (**55** in total) like:
* xp
* damageDealt
* capturePoints
* shots
* credits
* kills
* health
* ...

---

The "MainPlayer" function returns a object of the type **MainPlayer**.

**MainPlayer** has properties (**176** in total) like:
* hasMods
* isPremium
* gold
* freeXP
* stunned
* team
* name
* clientVersionFromXml
* clientVersionFromExe
* vehicle
* **details**
* ...

... and **details** is a List of **PersonalDetails** which gives 
informations about the interaction with each player.

**PersonalDetails** has properties (**19** in total) like:
* id
* spotted
* crit
* fire
* damageDealt
* targetKills
* directHits
* ...

---

[Scriptec](https://github.com/ItsScriptec) 