using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SecretSanta.Models;
using SecretSanta.Repository;

namespace SecretSanta.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class SecretSantaController : Controller
    {
        private SecretSantaRepository _secretSantaRepository;

        public SecretSantaController()
        {
            _secretSantaRepository = new SecretSantaRepository();
        }

        [HttpPost]
        [Route("CreateSecretSantaContext")]
        public object CreateSecretSantaContext([FromBody]string name)
        {
            var secretSantaContext = new SecretSantaContext
            {
                Id = new Guid(),
                Name = name,
                DateCreated = DateTime.Now

            };
            return Ok(_secretSantaRepository.InsertSecretSantaContext(secretSantaContext));
        }

        [HttpPost]
        [Route("InitializeSecretSantaContext/{contextId}")]
        public object InitializeSecretSantaContext([FromBody]List<Participant> participants, [FromRoute]string contextId)
        {
            
            _secretSantaRepository.InsertParticipants(participants);
            var allParticipants = _secretSantaRepository.GetAllParticipants(new Guid(contextId)).GetAwaiter().GetResult();
            var recipientsAssigned = false;
            while(!recipientsAssigned)
            {
                recipientsAssigned = AssignGiftRecipients(allParticipants);
            }

            return Ok();
        }

        private bool AssignGiftRecipients(List<Participant> participants)
        {
            var remainingParticipants = participants.Count;
            foreach (var participant in participants)
            {
                Random r = new Random();
                while (remainingParticipants > 0)
                {
                    participants = participants.Where(x => !x.isDrawn).ToList();
                    var giftRecipient = participants[r.Next(0, participants.Count)];
                    if (participant.id != giftRecipient.id &&
                        !participant.DisallowedRecipients.Any(x => x.Equals(giftRecipient.name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        participant.giftRecipient = giftRecipient.id;
                        giftRecipient.isDrawn = true;
                        _secretSantaRepository.UpdateParticipant(participant, participant.id);
                        _secretSantaRepository.UpdateParticipant(giftRecipient, giftRecipient.id);
                        remainingParticipants--;
                        break;
                    }
                    else if (participant.id == giftRecipient.id && remainingParticipants == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [HttpGet("{name}")]
        public object GetGiftRecipient(string name)
        {
            var participant = _secretSantaRepository.GetParticipantByName(name).GetAwaiter().GetResult();
            var giftRecipient = _secretSantaRepository.GetParticipant(participant.giftRecipient).GetAwaiter().GetResult(); ;
            return Ok(giftRecipient);
        }
    }
}
