using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardParty
{
    public static class EnumerableHelper
    {
        public static string ToDisplayName(this IEnumerable enumerable, string seperator = ",")
        {
            if (enumerable == null)
                return null;

            string name = null;

            foreach (var current in enumerable)
            {
                if (name != null)
                    name += seperator + " ";

                name += current.ToString();
            }

            return name;
        }
        public static string ToDisplayName<T>(this IEnumerable<T> enumerable, Func<T,string> toDisaplyName, string seperator = ",")
        {
            if (toDisaplyName == null || enumerable == null)
                return null;

            string name = null;

            foreach (var current in enumerable)
            {
                if (name != null)
                    name += seperator + " ";

                name += toDisaplyName.Invoke(current);
            }

            return name;
        }
    }
}
