using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardParty
{
    public interface IFactory<T>
    {
        T Create();
    }
}
