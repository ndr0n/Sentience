using System;
using UnityEngine;
using Random = System.Random;

namespace Sentience
{
    [System.Serializable]
    public enum Sentiment
    {
        Hated = 0,
        Disliked = 1,
        Neutral = 2,
        Liked = 3,
        Loved = 4,
    }

    [System.Serializable]
    public class Reputation
    {
        [SerializeField] int val;

        public int Value
        {
            get => val;
            set
            {
                val = value;
                if (val < 0) val = 0;
                if (val > 100) val = 100;
            }
        }

        public Sentiment Sentiment
        {
            get
            {
                return Value switch
                {
                    < 20 => Sentiment.Hated,
                    < 40 => Sentiment.Disliked,
                    < 60 => Sentiment.Neutral,
                    < 80 => Sentiment.Liked,
                    _ => Sentiment.Loved
                };
            }
        }

        public Reputation(int value)
        {
            val = value;
        }

        public Reputation(Sentiment status)
        {
            switch (status)
            {
                case Sentiment.Hated:
                    val = UnityEngine.Random.Range(0, 20);
                    break;
                case Sentiment.Disliked:
                    val = UnityEngine.Random.Range(20, 40);
                    break;
                case Sentiment.Neutral:
                    val = UnityEngine.Random.Range(40, 60);
                    break;
                case Sentiment.Liked:
                    val = UnityEngine.Random.Range(60, 80);
                    break;
                case Sentiment.Loved:
                    val = UnityEngine.Random.Range(80, 100);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public void ModifyRelationship(int value)
        {
            Value += value;
        }

        public void ModifyRelationship(Sentiment value)
        {
            switch (value)
            {
                case Sentiment.Hated:
                    ModifyRelationship(-10);
                    break;
                case Sentiment.Disliked:
                    ModifyRelationship(-5);
                    break;
                case Sentiment.Neutral:
                    ModifyRelationship(0);
                    break;
                case Sentiment.Liked:
                    ModifyRelationship(5);
                    break;
                case Sentiment.Loved:
                    ModifyRelationship(10);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}