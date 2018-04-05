using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace W0T.ReplayAnalyzer
{
    //   R E P L A Y
    //   The class Replay gets all important informations from the json blocks.
    //   Methods: Battle, Players, MainPlayer

    public class Replay
    {
        // variables
        JObject blockOne;
        JObject blockTwo;
        List<Player> players = new List<Player>();

        // constructor
        public Replay(string replayFilePath)
        {
            var converter = new Converter(replayFilePath).ToJson();

            // get the first json block
            blockOne = converter.BlockOne;

            // get the second json block
            JArray blockTwoArray = JsonConvert.DeserializeObject<JArray>(converter.BlockTwo.ToString());
            blockTwo = (JObject)blockTwoArray.First;
        }

        // get the settings of the battle
        public BattleSettings Battle()
        {
            BattleSettings set = new BattleSettings();

            set.regionCode = (string)blockOne["regionCode"];
            set.serverName = (string)blockOne["serverName"];
            set.dateTime = (string)blockOne["dateTime"];
            set.mapDisplayName = (string)blockOne["mapDisplayName"];
            set.mapName = (string)blockOne["mapName"];
            set.gameplayID = (string)blockOne["gameplayID"];
            set.battleType = (string)blockOne["battleType"];

            return set;       
        }

        // get the players (with battle-results)
        public List<Player> Players()
        {
            // get the "vehicles" json-part
            JObject vehiclesJson = JObject.Parse(blockOne["vehicles"].ToString());

            // for each player in "vehicles"
            foreach (var player in vehiclesJson.Properties())
            {
                // create a new instance of Player class
                Player currentPlayer = new Player();

                // get the players json part
                JObject playerJson = JObject.Parse(player.Value.ToString());

                // STANDARD INFORMATIONS
                currentPlayer.id = Convert.ToInt32(player.Name);
                currentPlayer.name = (string)playerJson["name"];
                currentPlayer.team = (int)playerJson["team"];
                currentPlayer.vehicle = (string)playerJson["vehicleType"];
                currentPlayer.crewGroup = (int?)playerJson["crewGroup"];
                currentPlayer.clanAbbrev = (string)playerJson["clanAbbrev"];
                currentPlayer.forbidInBattleInvitations = (bool?)playerJson["forbidInBattleInvitations"];

                // get the player-results json part
                JObject allresults = blockTwo?.ToObject<JObject>();
                allresults = allresults?["vehicles"]?.ToObject<JObject>();

                // if the results part is not null (older replays don't have this part)
                if (allresults != null)
                {
                    // get the right json part
                    JObject playerresults = JObject.Parse(JArray.Parse(allresults[player.Name].ToString())[0].ToString());

                    // create new instance of results
                    results res = new results();

                    // XP
                    res.xp = (int)playerresults["xp"];
                    res.achievementXP = (int)playerresults["achievementXP"];
                    res.achievementFreeXP = (int)playerresults["achievementFreeXP"];

                    // CREDITS
                    res.credits = (int)playerresults["credits"];
                    res.achievementCredits = (int)playerresults["achievementCredits"];

                    // DID
                    res.shots = (int)playerresults["shots"];
                    res.explosionHits = (int)playerresults["explosionHits"];
                    res.directHits = (int)playerresults["directHits"];
                    res.piercings = (int)playerresults["piercings"];
                    res.directTeamHits = (int)playerresults["directTeamHits"];
                    res.kills = (int)playerresults["kills"];
                    res.damaged = (int)playerresults["damaged"];
                    res.damageDealt = (int)playerresults["damageDealt"];
                    res.sniperDamageDealt = (int)playerresults["sniperDamageDealt"];
                    res.damageEventList = playerresults["damageEventList"].ToObject<int?[]>();

                    res.isTeamKiller = (bool)playerresults["isTeamKiller"];
                    res.tkills = (int)playerresults["tkills"];
                    res.tdamageDealt = (int)playerresults["tdamageDealt"];
                    res.tdestroyedModules = (int)playerresults["tdestroyedModules"];

                    // ASSIST
                    res.spotted = (int)playerresults["spotted"];
                    res.damageAssistedRadio = (int)playerresults["damageAssistedRadio"];
                    res.damageAssistedTrack = (int)playerresults["damageAssistedTrack"];
                    res.stunNum = (int?)playerresults["stunNum"];
                    res.stunDuration = (double?)playerresults["stunDuration"];
                    res.stunned = (int?)playerresults["stunned"];
                    res.damageAssistedStun = (int?)playerresults["damageAssistedStun"];

                    // RECEIVED & BLOCKED
                    res.directHitsReceived = (int)playerresults["directHitsReceived"];
                    res.potentialDamageReceived = (int)playerresults["potentialDamageReceived"];
                    res.damageReceived = (int)playerresults["damageReceived"];
                    res.damageReceivedFromInvisible = (int?)playerresults["damageReceivedFromInvisible"];
                    res.piercingsReceived = (int)playerresults["piercingsReceived"];
                    res.noDamageDirectHitsReceived = (int)playerresults["noDamageDirectHitsReceived"];
                    res.explosionHitsReceived = (int)playerresults["explosionHitsReceived"];
                    res.damageBlockedByArmor = (int)playerresults["damageBlockedByArmor"];

                    // FLAG
                    res.soloFlagCapture = (int)playerresults["soloFlagCapture"];
                    res.capturePoints = (int)playerresults["capturePoints"];
                    res.flagCapture = (int)playerresults["flagCapture"];
                    res.capturingBase = (int?)playerresults["capturingBase"];
                    res.flagActions = playerresults["flagActions"].ToObject<int[]>();
                    res.droppedCapturePoints = (int)playerresults["droppedCapturePoints"];

                    // DEATH
                    res.killerID = (int)playerresults["killerID"];
                    res.deathReason = (int)playerresults["deathReason"];
                    res.deathCount = (int)playerresults["deathCount"];
                    res.lifeTime = (int)playerresults["lifeTime"];

                    // OTHERS
                    res.health = (int)playerresults["health"];
                    res.extPublic = (object)playerresults["extPublic"];
                    res.stopRespawn = (bool)playerresults["stopRespawn"];
                    res.index = (int)playerresults["index"];
                    res.achievements = playerresults["achievements"].ToObject<string[]>();
                    res.mileage = (int)playerresults["mileage"];
                    res.resourceAbsorbed = (int)playerresults["resourceAbsorbed"];
                    res.accountDBID = (int)playerresults["accountDBID"];
                    res.typeCompDescr = (int)playerresults["typeCompDescr"];
                    res.rolloutsCount = (int)playerresults["rolloutsCount"];
                    res.winPoints = (int)playerresults["winPoints"];

                    // set the result of the current player
                    currentPlayer.results = res;
                }

                // add the current player to the Players List
                players.Add(currentPlayer);
            }

            // return the List of all players
            return players;
        }

        // get all informations about the recording player
        public MainPlayer MainPlayer()
        {
            // get the players name
            string name = Convert.ToString(blockOne["playerName"]);

            // create instance for the main player from the players list
            Player thisplayer = new Player();

            // create instance for the main player
            MainPlayer myplayer = new MainPlayer();

            // start the Players functon to get a list of all players
            Players();

            // for each player in the players list
            foreach (Player player in players)
            {
                // if the players name == mainplayers name
                if (player.name == name)
                {
                    // set thisplayer as the current player
                    thisplayer = player;
                    break;
                }
            }

            // STANDARD INFORMATIONS
            myplayer.id = (string)blockOne["playerID"];
            myplayer.name = thisplayer.name;
            myplayer.clanAbbrev = thisplayer.clanAbbrev;
            myplayer.clanDBID = thisplayer.clanDBID;
            myplayer.crewGroup = thisplayer.crewGroup;
            myplayer.team = thisplayer.team;
            myplayer.vehicle = thisplayer.vehicle;
            myplayer.preBattleID = thisplayer.preBattleID;
            myplayer.igrType = thisplayer.igrType;
            myplayer.forbidInBattleInvitations = thisplayer.forbidInBattleInvitations;
            myplayer.hasMods = (bool?)blockOne["hasMods"];
            myplayer.clientVersionFromXml = (string)blockOne["clientVersionFromXml"];
            myplayer.clientVersionFromExe = (string)blockOne["clientVersionFromExe"];

            // get the "personal" json part
            var personal = JObject.Parse(blockTwo.ToString());
            personal = personal["personal"]?.ToObject<JObject>();
            personal = personal?.First?.First?.ToObject<JObject>();

            // if the "personal" part is not null (older replays don't have this part)
            if (personal != null)
            {
         ////////// XP

                // XP FACTOR 100
                myplayer.orderFreeXPFactor100 = (int)personal["orderFreeXPFactor100"];
                myplayer.orderXPFactor100 = (int)personal["orderXPFactor100"];
                myplayer.boosterTMenXPFactor100 = (int)personal["boosterTMenXPFactor100"];
                myplayer.premiumVehicleXPFactor100 = (int?)personal["premiumVehicleXPFactor100"];
                myplayer.boosterXPFactor100 = (int)personal["boosterXPFactor100"];
                myplayer.orderTMenXPFactor100 = (int)personal["orderTMenXPFactor100"];
                myplayer.squadXPFactor100 = (int?)personal["squadXPFactor100"];
                myplayer.boosterFreeXPFactor100 = (int)personal["boosterFreeXPFactor100"];

                // XP FACTOR 10
                myplayer.premiumXPFactor10 = (int)personal["premiumXPFactor10"];
                myplayer.igrXPFactor10 = (int)personal["igrXPFactor10"];
                myplayer.dailyXPFactor10 = (int)personal["dailyXPFactor10"];
                myplayer.refSystemXPFactor10 = (int)personal["refSystemXPFactor10"];
                myplayer.appliedPremiumXPFactor10 = (int)personal["appliedPremiumXPFactor10"];

                // XP VALUES
                myplayer.xp = (int)personal["xp"];
                myplayer.originalXP = (int)personal["originalXP"];
                myplayer.orderXP = (int)personal["orderXP"];
                myplayer.achievementXP = (int)personal["achievementXP"];
                myplayer.eventTMenXP = (int)personal["eventTMenXP"];
                myplayer.eventXP = (int)personal["eventXP"];
                myplayer.tmenXP = (int)personal["tmenXP"];
                myplayer.originalTMenXP = (int)personal["originalTMenXP"];
                myplayer.subtotalTMenXP = (int)personal["subtotalTMenXP"];
                myplayer.factualXP = (int)personal["factualXP"];
                myplayer.eventFreeXP = (int)personal["eventFreeXP"];
                myplayer.orderFreeXP = (int)personal["orderFreeXP"];
                myplayer.freeXP = (int)personal["freeXP"];
                myplayer.subtotalFreeXP = (int)personal["subtotalFreeXP"];
                myplayer.originalFreeXP = (int)personal["originalFreeXP"];
                myplayer.achievementFreeXP = (int)personal["achievementFreeXP"];
                myplayer.orderTMenXP = (int)personal["orderTMenXP"];
                myplayer.boosterTMenXP = (int)personal["boosterTMenXP"];
                myplayer.boosterXP = (int)personal["boosterXP"];
                myplayer.boosterFreeXP = (int)personal["boosterFreeXP"];
                myplayer.subtotalXP = (int)personal["subtotalXP"];
                myplayer.squadXP = (int?)personal["squadXP"];
                myplayer.premiumVehicleXP = (int)personal["premiumVehicleXP"];
                myplayer.factualFreeXP = (int)personal["factualFreeXP"];

                // XP PENALTY
                myplayer.originalXPPenalty = (int)personal["originalXPPenalty"];
                myplayer.xpPenalty = (int)personal["xpPenalty"];

                // XP REPLAY
                myplayer.freeXPReplay = (int?)personal["freeXPReplay"];
                myplayer.xpReplay = (int?)personal["xpReplay"];
                myplayer.tmenXPReplay = (int?)personal["tmenXPReplay"];

                // XP LISTS
                myplayer.eventXPList = personal["eventXPList"].ToObject<int[]>();
                myplayer.eventFreeXPList = personal["eventFreeXPList"].ToObject<int[]>();
                myplayer.eventTMenXPList = personal["eventTMenXPList"].ToObject<int[]>();
                myplayer.eventXPFactor100List = (object)personal["eventXPFactor100List"];
                myplayer.eventFreeXPFactor100List = personal["eventFreeXPFactor100List"].ToObject<int[]>();
                myplayer.eventTMenXPFactor100List = (object)personal["eventTMenXPFactor100List"];
                myplayer.xpByTmen = personal["xpByTmen"].ToObject<int[][]>();

         ////////// CREDITS

                // CREDIT FACTOR 100
                myplayer.boosterCreditsFactor100 = (int)personal["boosterCreditsFactor100"];
                myplayer.orderCreditsFactor100 = (int)personal["orderCreditsFactor100"];

                // CREDIT FACTOR 10
                myplayer.premiumCreditsFactor10 = (int)personal["premiumCreditsFactor10"];
                myplayer.appliedPremiumCreditsFactor10 = (int)personal["appliedPremiumCreditsFactor10"];

                // CREDIT VALUES
                myplayer.credits = (int)personal["credits"];
                myplayer.originalCredits = (int)personal["originalCredits"];
                myplayer.boosterCredits = (int)personal["boosterCredits"];
                myplayer.creditsToDraw = (int)personal["creditsToDraw"];
                myplayer.originalCreditsToDraw = (int?)personal["originalCreditsToDraw"];
                myplayer.eventCredits = (int)personal["eventCredits"];
                myplayer.orderCredits = (int)personal["orderCredits"];
                myplayer.factualCredits = (int)personal["factualCredits"];
                myplayer.subtotalCredits = (int)personal["subtotalCredits"];
                myplayer.creditsReplay = (int?)personal["creditsReplay"];
                myplayer.achievementCredits = (int)personal["achievementCredits"];

                // CREDIT PENALTY
                myplayer.originalCreditsPenalty = (int)personal["originalCreditsPenalty"];
                myplayer.creditsPenalty = (int)personal["creditsPenalty"];

                // CREDIT CONTRIBUTION
                myplayer.creditsContributionIn = (int)personal["creditsContributionIn"];
                myplayer.creditsContributionOut = (int)personal["creditsContributionOut"];
                myplayer.originalCreditsContributionIn = (int)personal["originalCreditsContributionIn"];
                myplayer.originalCreditsContributionOut = (int)personal["originalCreditsContributionOut"];

                // CREDIT LISTS
                myplayer.eventCreditsList = personal["eventCreditsList"].ToObject<int[]>();
                myplayer.eventCreditsFactor100List = personal["eventCreditsFactor100List"].ToObject<int[]>();

         ////////// DID

                // SHOTS
                myplayer.shots = (int)personal["shots"];
                myplayer.directHits = (int)personal["directHits"];
                myplayer.explosionHits = (int)personal["explosionHits"];
                myplayer.piercings = (int)personal["piercings"];
                myplayer.damagedWhileMoving = (int)personal["damagedWhileMoving"];
                myplayer.damagedWhileEnemyMoving = (int)personal["damagedWhileEnemyMoving"];
                myplayer.killedAndDamagedByAllSquadmates = (int)personal["killedAndDamagedByAllSquadmates"];
                myplayer.killsBeforeTeamWasDamaged = (int)personal["killsBeforeTeamWasDamaged"];
                myplayer.damaged = (int)personal["damaged"];
                myplayer.kills = (int)personal["kills"];

                // DAMAGE
                myplayer.percentFromTotalTeamDamage = (double)personal["percentFromTotalTeamDamage"];
                myplayer.percentFromSecondBestDamage = (double)personal["percentFromSecondBestDamage"];
                myplayer.damageRating = (int)personal["damageRating"];
                myplayer.damageEventList = (int?)personal["damageEventList"];
                myplayer.damageDealt = (int)personal["damageDealt"];
                myplayer.sniperDamageDealt = (int)personal["sniperDamageDealt"];
                myplayer.movingAvgDamage = (int)personal["movingAvgDamage"];
                myplayer.damageBeforeTeamWasDamaged = (int)personal["damageBeforeTeamWasDamaged"];
                myplayer.avatarDamageEventList = personal["avatarDamageEventList"].ToObject<int?[]>();

                // AT TEAM
                myplayer.tdamageDealt = (int)personal["tdamageDealt"];
                myplayer.tkills = (int)personal["tkills"];
                myplayer.isTeamKiller = (bool)personal["isTeamKiller"];
                myplayer.tdestroyedModules = (int)personal["tdestroyedModules"];
                myplayer.directTeamHits = (int)personal["directTeamHits"];

                // ASSISTED
                myplayer.damageAssistedTrack = (int)personal["damageAssistedTrack"];
                myplayer.damageAssistedRadio = (int)personal["damageAssistedRadio"];
                myplayer.spotted = (int)personal["spotted"];
                myplayer.damageAssistedStun = (int?)personal["damageAssistedStun"];
                myplayer.stunned = (int?)personal["stunned"];
                myplayer.stunDuration = (int?)personal["stunDuration"];
                myplayer.stunNum = (int?)personal["stunNum"];

                // FLAG
                myplayer.flagActions = personal["flagActions"].ToObject<int[]>();
                myplayer.flagCapture = (int)personal["flagCapture"];
                myplayer.capturePoints = (int)personal["capturePoints"];
                myplayer.droppedCapturePoints = (int)personal["droppedCapturePoints"];
                myplayer.capturingBase = (int?)personal["capturingBase"];
                myplayer.soloFlagCapture = (int)personal["soloFlagCapture"];

         ///////// RECEIVED & BLOCKED

                // SHOTS
                myplayer.noDamageDirectHitsReceived = (int)personal["noDamageDirectHitsReceived"];
                myplayer.directHitsReceived = (int)personal["directHitsReceived"];
                myplayer.piercingsReceived = (int)personal["piercingsReceived"];
                myplayer.explosionHitsReceived = (int)personal["explosionHitsReceived"];

                // DAMAGE
                myplayer.damageReceivedFromInvisibles = (int?)personal["damageReceivedFromInvisibles"];
                myplayer.damageBlockedByArmor = (int)personal["damageBlockedByArmor"];
                myplayer.potentialDamageReceived = (int)personal["potentialDamageReceived"];
                myplayer.damageReceived = (int)personal["damageReceived"];

         ////////// OTHERS

                // GOLD
                myplayer.originalGold = (int)personal["originalGold"];
                myplayer.eventGoldFactor100List = personal["eventGoldFactor100List"].ToObject<int[]>();
                myplayer.eventGoldList = personal["eventGoldList"].ToObject<int[]>();
                myplayer.goldReplay = (int?)personal["goldReplay"];
                myplayer.eventGold = (int)personal["eventGold"];
                myplayer.gold = (int)personal["gold"];
                myplayer.subtotalGold = (int)personal["subtotalGold"];

                // MARKS
                myplayer.markOfMastery = (int)personal["markOfMastery"];
                myplayer.marksOnGun = (int)personal["marksOnGun"];
                myplayer.prevMarkOfMastery = (int)personal["prevMarkOfMastery"];

                // DEATH & HP
                myplayer.deathCount = (int)personal["deathCount"];
                myplayer.lifeTime = (int)personal["lifeTime"];
                myplayer.deathReason = (int)personal["deathReason"];
                myplayer.health = (int)personal["health"];
                myplayer.committedSuicide = (bool)personal["committedSuicide"];
                myplayer.killerID = (int)personal["killerID"];

                // CRYSTAL
                myplayer.originalCrystal = (int?)personal["originalCrystal"];
                myplayer.crystal = (int?)personal["crystal"];
                myplayer.crystalReplay = (int?)personal["crystalReplay"];
                myplayer.subtotalCrystal = (int?)personal["subtotalCrystal"];
                myplayer.eventCrystalList = personal["eventCrystalList"]?.ToObject<string[][]>();
                myplayer.eventCrystal = (int?)personal["eventCrystal"];

                // COSTS
                myplayer.autoEquipCost = personal["autoEquipCost"].ToObject<int[]>();
                myplayer.autoLoadCost = personal["autoLoadCost"].ToObject<int[]>();
                myplayer.autoRepairCost = (int)personal["autoRepairCost"];
                myplayer.repair = (int)personal["repair"];

                // OTHERS
                myplayer.vehTypeLockTime = (int)personal["vehTypeLockTime"];
                myplayer.winPoints = (int)personal["winPoints"];
                myplayer.stopRespawn = (bool)personal["stopRespawn"];
                myplayer.aogasFactor10 = (int)personal["aogasFactor10"];
                myplayer.extPublic = personal["extPublic"];
                myplayer.resourceAbsorbed = (int)personal["resourceAbsorbed"];
                myplayer.fairplayFactor10 = (int)personal["fairplayFactor10"];
                myplayer.serviceProviderID = (int)personal["serviceProviderID"];
                myplayer.typeCompDescr = (int)personal["typeCompDescr"];            
                myplayer.questsProgress = (object)personal["questsProgress"];
                myplayer.achievements = personal["achievements"].ToObject<int[]>();
                myplayer.dossierPopUps = personal["dossierPopUps"].ToObject<object>();
                myplayer.isPremium = (bool)personal["isPremium"];
                myplayer.mileage = (int)personal["mileage"];
                myplayer.rolloutsCount = (int)personal["rolloutsCount"];
                myplayer.index = (int)personal["index"];
                myplayer.accountDBID = (int)personal["accountDBID"];
                myplayer.battleNum = (int)personal["battleNum"];


                // create list for each player where the mainplayer did something
                List<PersonalDetails> personalDetails = new List<PersonalDetails>();

                // for each player in the mainplayers "details"
                foreach (JToken item in personal["details"])
                {
                    // get the right json part for player
                    JObject myitem = JObject.Parse(item.First.ToString());

                    // create instance of PersonalDetails
                    PersonalDetails details = new PersonalDetails();

                    // get the path of the detailed player and set id
                    var path = (string)item.Path.Remove(0, 10);
                    details.id = (string)path.Remove(path.Length - 6, 6);

                    // RECEIVED & BLOCKED
                    details.damageReceived = (int)myitem["damageReceived"];
                    details.rickochetsReceived = (int)myitem["rickochetsReceived"];
                    details.damageBlockedByArmor = (int)myitem["damageBlockedByArmor"];
                    details.noDamageDirectHitsReceived = (int)myitem["noDamageDirectHitsReceived"];

                    // DID
                    details.crits = (int)myitem["crits"];
                    details.fire = (int)myitem["fire"];
                    details.piercings = (int)myitem["piercings"];
                    details.explosionHits = (int)myitem["explosionHits"];
                    details.targetKills = (int)myitem["targetKills"];
                    details.directHits = (int)myitem["directHits"];
                    details.damageDealt = (int)myitem["damageDealt"];

                    // ASISTED
                    details.spotted = (int)myitem["spotted"];
                    details.damageAssistedTrack = (int)myitem["damageAssistedTrack"];
                    details.damageAssistedStun = (int?)myitem["damageAssistedStun"];
                    details.stunDuration = (double?)myitem["stunDuration"];
                    details.stunNum = (int?)myitem["stunNum"];
                    details.damageAssistedRadio = (int)myitem["damageAssistedRadio"];

                    // DEATH
                    details.deathReason = (int)myitem["deathReason"];

                    // add this player to the list
                    personalDetails.Add(details);
                }

                // set the mainplayer details to this list
                myplayer.details = personalDetails;
            }

            // return the mainplayer
            return myplayer;
        }
    }
}
