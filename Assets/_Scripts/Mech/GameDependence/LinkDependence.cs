using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Mech
{
    public class LinkDependece<TLink, T> : GameDependence<T>
    {
        public LinkDependece(object mainObject, Dictionary<TLink, T> container, Func<List<float>, float> calculationFuncion, params object[] links)
        {
            CalculationFunction = calculationFuncion;
            argumentsOfType = new List<T>();
            allArguments = new List<object>();

            foreach (var link in links)
            {
                object argument;

                if (link.GetType() == typeof(TLink))
                {
                    var value = container[(TLink)link];
                    argument = value;
                    argumentsOfType.Add(value);
                }
                else
                {
                    var getterMethodName = mainObject.GetType().GetProperty(link.ToString()).GetGetMethod().Name;
                    argument = Delegate.CreateDelegate(typeof(Func<float>), mainObject, getterMethodName);
                }

                allArguments.Add(argument);
            }

        }

        public LinkDependece(Dictionary<TLink, T> container, Func<List<float>, float> calculationFuncion, params object[] links)
            : this(null, container, calculationFuncion, links)
        {

        }
    }
}
