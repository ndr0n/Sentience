using System;
using System.Collections.Generic;
using System.Linq;
using Sentience;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace MindTheatre
{
    [System.Serializable]
    public class Speaker : EntityComponent
    {
        public Action<string> OnAskQuestion;
        public Action<string> OnReceiveAnswer;
        public Action<string> OnReceivePartialReply;
        Sentient sentient;


        public override void OnInit(EntityData data, Random random)
        {
            base.OnInit(data, random);
            sentient = data.TryGet<Sentient>();
        }

        public void StartSpeakingWith(Identity questioner)
        {
            // if (Data.Has<Persona>())
            // {
            // Persona persona = Data.Get<Persona>();
            // persona.RefreshDesire();
            // }

            string question = $"Hello, I am {questioner.Data.Name}, a {questioner.Species.Name} in {questioner.Location}.";
            AskQuestion(questioner.Data.Name, question);
        }

        public void AskQuestion(string questioner, string question)
        {
            OnAskQuestion?.Invoke(question);

            string details = "";
            // if (!string.IsNullOrWhiteSpace(Persona.Desire)) details += $"You desire {Persona.Desire}.\n";

            // ID id = questioner.Data.Get<ID>();
            var awaiter = sentient.AskQuestion(questioner, question, details, OnReceivePartialReply, OnReceiveAnswer);
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