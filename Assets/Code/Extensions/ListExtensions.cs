using System;
using System.Collections.Generic;
using Random = System.Random;

namespace Assets.Code.Extensions
{
    static class ListExtensions
    {
        private readonly static Random Rand = new Random();

        public static T GetRandomItem<T>(this List<T> input)
        {
            var index = (int) (Rand.NextDouble() * input.Count);
            return input[index > input.Count - 1 ? input.Count - 1 : index];
        }

        // fisher yates from http://stackoverflow.com/a/1262619
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetAndRemove<T>(this IList<T> subject, int index)
        {
            var target = subject[index];
            subject.RemoveAt(index);

            return target;
        }

        public static T GetAndRemoveRandomItem<T>(this IList<T> subject)
        {
            var index = (int)(Rand.NextDouble() * subject.Count);
            var target = subject[index > subject.Count - 1 ? subject.Count - 1 : index];

            subject.RemoveAt(index);
            return target;
        }

        public static bool ReplaceItem<T>(this IList<T> subject, Func<T, bool> predicate, T replacementItem,
                                          bool replaceAllThatMatchPredicate = false) where T : class
        {
            var foundItem = false;

            for (var i = 0; i < subject.Count; i++)
            {
                if (predicate(subject[i]))
                {
                    subject.RemoveAt(i);
                    subject.Insert(i, replacementItem);

                    if(!replaceAllThatMatchPredicate)
                        return true;

                    foundItem = true;
                }
            }

            return foundItem;
        }
    }
}
