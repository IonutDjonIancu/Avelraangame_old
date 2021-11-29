using Avelraangame.Models;
using Avelraangame.Models.ViewModels;
using Avelraangame.Services.ServiceUtils;
using Avelraangame.Services.SubService;
using Newtonsoft.Json;
using System;
using System.Linq;
using static Avelraangame.Services.ServiceUtils.CombatUtils;

namespace Avelraangame.Services
{
    public class CombatService : CombatSubService
    {
        #region Business logic
        public string Attack(RequestVm request)
        {
            // determine fight
            var attackvm = ValidateRequestDeserializationInto_AttackVm(request.Message);
            ValidateAttackDetails(attackvm);
            var fightvm = ValidateFightDetails(attackvm);

            // determine attacker
            var attacker = fightvm.GoodGuys.Where(s => s.CharacterId.Equals(attackvm.AttackerId)).FirstOrDefault();
            if (!attacker.AttackToken)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "attacker has no token."));
            }
            if (!attacker.IsAlive)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "attacker is dead."));
            }

            // determine target
            var target = fightvm.BadGuys.Where(s => s.CharacterId.Equals(attackvm.TargetId)).FirstOrDefault();
            if (!target.IsAlive)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "your target is dead already."));
            }

            int dmgDone = 0;
            (attacker, target, dmgDone) = RollAttack(attacker, target);

            if (dmgDone <= 0)
            {
                fightvm.FightDetails.LastActionResult = "You missed";
            }
            else if (dmgDone > 0 && dmgDone <= 200)
            {
                fightvm.FightDetails.LastActionResult = "You cause minor damage";
            }
            else if (dmgDone > 200 && dmgDone <= 1000)
            {
                fightvm.FightDetails.LastActionResult = "You deal some direct strikes";
            }
            else
            {
                fightvm.FightDetails.LastActionResult = "You critically hit";
            }

            fightvm = EndFight(fightvm);

            var fight = DataService.GetFightById(fightvm.FightId);
            fight.GoodGuys = JsonConvert.SerializeObject(fightvm.GoodGuys);
            fight.BadGuys = JsonConvert.SerializeObject(fightvm.BadGuys);
            fight.FightDetails = JsonConvert.SerializeObject(fightvm.FightDetails);

            DataService.UpdateFight(fight);

            return JsonConvert.SerializeObject(fightvm);
        }

        public string Pass(RequestVm request)
        {
            var defendvm = ValidateRequestDeserializationInto_DefendVm(request.Message);
            var fight = DataService.GetFightById(defendvm.FightId);
            if (fight == null)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.BadRequest, "fight id is missing or invalid."));
            }

            var fightvm = new FightVm(fight);
            if (!fightvm.GoodGuys.Where(s => s.PlayerId.Equals(defendvm.PlayerId)).Any())
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "player id has no right of decision on said fight."));
            }

            var charservice = new CharactersService();

            if (fightvm.GoodGuys.Where(s => s.IsAlive && s.AttackToken).Count() <= 0)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "no characters with token found to rest."));
            }

            foreach (var charvm in fightvm.GoodGuys.Where(s => s.IsAlive && s.AttackToken))
            {
                var originalChr = charservice.GetCalculatedCharacterById(charvm.CharacterId);
                charvm.Assets.Health += (originalChr.Assets.Health - charvm.Assets.Health) / 2;
                charvm.AttackToken = false;
            }

            fightvm.FightDetails.LastActionResult = "You take a step a back and mend your wounds";

            fight.GoodGuys = JsonConvert.SerializeObject(fightvm.GoodGuys);
            fight.BadGuys = JsonConvert.SerializeObject(fightvm.BadGuys);
            fight.FightDetails = JsonConvert.SerializeObject(fightvm.FightDetails);

            DataService.UpdateFight(fight);

            return JsonConvert.SerializeObject(fightvm);
        }

        public string Turn(RequestVm request)
        {
            var defendvm = ValidateRequestDeserializationInto_DefendVm(request.Message);
            var fight = DataService.GetFightById(defendvm.FightId);
            if (fight == null)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.BadRequest, "fight id is missing or invalid."));
            }

            var fightvm = new FightVm(fight);
            if(!fightvm.GoodGuys.Where(s => s.PlayerId.Equals(defendvm.PlayerId)).Any())
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "player id has no right of decision on said fight."));
            }

            if (!fightvm.BadGuys.Where(s => s.IsAlive).Any())
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "all enemies defeated."));
            }

            fightvm = RollNpcAttack(fightvm);
            fightvm = ReimburseTokens(fightvm);
            fightvm = EndFight(fightvm);

            fight.GoodGuys = JsonConvert.SerializeObject(fightvm.GoodGuys);
            fight.BadGuys = JsonConvert.SerializeObject(fightvm.BadGuys);
            fight.FightDetails = JsonConvert.SerializeObject(fightvm.FightDetails);

            DataService.UpdateFight(fight);

            return JsonConvert.SerializeObject(fightvm);
        }

        public string JoinParty(RequestVm request)
        {
            var charVm = ValidateRequestDeserializationInto_CharacterVm(request.Message);
            var chr = ValidateCharacterByPlayerId(charVm.CharacterId, charVm.PlayerId);

            if (!chr.IsAlive)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is dead."));
            }
            if (chr.IsInFight)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is fighting."));
            }

            var charService = new CharactersService();
            if (chr.IsInParty)
            {
                charService.LeaveParty(chr);
                return Scribe.ShortMessages.Success.ToString();
            }
            else
            {
                charService.JoinParty(chr);
                return Scribe.ShortMessages.Success.ToString();
            }
        }

        public string StartCombatFromStory(RequestVm request)
        {
            var storyvm = ValidateRequestDeserializationInto_CombatStoryVm(request.Message);

            var requester = ValidateCharacterByPlayerId(storyvm.CharacterId, storyvm.PlayerId);
            if (!requester.IsAlive)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is dead."));
            }
            if (requester.IsInFight)
            {
                throw new Exception(message: string.Join(": ", Scribe.ShortMessages.Failure, "character is already in a fight."));
            }

            var requesterVm = new CharacterVm(requester);

            var act = DataService.GetActById(storyvm.ActId);

            return StartCombat(requesterVm, act.Name);
        }

        public string StartCombat(CharacterVm requester, string renown)
        {
            var fightService = new FightService();

            var fightvm = fightService.StartAFight(requester); // the fight starts here
            fightvm.FightDetails.Renown = renown;

            fightvm.FightDetails.TacticalSituation = DecideTacticalSituation(fightvm.GoodGuys, fightvm.BadGuys);
            fightvm.FightDetails.LastActionResult = fightvm.FightDetails.TacticalSituation;
            //if (fightvm.FightDetails.TacticalSituation.Equals(TacticalSituation.Major_tactical_disadvantage.ToString()) ||
            //    fightvm.FightDetails.TacticalSituation.Equals(TacticalSituation.Slight_tactical_disadvantage.ToString()))
            //{
            //    fightvm = RollNpcAttack(fightvm);
            //}
            //else
            //{
            //}

            var fight = new Fight()
            {
                Id = fightvm.FightId,
                GoodGuys = JsonConvert.SerializeObject(fightvm.GoodGuys),
                BadGuys = JsonConvert.SerializeObject(fightvm.BadGuys),
                FightDetails = JsonConvert.SerializeObject(fightvm.FightDetails)
            };

            DataService.UpdateFight(fight);

            return JsonConvert.SerializeObject(fightvm);
        }
        
        #endregion

        #region Getters
        #endregion


    }
}
