using System;
using System.Collections.Generic;
using System.Linq;
using Sentience;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace MindTheatre
{
    public class Speaker : EntityComponent
    {
        public Action<string> OnAskQuestion;
        public Action<string> OnReceiveAnswer;

        Sentient sentient;

        public override void OnSpawn(EntitySpawn spawn)
        {
            base.OnSpawn(spawn);
            if (Data.Has<Identity>())
            {
                Identity identity = Data.Get<Identity>();
                if (spawn.TryGetComponent(out Sentient _sentient))
                {
                    sentient = _sentient;
                    sentient.Init(identity);
                }
            }
        }

        public void StartSpeakingWith(Identity questioner)
        {
            string question = $"Hello, I am {questioner.Data.Name}, a {questioner.Species.Name} in {questioner.Location}.";
            AskQuestion(questioner, question);
        }

        public void AskQuestion(Identity questioner, string question)
        {
            OnAskQuestion?.Invoke(question);
            Debug.Log($"{questioner} ASKED QUESTION TO {Data.Name}: {question}");

            string details = "";
            // if (!string.IsNullOrWhiteSpace(Persona.Desire)) details += $"You desire {Persona.Desire}.\n";

            ID id = questioner.Data.Get<ID>();
            var awaiter = sentient.AskQuestion(id.Name, question, details, null, OnReceiveAnswer);
        }
    }

    [System.Serializable]
    public class SpeakerAuthoring : EntityAuthoring
    {
        public override IEntityComponent Spawn(System.Random random)
        {
            Speaker speaker = new();
            return speaker;
        }
    }
}